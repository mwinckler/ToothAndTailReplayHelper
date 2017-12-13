using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;

namespace ToothAndTailReplayHelper.Internal
{
    internal class ReplayListener : IDisposable
    {
        private const string PlayerIdentityNodeName  = "Identity";
        private const string PlayerNameAttributeName = "Name";

        FileSystemWatcher fileSystemWatcher;
        HashSet<string> handledFileHashes = new HashSet<string>();
        SemaphoreSlim fileModificationSemaphore = new SemaphoreSlim(1, 1);

        public ReplayListener(string directoryPath, string fileFilter = "LastReplay.xml")
        {
            fileSystemWatcher = new FileSystemWatcher(directoryPath, fileFilter);
            fileSystemWatcher.Changed += FileModified;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }

        private async void FileModified(object sender, FileSystemEventArgs e)
        {
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
                    // Errors can occur if TnT is still writing the replay file. Wait until it is finished.
                    return;
                }

                var newFilename = GetReplayFilename(new FileInfo(e.FullPath));

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
        }

        private string GetReplayFilename(FileInfo replayFile)
        {
            if (!replayFile.Exists)
            {
                return null;
            }

            var replayXml = XDocument.Load(replayFile.FullName);

            var datePart = DateTime.Now.ToString("yyyyMMddTHHmm");
            var namePart = string.Join(" vs ", replayXml.Descendants(PlayerIdentityNodeName).Select(node => node.Attribute(PlayerNameAttributeName).Value));

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                namePart = namePart.Replace(c, '_');
            }

            return $"{datePart} {namePart}.xml";
        }
    }
}
