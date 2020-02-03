using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Twitch_CC.ViewModels.Commands
{
    public class LoadFavoritesCommandRelay : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<TextBox> _execute;

        public LoadFavoritesCommandRelay(Action<TextBox> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter as TextBox);
        }
    }
}
