using System;
using System.Windows;

namespace ToothAndTailReplayHelper.Command
{
    internal sealed class ExitApplicationCommand : IExitApplicationCommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
