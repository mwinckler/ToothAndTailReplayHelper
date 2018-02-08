using Squirrel;
using System;
using System.Threading.Tasks;

namespace ToothAndTailReplayHelper.Helper
{
    internal sealed class Updater
    {
        private const string UpdateUri = "https://github.com/mwinckler/ToothAndTailReplayHelper";

        internal async Task UpdateAsync()
        {
            using (var updateManager = new UpdateManager(UpdateUri))
            {
                await updateManager.UpdateApp().ConfigureAwait(false);
            }
        }
    }
}
