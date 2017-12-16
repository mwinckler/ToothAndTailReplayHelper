using System;
using ToothAndTailReplayHelper.ViewModel;

namespace ToothAndTailReplayHelper.Command
{
    internal sealed class OpenSettingsCommand : IOpenSettingsCommand
    {
        public event EventHandler CanExecuteChanged;

        private ISettingsWindow settingsWindow;

        public OpenSettingsCommand(ISettingsWindow settingsWindow)
        {
            this.settingsWindow = settingsWindow;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            settingsWindow.Show();
        }
    }
}
