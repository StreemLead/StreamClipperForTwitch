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
using Twitch_CC.ViewModels;
using TwitchLib.Api;

namespace Twitch_CC.Views
{
    /// <summary>
    /// Interaktionslogik für FavoritView.xaml
    /// </summary>
    public partial class FavoritView : Page
    {
        public FavoritView(TwitchAPI client, SnackbarMessageQueue messageQueue)
        {
            InitializeComponent();

            var favoritViewModel = new FavoritViewModel(client, messageQueue);
            DataContext = favoritViewModel;
        }
    }
}
