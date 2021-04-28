using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly SimpleCrawler crawler;
        private readonly object dataLock = new();

        public MainWindow()
        {
            InitializeComponent();

            var data = new ObservableCollection<Data>();
            BindingOperations.EnableCollectionSynchronization(data, dataLock);
            ResultListView.ItemsSource = data;

            crawler = new SimpleCrawler(data, dataLock);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var url = TbEntryUrl.Text;
            new Thread(() =>
            {
                BtnStart.Dispatcher.Invoke(() => BtnStart.IsEnabled = false);
                crawler.Crawl(url);
                BtnStart.Dispatcher.Invoke(() => BtnStart.IsEnabled = true);
            }).Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}