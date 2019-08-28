﻿using GW2Tradz.Analyzers;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public string HelpText { get; } = "Double click to open bltc for item\n" +
            "\n" +
            "Filter/search bar in top\n" +
            "\n" +
            "Columns can sort\n";




        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                NotifyPropertyChanged("FilterString");
                FilterCollection();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void FilterCollection()
        {
            foreach(var view in _collectionViews)
            {
                view.Refresh();
            }
        }

        private string _filterString;
        private List<ICollectionView> _collectionViews = new List<ICollectionView> { };

        public bool Filter(object obj)
        {
            var data = obj as TradingAction;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterString))
                {
                    return data.Item.Name.IndexOf(_filterString, StringComparison.InvariantCultureIgnoreCase) >= 0 || data.Description.IndexOf(_filterString, StringComparison.InvariantCultureIgnoreCase) >= 0;
                }
                return true;
            }
            return false;
        }

        public MainWindow()
        {
            InitializeComponent();

            if(File.Exists("settings.json"))
            {
                JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
            }

            var cache = new Cache();
            cache.Load();

            if (Settings.TotalCoins == -1)
            {
                Settings.TotalCoins = cache.WalletGold + cache.CurrentBuys.Values.Sum() + cache.DeliveryBox.Coins;
            }

            var flipping = new FlippingAnalyzer().Cache();
            var dyes = new DyeSalvagingAnalyzer().Cache();
            var crafting = new CraftingAnalyzer().Cache();
            var ecto = new EctoAnalyzer().Cache();
            var elonian = new ElonianAnalyzer().Cache();
            var fractal = new FractalEncryptionAnalyzer().Cache();
            var unid = new UnidentifiedGearSalvager().Cache();
            var clicking = new ClickingAnalyser().Cache();
            var gemstones = new GemstoneAnalyzer().Cache();
            var materials = new MaterialFluctuationAnalyzer().Cache();
            var gearSalvaging = new GearSalvagingAnalyzer().Cache();
            var hideSalvaging = new HideSalvagingAnalyzer().Cache();
            FlippingGrid.DataContext = flipping.Analyse(cache);
            DyeGrid.DataContext = dyes.Analyse(cache);
            CraftingGrid.DataContext = crafting.Analyse(cache);
            EctoGrid.DataContext = ecto.Analyse(cache);
            ElonianGrid.DataContext = elonian.Analyse(cache);
            FractalGrid.DataContext = fractal.Analyse(cache);
            UnidGrid.DataContext = unid.Analyse(cache);
            ClickingGrid.DataContext = clicking.Analyse(cache);
            GemstoneGrid.DataContext = gemstones.Analyse(cache);
            MaterialsGrid.DataContext = materials.Analyse(cache);
            GearSalvagingGrid.DataContext = gearSalvaging.Analyse(cache);
            HideSalvagingGrid.DataContext = hideSalvaging.Analyse(cache);
            CombinedGrid.DataContext = new CombiningAnalyzer { Analyzers = new List<IAnalyzer> { flipping, dyes, crafting, ecto, elonian, fractal, unid, clicking, gemstones, materials, gearSalvaging, hideSalvaging } }.Analyse(cache);

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
                view.Filter = Filter;
                _collectionViews.Add(view);
                view.Refresh();
            }
        }
    }
}
