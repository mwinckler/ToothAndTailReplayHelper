using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using ToothAndTailReplayHelper.Model;
using ToothAndTailReplayHelper.View;

namespace ToothAndTailReplayHelper.Helper
{
    internal sealed class ReplaySaver : IReplaySaver
    {
        private const string FileFilter = "LastReplay.xml";

        public event EventHandler<string> ReplaySaved;

        IFilenameGenerator filenameGenerator;
        FileSystemWatcher fileSystemWatcher;
        HashSet<string> handledFileHashes = new HashSet<string>();
        SemaphoreSlim fileModificationSemaphore = new SemaphoreSlim(1, 1);

        public ReplaySaver(ISettings settings, IFilenameGenerator filenameGenerator, ITrayNotifier trayNotifier)
        {
            this.filenameGenerator = filenameGenerator;

            fileSystemWatcher = new FileSystemWatcher(settings.ReplayDirectoryPath, FileFilter);
            fileSystemWatcher.Changed += FileModified;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }

        private async void FileModified(object sender, FileSystemEventArgs e)
        {
            string newFilename;

            // This semaphore is to handle a known issue with FileSystemWatcher where
            // it can raise multiple events for the same create action. We only want
            // to copy the file once.
            await fileModificationSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (!File.Exists(e.FullPath))
                {
                    return;
                }

                try
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(e.FullPath))
                        {
                            var hash = string.Join("", md5.ComputeHash(stream).Select(b => b.ToString("X2")));

                            if (handledFileHashes.Contains(hash))
                            {
                                return;
                            }

                            handledFileHashes.Add(hash);
                        }
                    }
                }
                catch
                {
                    // Errors can occur if TnT is still writing the replay file.
                    // Suppress the exception because another FileSystemWatcher event
                    // should be raised once it is finished.
                    return;
                }

                newFilename = filenameGenerator.GenerateFilename(new FileInfo(e.FullPath));

                if (string.IsNullOrEmpty(newFilename) || File.Exists(newFilename))
                {
                    return;
                }

                File.Copy(e.FullPath, Path.Combine(Path.GetDirectoryName(e.FullPath), newFilename), true);
            }
            finally
            {
                fileModificationSemaphore.Release();
            }

            if (!string.IsNullOrWhiteSpace(newFilename))
            {
                ReplaySaved?.Invoke(this, newFilename);
            }
        }
    }
}
