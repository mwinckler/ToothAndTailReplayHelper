using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ToothAndTailReplayHelper.Model
{
    internal class ReplayParser : IReplayParser
    {
        private const string VersionNodeName = "VersionHash";

        public string GetVersion(string replayFilePath)
        {
            // Reading contents ahead of time instead of XDocument.Load because Load seems to have
            // a bug that doesn't handle UTF8 characters in the filename.
            var replayXml = File.ReadAllText(replayFilePath);
            return XDocument.Parse(replayXml).Descendants(VersionNodeName).Select(node => node.Value).FirstOrDefault() ?? string.Empty;
        }
    }
}
