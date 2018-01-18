using System;
using System.Diagnostics;

namespace ToothAndTailReplayHelper.Command
{
    internal sealed class OpenFilenamePatternHelpCommand : IOpenFilenamePatternHelpCommand
    {
        private const string HelpUri = @"https://github.com/mwinckler/ToothAndTailReplayHelper/blob/master/README.md";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Process.Start(HelpUri);
        }
    }
}