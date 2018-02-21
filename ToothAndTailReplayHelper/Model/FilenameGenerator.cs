using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class FilenameGenerator : IFilenameGenerator
    {
        private const string PlayerIdentityNodeName  = "Identity";
        private const string PlayerNameAttributeName = "Name";
        private const string DurationNodeName        = "MatchTime";

        private readonly ISettings settings;
        private readonly IFilenameTokenParser filenameTokenParser;
        private readonly IReplayParser replayParser;

        public FilenameGenerator(ISettings settings, IFilenameTokenParser filenameTokenParser, IReplayParser replayParser)
        {
            this.settings = settings;
            this.filenameTokenParser = filenameTokenParser;
            this.replayParser = replayParser;
        }

        public string GenerateFilename(FileInfo replayFile, ISettings settingsOverride = null)
        {
            settingsOverride = settingsOverride ?? settings;

            if (!replayFile.Exists)
            {
                return null;
            }

            var replayContents = File.ReadAllText(replayFile.FullName);
            var replayXml = XDocument.Parse(replayContents);

            var dateValue = DateTime.Now;

            var playerNamesValue = string.Join(
                " vs ",
                replayXml.Descendants(PlayerIdentityNodeName)
                    .Select(node => node.Attribute(PlayerNameAttributeName).Value)
                    .OrderBy(playerName => string.Compare(settingsOverride.PlayerUsername, playerName, true))
            );

            float.TryParse(replayXml.Descendants(DurationNodeName).Select(node => node.Value).FirstOrDefault() ?? string.Empty, out float durationSeconds);
            var versionValue = replayParser.GetVersion(replayFile.FullName);

            var filenameTokens = filenameTokenParser.ParseTokens(settingsOverride.FileNamingPattern);
            var filenameBuilder = new StringBuilder();

            foreach (var token in filenameTokens)
            {
                switch (token.Item1)
                {
                    case FilenameToken.StringLiteral:
                        filenameBuilder.Append(token.Item2);
                        continue;

                    case FilenameToken.DateTime:
                        filenameBuilder.Append(dateValue.ToString(token.Item2));
                        continue;

                    case FilenameToken.PlayerNames:
                        filenameBuilder.Append(playerNamesValue);
                        continue;

                    case FilenameToken.Duration:
                        filenameBuilder.Append(SecondsToHumanReadableTime(durationSeconds));
                        continue;

                    case FilenameToken.Version:
                        filenameBuilder.Append(versionValue);
                        continue;

                    default:
                        throw new ArgumentOutOfRangeException($"Unknown filename token: {token.Item1}");
                }
            }

            var filename = filenameBuilder.ToString();

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }

            return $"{filename}.xml";
        }

        private string SecondsToHumanReadableTime(float seconds)
        {
            var minutesPart = (int)Math.Floor(seconds / 60);
            var secondsPart = (int)seconds % 60;

            return $"{minutesPart}m{secondsPart}s";
        }
    }
}