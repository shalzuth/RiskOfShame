using System;
using System.Threading;
using System.Windows;
using RiskOfShame.Loader.Core;

namespace RiskOfShame.Loader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddAssembly(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteAssembly(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RebuildAssembly(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Config Config => Config.Instance;
        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Config.Save();
        }
    }
}
