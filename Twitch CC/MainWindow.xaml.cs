using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Twitch_CC.Views;
using TwitchLib.Api;

namespace Twitch_CC
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TwitchAPI client;
        FavoritViewModel favoritViewModel;

        public MainWindow()
        {
            InitializeComponent();

            var client = new TwitchAPI();
            client.Settings.ClientId = "lyph2sgrvpz83iw9pl8w2l3defvywf";
            SnackbarMessageQueue MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(8000));
            snackBar.MessageQueue = MessageQueue;
            MainFrame.Content = new FavoritView(client, MessageQueue);
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
