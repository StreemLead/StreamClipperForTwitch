using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twitch_CC.Models;
using Twitch_CC.ViewModels;
using TwitchLib.Api;

namespace Twitch_CC.Views
{
    /// <summary>
    /// Interaktionslogik für ClipView.xaml
    /// </summary>
    public partial class ClipView : Page
    {
        public ClipView(TwitchAPI client, StreamerChannelModel channel, SnackbarMessageQueue messageQueue)
        {
            InitializeComponent();

            var clipViewModel = new ClipViewModel(client, channel, messageQueue);
            DataContext = clipViewModel;
        }
    }
}
