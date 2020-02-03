using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_CC.Models
{
    public class StreamerSearchClipsModel : INotifyPropertyChanged
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

        private string _period;
        public string Period
        {
            get
            {
                return _period;
            }
            set
            {
                _period = value;
                OnPropertyRaised("Period");
            }
        }

        private string _startdate;
        public string StartDate
        {
            get
            {
                return _startdate;
            }
            set
            {
                _startdate = value;
                OnPropertyRaised("StartDate");
            }
        }

        private string _enddate;
        public string EndDate
        {
            get
            {
                return _enddate;
            }
            set
            {
                _enddate = value;
                OnPropertyRaised("EndDate");
            }
        }

        private string _cursor;
        public string Cursor
        {
            get
            {
                return _cursor;
            }
            set
            {
                _cursor = value;
                OnPropertyRaised("Cursor");
            }
        }
    }
}
