using System;
using System.Diagnostics;
using System.IO;
using ToothAndTailReplayHelper.Model;

namespace ToothAndTailReplayHelper.Command
{
    internal sealed class OpenReplayFolderCommand : IOpenReplayFolderCommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly ISettings settings;

        public OpenReplayFolderCommand(ISettings settings)
        {
            this.settings = settings;
        }

        public bool CanExecute(object parameter)
        {
            return Directory.Exists(settings.ReplayDirectoryPath);
        }

        public void Execute(object parameter)
        {
            Process.Start(settings.ReplayDirectoryPath);
        }
    }
}
