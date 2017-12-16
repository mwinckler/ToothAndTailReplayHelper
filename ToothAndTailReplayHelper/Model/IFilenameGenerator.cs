using System.IO;

namespace ToothAndTailReplayHelper.Model
{
    internal interface IFilenameGenerator
    {
        string GenerateFilename(FileInfo replayFile);
    }
}
