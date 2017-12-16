namespace ToothAndTailReplayHelper.Model
{
    internal interface ISettings
    {
        string ReplayDirectoryPath { get; set; }
        string FileNamingPattern { get; set; }
        string PlayerUsername { get; set; }

        void Persist();
    }
}
