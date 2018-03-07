using ToothAndTailReplayHelper.Helper;
using ToothAndTailReplayHelper.View;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class App : IApp
    {
        private readonly ITrayNotifier trayNotifier;

        public App(
            ITrayNotifier trayNotifier,
            IReplaySaver replaySaver,
            IReplayArchiver replayArchiver,
            IPostMatchNotifier postMatchNotifier)
        {
            this.trayNotifier = trayNotifier;

            replaySaver.ReplaySaved += (_, filename) =>
            {
                postMatchNotifier.Notify(filename);
                replayArchiver.ArchiveOldReplays();
            };
        }

        public void Run()
        {
            trayNotifier.Notify(Properties.Resources.ListeningForReplays);
        }
    }
}
