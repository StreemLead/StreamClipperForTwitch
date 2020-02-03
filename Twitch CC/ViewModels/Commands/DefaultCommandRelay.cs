using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Twitch_CC.ViewModels.Commands
{
    public class DefaultCommandRelay : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public DefaultCommandRelay(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
