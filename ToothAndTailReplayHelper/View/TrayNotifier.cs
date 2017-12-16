using Hardcodet.Wpf.TaskbarNotification;
using System;

namespace ToothAndTailReplayHelper.View
{
    internal sealed class TrayNotifier : ITrayNotifier, IDisposable
    {
        private TaskbarIcon taskbarIcon;

        public TrayNotifier(TaskbarIcon taskbarIcon)
        {
            this.taskbarIcon = taskbarIcon;
        }

        public void Dispose()
        {
            taskbarIcon?.Dispose();
        }

        public void Notify(string message)
        {
            taskbarIcon.ShowBalloonTip(
                Properties.Resources.ApplicationTitle,
                message,
                BalloonIcon.None
            );
        }
    }
}