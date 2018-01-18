using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ToothAndTailReplayHelper.ViewModel;

namespace ToothAndTailReplayHelper.View
{
    public partial class SettingsWindow : Window
    {
        private ISettingsWindow settingsWindow;

        public SettingsWindow(ISettingsWindow settingsWindow)
        {
            InitializeComponent();

            this.settingsWindow = settingsWindow;

            DataContext = settingsWindow;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;
            settingsWindow.Cancel();
        }

        private void FileNamingPatternKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((Control)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            settingsWindow.UpdateSampleFilename();
        }
    }
}
