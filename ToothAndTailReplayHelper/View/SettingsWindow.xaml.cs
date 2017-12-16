using System.ComponentModel;
using System.Windows;
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
    }
}
