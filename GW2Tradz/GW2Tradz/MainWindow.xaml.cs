using GW2Tradz.Analyzers;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            if (Settings.TotalGold == -1)
            {
                Settings.TotalGold = cache.WalletGold + cache.CurrentBuys.Values.Sum() + cache.DeliveryBox.Coins;
            }

            var flipping = new FlippingAnalyzer();
            var dyes = new DyeSalvagingAnalyzer();
            var crafting = new CraftingAnalyzer();
            var ecto = new EctoAnalyzer();
            var elonian = new ElonianAnalyzer();
            var fractal = new FractalEncryptionAnalyzer();
            var unid = new UnidentifiedGearSalvager();
            FlippingGrid.DataContext = flipping.Analyse(cache);
            DyeGrid.DataContext = dyes.Analyse(cache);
            CraftingGrid.DataContext = crafting.Analyse(cache);
            EctoGrid.DataContext = ecto.Analyse(cache);
            ElonianGrid.DataContext = elonian.Analyse(cache);
            FractalGrid.DataContext = fractal.Analyse(cache);
            UnidGrid.DataContext = unid.Analyse(cache);
            CombinedGrid.DataContext = new CombiningAnalyzer { Analyzers = new List<IAnalyzer> { flipping, dyes, crafting, ecto, elonian, fractal, unid } }.Analyse(cache);
        }

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clipboard.SetText(e.AddedItems.OfType<Item>().FirstOrDefault()?.Name ?? "");
        }

        private void FlippingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clipboard.SetText(e.AddedItems.OfType<TradingAction>().FirstOrDefault()?.Item?.Name ?? "");
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedValue is TradingAction action)
            {
                Process.Start("https://www.gw2bltc.com/en/item/" + action.Item.Id);
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription { PropertyName = "Profit", Direction = System.ComponentModel.ListSortDirection.Descending });
                view.Refresh();
            }
        }
    }
}
