using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Twitch_CC.Models;
using Twitch_CC.ViewModels.Commands;
using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Clips;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using System.Threading;
using System.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace Twitch_CC.ViewModels
{
    public class ClipViewModel
    {
        public StreamerSearchClipsModel searchQuery { get; set; }
        public ClipCommandRelay downloadCommandRelay { get; private set; }
        public ClipCommandRelay watchCommandRelay { get; private set; }
        public DefaultCommandRelay selectDownloadFolderCommandRelay { get; private set; }
        public DefaultCommandRelay selectAllClipsCommandRelay { get; private set; }
        public DefaultCommandRelay downloadSelectedClipsCommandRelay { get; private set; }
        public DefaultCommandRelay refreshClipsCommandRelay { get; private set; }
        public DefaultCommandRelay navigateBackCommandRelay { get; private set; }
        public ObservableCollection<StreamerClipModel> lClips { get; private set; }
        public ObservableCollection<string> periods { get; private set; }
        public bool allClipsSelected { get; private set; }
        public bool isLoadingClips { get; private set; }
        public string downloadFolder { get; private set; }
        public ClipsTitleModel clipsTitle { get; private set; }
        private TwitchAPI _client;
        private SnackbarMessageQueue _messageQueue;
        private StreamerChannelModel _channel;
        private CancellationTokenSource ctsSearch;
        private string lastPeriod = "";
        private bool isDownloading = false;

        public ClipViewModel(TwitchAPI client, StreamerChannelModel channel, SnackbarMessageQueue messageQueue)
        {
            downloadCommandRelay = new ClipCommandRelay(Download);
            watchCommandRelay = new ClipCommandRelay(Watch);
            selectDownloadFolderCommandRelay = new DefaultCommandRelay(SelectDownloadFolder);
            selectAllClipsCommandRelay = new DefaultCommandRelay(SelectAllClips);
            downloadSelectedClipsCommandRelay = new DefaultCommandRelay(DownloadSelectedClips);
            refreshClipsCommandRelay = new DefaultCommandRelay(SearchClips);
            navigateBackCommandRelay = new DefaultCommandRelay(NavigateBack);
            lClips = new ObservableCollection<StreamerClipModel>();
            searchQuery = new StreamerSearchClipsModel();
            periods = new ObservableCollection<string>() { "Day", "Week", "Month", "All" };
            clipsTitle = new ClipsTitleModel() { Title = "Clips of " + channel.Name };

            _client = client;
            _channel = channel;
            _messageQueue = messageQueue;

            downloadFolder = KnownFolders.Downloads.Path;
            allClipsSelected = false;
            isLoadingClips = false;

            searchQuery.Name = channel.Name;
            searchQuery.Period = "Day";
            lastPeriod = "";
            SearchClips();
        }

        public void NavigateBack()
        {
            ctsSearch.Cancel();
            var mainWindow = Application.Current.Windows[0] as MainWindow;
            mainWindow.MainFrame.GoBack();
        }
        public void SearchClips()
        {
            if (isLoadingClips == false && searchQuery.Period != lastPeriod && !isDownloading)
            {
                searchQuery.Cursor = "";
                lastPeriod = searchQuery.Period;
                isLoadingClips = true;
                lClips.Clear();
                clipsTitle.Title = "Clips of " + _channel.Name;
                ctsSearch = new CancellationTokenSource();
                CancellationToken token = ctsSearch.Token;

                Task.Run(async () =>
                {
                    TwitchLib.Api.V5.Models.Clips.Period period = 0;
                    if (searchQuery.Period == "Day")
                        period = TwitchLib.Api.V5.Models.Clips.Period.Day;
                    else if (searchQuery.Period == "Week")
                        period = TwitchLib.Api.V5.Models.Clips.Period.Week;
                    else if (searchQuery.Period == "Month")
                        period = TwitchLib.Api.V5.Models.Clips.Period.Month;
                    else if (searchQuery.Period == "All")
                        period = TwitchLib.Api.V5.Models.Clips.Period.All;

                    while (true)
                    {
                        token.ThrowIfCancellationRequested();

                        var clips = await _client.V5.Clips.GetTopClipsAsync(searchQuery.Name, searchQuery.Cursor, null, 10, period);
                        
                        if (clips.Cursor == "" && clips.Clips.Count() == 0 && lClips.Count() == 0)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                clipsTitle.Title = "nothing found.";
                            }));
                            break;
                        }
                        else if (clips.Cursor == "")
                            break;
                        else
                            searchQuery.Cursor = clips.Cursor;



                        foreach (Clip clip in clips.Clips)
                        {
                            token.ThrowIfCancellationRequested();

                            var clipFormatted = new StreamerClipModel();
                            clipFormatted.Name = clip.Title;
                            clipFormatted.Downloadlink = clip.Thumbnails.Medium.Split(new string[] { "-preview" }, StringSplitOptions.None)[0] + ".mp4";
                            clipFormatted.Views = clip.Views;
                            clipFormatted.CreatedAt = clip.CreatedAt;
                            clipFormatted.PreviewLink = clip.EmbedUrl;

                            try
                            { clipFormatted.VOD = clip.VOD.Url; }
                            catch
                            { clipFormatted.VOD = ""; }

                            clipFormatted.Thumbnail = clip.Thumbnails.Small;

                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                try
                                {
                                    clipFormatted.ThumbnailBitmap = new BitmapImage();
                                    clipFormatted.ThumbnailBitmap.BeginInit();
                                    clipFormatted.ThumbnailBitmap.UriSource = new Uri(clipFormatted.Thumbnail, UriKind.Absolute);
                                    clipFormatted.ThumbnailBitmap.DecodePixelWidth = 200;
                                    clipFormatted.ThumbnailBitmap.DecodePixelHeight = 160;
                                    clipFormatted.ThumbnailBitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    clipFormatted.ThumbnailBitmap.EndInit();
                                }
                                catch
                                {
                                    Debug.WriteLine("ERROR Loading => " + clipFormatted.Thumbnail);
                                }
                                lClips.Add(clipFormatted);
                            }));
                        }
                    }

                    isLoadingClips = false;
                    Debug.WriteLine("Done...");
                }, token);
            }
            else if (isDownloading)
            {
                searchQuery.Period = lastPeriod;
                _messageQueue.Enqueue("Please wait. Download is in progress.");
            }
            else
            {
                isLoadingClips = false;
                ctsSearch.Cancel();
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        SearchClips();
                    }));
                });
            }
        }
        public void SelectDownloadFolder()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Set Download Folder";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = KnownFolders.Downloads.Path;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = KnownFolders.Downloads.Path;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                downloadFolder = dlg.FileName;
        }
        public void SelectAllClips()
        {
            if (allClipsSelected == false)
            {
                allClipsSelected = true;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    foreach(StreamerClipModel clip in lClips)
                        clip.Selected = true;
                }));
            }
            else
            {
                allClipsSelected = false;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (StreamerClipModel clip in lClips)
                        clip.Selected = false;
                }));
            }
        }
        public void Download(StreamerClipModel clip)
        {
            if (!isDownloading)
            {
                isDownloading = true;
                _messageQueue.Enqueue("Download started!");

                Task.Run(() =>
                {
                    using (WebClient wbCL = new WebClient())
                        wbCL.DownloadFile(clip.Downloadlink, downloadFolder + "\\" + EscapeSpecialChars(clip.Name) + "_" + GetTimestamp() + ".mp4");

                    _messageQueue.Enqueue("Download finished!");
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        isDownloading = false;
                    }));
                });
            }
            else
            {
                _messageQueue.Enqueue("Please wait. Download is in progress.");
            }
        }
        public void DownloadSelectedClips()
        {
            if (!isDownloading)
            {
                isDownloading = true;
                _messageQueue.Enqueue("Download started!");
                Task.Run(() =>
                {
                    using (WebClient wbCL = new WebClient())
                        foreach (StreamerClipModel clip in lClips)
                            if (clip.Selected == true)
                                wbCL.DownloadFile(clip.Downloadlink, downloadFolder + "\\" + EscapeSpecialChars(clip.Name) + "_" + GetTimestamp() + ".mp4");

                    _messageQueue.Enqueue("Download finished!");
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        isDownloading = false;
                    }));
                });
            }
            else
            {
                _messageQueue.Enqueue("Please wait. Download is in progress.");
            }
        }
        public void Watch(StreamerClipModel clip)
        {
            Process.Start(clip.PreviewLink);
        }
        public string EscapeSpecialChars(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ' || c == '#')
                    sb.Append(c);

            return sb.ToString();
        }
        public string GetTimestamp()
        {
            var timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return timestamp.ToString();
        }
    }

    public class ClipsTitleModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyRaised("Title");
            }
        }
    }
}
