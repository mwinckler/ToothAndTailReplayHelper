using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using ToothAndTailReplayHelper.Command;
using ToothAndTailReplayHelper.Model;

namespace ToothAndTailReplayHelper.ViewModel
{
    internal sealed class SettingsWindow : ISettingsWindow, INotifyPropertyChanged
    {
        private const string SampleReplayFilename = @"Resources\LastReplay.xml";

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

        public string SampleFilename
        {
            get => sampleFilename;
            set
            {
                sampleFilename = value;
                OnPropertyChanged(nameof(SampleFilename));
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand OpenReplayFolderCommand { get; }

        public ICommand OpenFilenamePatternHelpCommand { get; }

        public bool IsValid
        {
            get => true;
        }

        private ISettings settings;
        private string replayDirectoryPath;
        private string fileNamingPattern;
        private string playerUsername;
        private string sampleFilename;

        private readonly IFilenameGenerator filenameGenerator;

        public SettingsWindow(
            ISettings settings,
            IOpenReplayFolderCommand openReplayFolderCommand,
            IOpenFilenamePatternHelpCommand openFilenamePatternHelpCommand,
            IFilenameGenerator filenameGenerator
        )
        {
            this.settings = settings;
            this.filenameGenerator = filenameGenerator;

            SaveCommand = new Command.Command(_ => IsValid, _ => Save());
            CancelCommand = new Command.Command(_ => true, _ => Cancel());
            OpenReplayFolderCommand = openReplayFolderCommand;
            OpenFilenamePatternHelpCommand = openFilenamePatternHelpCommand;

            LoadPropertiesFromSettings();
            UpdateSampleFilename();
        }

        public void UpdateSampleFilename()
        {
            var sampleReplayFilePath = new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SampleReplayFilename));

            SampleFilename = filenameGenerator.GenerateFilename(sampleReplayFilePath, new SampleSettings(FileNamingPattern, PlayerUsername));
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

        private sealed class SampleSettings : Settings
        {
            public SampleSettings(string fileNamingPattern, string playerUsername)
            {
                FileNamingPattern = fileNamingPattern;
                PlayerUsername = playerUsername;
            }

            public override void Persist()
            {
            }
        }
    }
}
