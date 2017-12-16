using Newtonsoft.Json;
using System;
using System.IO;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class Settings : ISettings
    {
        private const string SettingsFilename = "settings.json";

        public string FileNamingPattern { get; set; }

        public string PlayerUsername { get; set; }

        public string ReplayDirectoryPath { get; set; }

        public void Persist()
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

            var persistedSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFilename));

            FileNamingPattern = persistedSettings.FileNamingPattern;
            PlayerUsername = persistedSettings.PlayerUsername;
            ReplayDirectoryPath = persistedSettings.ReplayDirectoryPath;
        }
    }
}
