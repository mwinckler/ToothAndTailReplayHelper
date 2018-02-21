using System;
using System.IO;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class ReplayArchiver : IReplayArchiver
    {
        private const string ArchiveDirectoryName = "archive";

        private readonly ISettings settings;
        private readonly IReplayParser replayParser;

        public ReplayArchiver(ISettings settings, IReplayParser replayParser)
        {
            this.settings = settings;
            this.replayParser = replayParser;
        }

        public void ArchiveOldReplays()
        {
            try
            {
                if (!settings.AutoArchiveOldReplays)
                {
                    return;
                }

                var replayDirectory = new DirectoryInfo(settings.ReplayDirectoryPath);
                var lastReplay = new FileInfo(Path.Combine(replayDirectory.FullName, Constants.LastReplayFileName));

                if (!lastReplay.Exists)
                {
                    return;
                }

                var currentVersion = replayParser.GetVersion(lastReplay.FullName);

                if (string.IsNullOrEmpty(currentVersion))
                {
                    return;
                }

                foreach (var replayFile in replayDirectory.GetFiles("*.xml", SearchOption.TopDirectoryOnly))
                {
                    var fileVersion = replayParser.GetVersion(replayFile.FullName);

                    if (CompareSemanticVersions(fileVersion, currentVersion) < 0)
                    {
                        ArchiveFile(replayFile.FullName, fileVersion);
                    }
                }
            }
            catch (Exception)
            {
                // TODO: Log. For now: suppress.
            }
        }

        private int CompareSemanticVersions(string x, string y)
        {
            var xParts = x.Split('.');
            var yParts = y.Split('.');

            for (var i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
            {
                if (!int.TryParse(yParts[i], out int yInt))
                {
                    return 1;
                }

                if (!int.TryParse(xParts[i], out int xInt))
                {
                    return -1;
                }

                if (xInt == yInt)
                {
                    continue;
                }

                return xInt.CompareTo(yInt);
            }

            return xParts.Length.CompareTo(yParts.Length);
        }

        private void ArchiveFile(string filePath, string fileVersion)
        {
            var archiveDir = new DirectoryInfo(Path.Combine(settings.ReplayDirectoryPath, ArchiveDirectoryName));

            if (!archiveDir.Exists)
            {
                archiveDir.Create();
            }

            var versionDirPath = Path.Combine(archiveDir.FullName, fileVersion);

            if (!Directory.Exists(versionDirPath)) {
                archiveDir.CreateSubdirectory(fileVersion);
            }

            File.Move(filePath, Path.Combine(versionDirPath, Path.GetFileName(filePath)));
        }
    }
}
