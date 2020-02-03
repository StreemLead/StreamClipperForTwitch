using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Twitch_CC.Models;
using Twitch_CC.ViewModels.Commands;
using Twitch_CC.Views;
using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Channels;
using TwitchLib.Api.V5.Models.Search;

namespace Twitch_CC.ViewModels
{
    public class FavoritViewModel
    {
        public ChannelSearchCommandRelay channelSearchCommandRelay { get; private set; }
        public ChannelSelectCommandRelay channelSelectCommandRelay { get; private set; }
        public LoadFavoritesCommandRelay loadFavoritesCommandRelay { get; private set; }
        public AddFavoriteCommandRelay addFavoriteCommandRelay { get; private set; }
        public ObservableCollection<StreamerChannelModel> channels { get; private set; }
        public ObservableCollection<StreamerChannelModelJSON> favorites { get; private set; }
        public string favoritesFilePath { get; private set; } = Environment.CurrentDirectory + @"\settings.txt";
        public SearchChannelTitleModel searchTitle { get; private set; }
        private TwitchAPI _client;
        private SnackbarMessageQueue _messageQueue;
        private CancellationTokenSource ctsFavorites;
        private CancellationTokenSource ctsSearch;

        public FavoritViewModel(TwitchAPI client, SnackbarMessageQueue messageQueue)
        {
            channelSearchCommandRelay = new ChannelSearchCommandRelay(SearchChannels);
            channelSelectCommandRelay = new ChannelSelectCommandRelay(SelectChannel);
            loadFavoritesCommandRelay = new LoadFavoritesCommandRelay(LoadFavorites);
            addFavoriteCommandRelay = new AddFavoriteCommandRelay(AddToFavorites);
            channels = new ObservableCollection<StreamerChannelModel>();

            _client = client;
            _messageQueue = messageQueue;

            searchTitle = new SearchChannelTitleModel();
            searchTitle.Title = "Favorites";

            favorites = new ObservableCollection<StreamerChannelModelJSON>();
            LoadFavorites();
        }

        public void SearchChannels(string name)
        {
            searchTitle.Title = "Channels";
            channels.Clear();
            ctsFavorites.Cancel();
            ctsSearch = new CancellationTokenSource();
            CancellationToken token = ctsSearch.Token;

            Task.Run(async () =>
            {
                SearchChannels channelInfos = await _client.V5.Search.SearchChannelsAsync(name);
                foreach (Channel channelInfo in channelInfos.Channels)
                {
                    token.ThrowIfCancellationRequested();

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        var channelModel = new StreamerChannelModel();
                        channelModel.Name = channelInfo.Name;
                        channelModel.Thumbnail = channelInfo.Logo;
                        try
                        {
                            channelModel.Image = new BitmapImage();
                            channelModel.Image.BeginInit();
                            channelModel.Image.UriSource = new Uri(channelInfo.Logo, UriKind.Absolute);
                            channelModel.Image.EndInit();
                        }
                        catch { }

                        if (favorites.Any(fav => fav.Name == channelModel.Name))
                            channelModel.IsFavorite = true;
                        else
                            channelModel.IsFavorite = false;

                        channels.Add(channelModel);
                    }));
                }
            }, token);
        }
        public void LoadFavorites(TextBox searchBox)
        {
            ctsSearch.Cancel();
            searchBox.Text = "";
            LoadFavorites();
        }
        public void SelectChannel(StreamerChannelModel channel)
        {
            var mainWindow = Application.Current.Windows[0] as MainWindow;
            mainWindow.MainFrame.Content = new ClipView(_client, channel, _messageQueue);
        }
        public void AddToFavorites(StreamerChannelModel channel)
        {
            if (!favorites.Any(fav => fav.Name == channel.Name))
            {
                favorites.Add(new StreamerChannelModelJSON() { Name = channel.Name, Thumbnail = channel.Thumbnail });
                File.WriteAllText(favoritesFilePath, JsonConvert.SerializeObject(favorites));
            }
            else
            {
                favorites.Remove(favorites.FirstOrDefault(fav => fav.Name == channel.Name));
                File.WriteAllText(favoritesFilePath, JsonConvert.SerializeObject(favorites));
            }
        }
        public void LoadFavorites()
        {
            if (File.Exists(favoritesFilePath))
            {
                try
                {
                    favorites = JsonConvert.DeserializeObject<ObservableCollection<StreamerChannelModelJSON>>(File.ReadAllText(favoritesFilePath));
                }
                catch
                {
                    favorites = new ObservableCollection<StreamerChannelModelJSON>();
                }
            }

            channels.Clear();
            searchTitle.Title = "Favorites";
            ctsFavorites = new CancellationTokenSource();
            CancellationToken token = ctsFavorites.Token;

            Task.Run(() =>
            {
                foreach (StreamerChannelModelJSON ch in favorites)
                {
                    token.ThrowIfCancellationRequested();

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        var channelModel = new StreamerChannelModel();
                        channelModel.Name = ch.Name;
                        channelModel.Thumbnail = ch.Thumbnail;
                        try
                        {
                            channelModel.Image = new BitmapImage();
                            channelModel.Image.BeginInit();
                            channelModel.Image.UriSource = new Uri(ch.Thumbnail, UriKind.Absolute);
                            channelModel.Image.EndInit();
                        }
                        catch { }

                        channelModel.IsFavorite = true;
                        channels.Add(channelModel);
                    }));
                }
            }, token);
        }
    }

    public class SearchChannelTitleModel : INotifyPropertyChanged
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
