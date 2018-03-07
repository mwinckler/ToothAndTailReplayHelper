using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToothAndTailReplayHelper.View;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class PostMatchNotifier : IPostMatchNotifier
    {
        private readonly ITrayNotifier trayNotifier;
        private readonly IReplayParser replayParser;
        private readonly ISettings settings;

        public PostMatchNotifier(ITrayNotifier trayNotifier, IReplayParser replayParser, ISettings settings)
        {
            this.trayNotifier = trayNotifier;
            this.replayParser = replayParser;
            this.settings = settings;
        }

        public void Notify(string replayFilename)
        {
            var decks = replayParser.GetPlayerDecks(Path.Combine(settings.ReplayDirectoryPath, replayFilename));
            var tooltipText = new List<string> { replayFilename };

            foreach (var kv in decks.Where(pair => string.IsNullOrEmpty(settings.PlayerUsername) || settings.PlayerUsername != pair.Key).OrderBy(pair => pair.Key))
            {
                tooltipText.Add($"{kv.Key}'s deck: {string.Join(", ", kv.Value.Select(SanitizeCardName))}");
            }

            trayNotifier.Notify(Properties.Resources.ReplaySaved, string.Join(System.Environment.NewLine, tooltipText));
        }

        private string SanitizeCardName(string card)
        {
            return card.Replace("warren_", "").Replace("structure_", "");
        }
    }
}
