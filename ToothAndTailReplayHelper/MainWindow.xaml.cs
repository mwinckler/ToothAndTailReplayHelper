using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToothAndTailReplayHelper.Internal;

namespace ToothAndTailReplayHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReplayListener replayListener;

        public MainWindow()
        {
            InitializeComponent();

            ShowInTaskbar = false;

            replayListener = new ReplayListener(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ToothAndTail", "replays"));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            replayListener?.Dispose();
        }
    }
}
