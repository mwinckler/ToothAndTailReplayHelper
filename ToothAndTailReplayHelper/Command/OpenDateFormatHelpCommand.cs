using System;
using System.Diagnostics;

namespace ToothAndTailReplayHelper.Command
{
    internal sealed class OpenDateFormatHelpCommand : IOpenDateFormatHelpCommand
    {
        private const string HelpUri = @"https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Process.Start(HelpUri);
        }
    }
}