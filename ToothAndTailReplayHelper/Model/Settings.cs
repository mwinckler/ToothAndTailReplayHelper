using System;

namespace ToothAndTailReplayHelper.Model
{
    internal abstract class Settings : ISettings
    {
        public string FileNamingPattern { get; set; }

        public string PlayerUsername { get; set; }

        public string ReplayDirectoryPath { get; set; }

        public bool AutoArchiveOldReplays { get; set; }

        public abstract void Persist();
    }
}
