using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ToothAndTailReplayHelper.Model
{
    internal class ReplayParser : IReplayParser
    {
        private const string PlayerNodeName         = "Player";
        private const string PlayerIdentityNodeName = "Identity";
        private const string PlayerNameAttribute    = "Name";
        private const string CardNodeName           = "Card";
        private const string CardDataAttribute      = "Data";
        private const string VersionNodeName        = "VersionHash";

        private readonly ISettings settings;

        public ReplayParser(ISettings settings)
        {
            this.settings = settings;
        }

        public string GetVersion(string replayFilePath)
        {
            return GetXDocument(replayFilePath).Descendants(VersionNodeName).Select(node => node.Value).FirstOrDefault() ?? string.Empty;
        }

        public IDictionary<string, IEnumerable<string>> GetPlayerDecks(string replayFilePath)
        {
            return GetXDocument(replayFilePath).Descendants(PlayerNodeName).Select(node => new
            {
                playerName = node.Descendants(PlayerIdentityNodeName).First().Attribute(PlayerNameAttribute).Value,
                cards = node.Descendants(CardNodeName).Select(card => card.Attribute(CardDataAttribute).Value)
            }).ToDictionary(
                x => x.playerName,
                x => x.cards
            );
        }

        private static XDocument GetXDocument(string replayFilePath)
        {
            // Reading contents ahead of time instead of XDocument.Load because Load seems to have
            // a bug that doesn't handle UTF8 characters in the filename.
            return XDocument.Parse(File.ReadAllText(replayFilePath));
        }
    }
}
