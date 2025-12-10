using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using AuctionHouse.Domain;
using AuctionHouse.Domain.DTO;

namespace AuctionHouse.WPFPresentation.View
{
    /// <summary>
    /// Interaction logic for ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        private readonly DomainController _domainController;
        private ObservableCollection<Item> _items = new();

        public ItemWindow(DomainController domainController)
        {
            InitializeComponent();

            _domainController = domainController
                ?? throw new ArgumentNullException(nameof(domainController));

            Loaded += ItemsWindow_Loaded;
        }

        private void ItemsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var items = _domainController.GetAllItems(); // Collection<Item>
                _items = new ObservableCollection<Item>(items);
                ItemListBox.ItemsSource = _items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
