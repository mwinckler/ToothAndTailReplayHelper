﻿using Autofac;
using System;
using System.IO;
using System.Windows;
using ToothAndTailReplayHelper.Helper;
using ToothAndTailReplayHelper.Model;
using ToothAndTailReplayHelper.View;

namespace ToothAndTailReplayHelper
{
    public partial class App : Application
    {
        private const string LogFilename = "log.txt";

        private IContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var builder = new ContainerBuilder();
            builder.RegisterModule(new Model.Module());
            container = builder.Build();

            var trayNotifier = container.Resolve<ITrayNotifier>();

            container.Resolve<IReplaySaver>().ReplaySaved += (_, filename) =>
            {
                trayNotifier.Notify(ToothAndTailReplayHelper.Properties.Resources.ReplaySaved, filename);
                container.Resolve<IReplayArchiver>().ArchiveOldReplays();
            };

            trayNotifier.Notify(ToothAndTailReplayHelper.Properties.Resources.ListeningForReplays);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var message = $"[ERROR] Unhandled exception: {e.ExceptionObject}";

                using (var fs = File.OpenWrite(LogFilename))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(message);
                }

                var trayNotifier = container?.Resolve<ITrayNotifier>();
                trayNotifier?.Notify($"Error: Unhandled exception; see {LogFilename} for more details");
            }
            catch
            {
                // Suppress.
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            container?.Dispose();
        }
    }
}
