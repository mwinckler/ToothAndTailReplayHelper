using System;
using System.ComponentModel;
using System.Windows.Input;
using ToothAndTailReplayHelper.Model;

namespace ToothAndTailReplayHelper.ViewModel
{
    internal sealed class SettingsWindow : ISettingsWindow, INotifyPropertyChanged
    {
        public event EventHandler ShowRequested;
        public event EventHandler HideRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public string ReplayDirectoryPath
        {
            get => replayDirectoryPath;
            set
            {
                replayDirectoryPath = value;
                OnPropertyChanged(nameof(ReplayDirectoryPath));
            }
        }

        public string FileNamingPattern {
            get => fileNamingPattern;
            set
            {
                fileNamingPattern = value;
                OnPropertyChanged(nameof(FileNamingPattern));
            }
        }

        public string PlayerUsername
        {
            get => playerUsername;
            set
            {
                playerUsername = value;
                OnPropertyChanged(nameof(PlayerUsername));
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        public bool IsValid
        {
            get => true;
        }

        private ISettings settings;
        private string replayDirectoryPath;
        private string fileNamingPattern;
        private string playerUsername;

        public SettingsWindow(ISettings settings)
        {
            this.settings = settings;

            SaveCommand = new Command.Command(_ => IsValid, _ => Save());
            CancelCommand = new Command.Command(_ => true, _ => Cancel());

            LoadPropertiesFromSettings();
        }

        public void Show()
        {
            LoadPropertiesFromSettings();
            ShowRequested?.Invoke(this, EventArgs.Empty);
        }

        public void Hide()
        {
            HideRequested?.Invoke(this, EventArgs.Empty);
        }

        public void Save()
        {
            settings.ReplayDirectoryPath = ReplayDirectoryPath;
            settings.FileNamingPattern = FileNamingPattern;
            settings.PlayerUsername = PlayerUsername;
            settings.Persist();
            Hide();
        }

        public void Cancel()
        {
            LoadPropertiesFromSettings();

            Hide();
        }

        private void LoadPropertiesFromSettings()
        {
            ReplayDirectoryPath = settings.ReplayDirectoryPath;
            FileNamingPattern = settings.FileNamingPattern;
            PlayerUsername = settings.PlayerUsername;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
