using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GW2Solver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

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
            foreach (var view in _collectionViews)
            {
                view.Refresh();
            }
        }

        private class Entry
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string Description { get; set; }
        }

        private string _filterString;
        private List<ICollectionView> _collectionViews = new List<ICollectionView> { };

        public bool Filter(object obj)
        {
            if (obj is Entry data)
            {
                if (!string.IsNullOrEmpty(_filterString))
                {
                    return data.Name.IndexOf(_filterString, StringComparison.InvariantCultureIgnoreCase) >= 0 || data.Description.IndexOf(_filterString, StringComparison.InvariantCultureIgnoreCase) >= 0;
                }
                return true;
            }
            return false;
        }

        public MainWindow()
        {
            InitializeComponent();

            Magic.DataContext = JsonConvert.DeserializeObject<List<Entry>>(File.ReadAllText("../../../../LPSolver/operations.json"));
        }


        private void FlippingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clipboard.SetText(e.AddedItems.OfType<Entry>().FirstOrDefault()?.Name ?? "");
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.Item is Entry action)
            {
                Process.Start("https://www.gw2bltc.com/en/item/" + action.ID);
            }
        }


        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
                view.Filter = Filter;
                _collectionViews.Add(view);
                view.Refresh();
            }
        }
    }
}
