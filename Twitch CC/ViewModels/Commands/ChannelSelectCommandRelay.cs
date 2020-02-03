using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Twitch_CC.Models;

namespace Twitch_CC.ViewModels.Commands
{
    public class AddFavoriteCommandRelay : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<StreamerChannelModel> _execute;

        public AddFavoriteCommandRelay(Action<StreamerChannelModel> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter as StreamerChannelModel);
        }
    }
}
