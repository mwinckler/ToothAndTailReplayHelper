using System.Collections.Generic;

namespace ToothAndTailReplayHelper.Model
{
    internal interface IReplayParser
    {
        string GetVersion(string replayFilePath);

        IDictionary<string, IEnumerable<string>> GetPlayerDecks(string replayFilePath);
    }
}
