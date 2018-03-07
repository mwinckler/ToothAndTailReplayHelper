using Squirrel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ToothAndTailReplayHelper.Helper
{
    internal sealed class Updater
    {
        private const string UpdateUri = "https://github.com/mwinckler/ToothAndTailReplayHelper";

        internal async Task UpdateAsync()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var updateManager = await UpdateManager.GitHubUpdateManager(UpdateUri).ConfigureAwait(false))
                {
                    await updateManager.UpdateApp().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                // Suppress.
            }
        }
    }
}
