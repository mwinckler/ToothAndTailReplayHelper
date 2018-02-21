using System;

namespace ToothAndTailReplayHelper.Helper
{
    internal interface IReplaySaver : IDisposable
    {
        event EventHandler<string> ReplaySaved;
    }
}
