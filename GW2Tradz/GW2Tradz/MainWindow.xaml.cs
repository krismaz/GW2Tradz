using GW2Tradz.Analyzers;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
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

namespace GW2Tradz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var cache = new Cache();
            var silver = new Silveress();
            cache.Update(silver.FetchBasicInfo());
            cache.UpdateHistory(silver.FetchHistory());
            MainGrid.ItemsSource = cache.Resolve();
            FlippingGrid.ItemsSource = new FlippingAnalyzer().Analyse(1300 * 100 * 100, cache);

        }

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clipboard.SetText(e.AddedItems.OfType<Item>().FirstOrDefault()?.Name ?? "");
        }

        private void FlippingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clipboard.SetText(e.AddedItems.OfType<TradingAction>().FirstOrDefault()?.Item?.Name ?? "");
        }
    }
}
