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
            cache.Load();
            MainGrid.ItemsSource = cache.Items;
            var flipping = new FlippingAnalyzer();
            var dyes = new DyeSalvagingAnalyzer();
            var crafting = new CraftingAnalyzer();
            FlippingGrid.ItemsSource = flipping.Analyse(cache);
            DyeGrid.ItemsSource = dyes.Analyse(cache);
            CraftingGrid.ItemsSource = crafting.Analyse(cache);
            CombinedGrid.ItemsSource = new CombiningAnalyzer { Analyzers = new List<IAnalyzer> { flipping, dyes, crafting } }.Analyse(cache);

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
