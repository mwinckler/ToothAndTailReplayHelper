using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToothAndTailReplayHelper.ViewModel
{
    internal class NotifyIconViewModel
    {
        public ICommand ExitApplication { get; }

        public ICommand OpenSettings { get; }

        public NotifyIconViewModel(ICommand exitApplication, ICommand openSettings)
        {
            ExitApplication = exitApplication;
            OpenSettings = openSettings;
        }
    }
}
