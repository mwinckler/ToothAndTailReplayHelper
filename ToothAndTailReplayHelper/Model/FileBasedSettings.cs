using Newtonsoft.Json;
using System;
using System.IO;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class FileBasedSettings : Settings
    {
        private const string SettingsFilename = "settings.json";

        public override void Persist()
        {
            File.WriteAllText(SettingsFilename, JsonConvert.SerializeObject(this));
        }

        internal void Initialize()
        {
            if (!File.Exists(SettingsFilename))
            {
                FileNamingPattern = "{Date:yyyyMMddTHHmm} {Players}";
                PlayerUsername = "";
                ReplayDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ToothAndTail", "replays");

                return;
            }

            var persistedSettings = JsonConvert.DeserializeObject<FileBasedSettings>(File.ReadAllText(SettingsFilename));

            FileNamingPattern = persistedSettings.FileNamingPattern;
            PlayerUsername = persistedSettings.PlayerUsername;
            ReplayDirectoryPath = persistedSettings.ReplayDirectoryPath;
            AutoArchiveOldReplays = persistedSettings.AutoArchiveOldReplays;
        }
    }
}
