using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Twitch_CC.Models
{
    public class StreamerClipModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyRaised("Name");
            }
        }

        private string _downloadlink;
        public string Downloadlink
        {
            get
            {
                return _downloadlink;
            }
            set
            {
                _downloadlink = value;
                OnPropertyRaised("Downloadlink");
            }
        }

        private string _previewlink;
        public string PreviewLink
        {
            get
            {
                return _previewlink;
            }
            set
            {
                _previewlink = value;
                OnPropertyRaised("PreviewLink");
            }
        }

        private int _views;
        public int Views
        {
            get
            {
                return _views;
            }
            set
            {
                _views = value;
                OnPropertyRaised("Views");
            }
        }

        private bool _selected = false;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                OnPropertyRaised("Selected");
            }
        }

        private string _thumbnail;
        public string Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                _thumbnail = value;
                OnPropertyRaised("Thumbnail");
            }
        }

        private DateTime _createdat;
        public DateTime CreatedAt
        {
            get
            {
                return _createdat;
            }
            set
            {
                _createdat = value;
                OnPropertyRaised("CreatedAt");
            }
        }

        private string _vod;
        public string VOD
        {
            get
            {
                return _vod;
            }
            set
            {
                _vod = value;
                OnPropertyRaised("VOD");
            }
        }

        private BitmapImage _thumbnailbitmap;
        public BitmapImage ThumbnailBitmap
        {
            get
            {
                return _thumbnailbitmap;
            }
            set
            {
                _thumbnailbitmap = value;
                OnPropertyRaised("ThumbnailBitmap");
            }
        }
    }
}
