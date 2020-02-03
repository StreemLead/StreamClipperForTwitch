using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Twitch_CC.Models
{
    public class StreamerChannelModel : INotifyPropertyChanged
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

        private bool _isfavorite;
        public bool IsFavorite
        {
            get
            {
                return _isfavorite;
            }
            set
            {
                _isfavorite = value;
                OnPropertyRaised("IsFavorite");
            }
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyRaised("Image");
            }
        }
    }
}
