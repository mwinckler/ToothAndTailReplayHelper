namespace ToothAndTailReplayHelper.Model
{
    internal interface ISettings
    {
        string ReplayDirectoryPath { get; set; }
        string FileNamingPattern { get; set; }
        string PlayerUsername { get; set; }
        bool AutoArchiveOldReplays { get; set; }

        void Persist();
    }
}
