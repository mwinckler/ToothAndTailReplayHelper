using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Controls;
using ToothAndTailReplayHelper.Command;
using ToothAndTailReplayHelper.Internal;
using ToothAndTailReplayHelper.View;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ReplaySaver>().AsImplementedInterfaces().SingleInstance().AutoActivate();

            builder.RegisterType<ExitApplicationCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OpenSettingsCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OpenReplayFolderCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OpenDateFormatHelpCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SettingsWindow>().SingleInstance();

            builder.RegisterType<Settings>().AsImplementedInterfaces().SingleInstance().OnActivating(args =>
            {
                args.Instance.Initialize();
            });

            builder.RegisterType<ViewModel.SettingsWindow>().AsImplementedInterfaces().SingleInstance().OnActivated(args =>
            {
                var window = args.Context.Resolve<SettingsWindow>();

                args.Instance.ShowRequested += (_, __) => window.Show();
                args.Instance.HideRequested += (_, __) => window.Hide();
            });

            builder.RegisterType<ContextMenu>().SingleInstance().OnActivated(args =>
            {
                args.Instance.Items.Add(new MenuItem
                {
                    Header = Properties.Resources.Settings,
                    Command = args.Context.Resolve<IOpenSettingsCommand>(),
                });

                args.Instance.Items.Add(new MenuItem
                {
                    Header = Properties.Resources.Quit,
                    Command = args.Context.Resolve<IExitApplicationCommand>()
                });
            });

            builder.RegisterType<FilenameGenerator>().AsImplementedInterfaces();
            builder.RegisterType<FilenameTokenParser>().AsImplementedInterfaces();
            builder.RegisterType<TrayNotifier>().AsImplementedInterfaces();

            builder.RegisterType<TaskbarIcon>().OnActivated(args =>
            {
                args.Instance.Icon = Properties.Resources.TrayIcon;
                args.Instance.ToolTip = Properties.Resources.ApplicationTitle;
                args.Instance.ContextMenu = args.Context.Resolve<ContextMenu>();
            });
        }
    }
}
