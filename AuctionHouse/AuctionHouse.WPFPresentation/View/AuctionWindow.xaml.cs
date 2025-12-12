using AuctionHouse.Domain;
using AuctionHouse.Domain.DTO;
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

namespace AuctionHouse.WPFPresentation.View
{
    /// <summary>
    /// Interaction logic for AuctionWindow.xaml
    /// </summary>

    public partial class AuctionWindow : Window
    {
        private readonly DomainController _domainController;

        public AuctionWindow(DomainController domainController)
        {
            InitializeComponent();
            _domainController = domainController ?? throw new ArgumentNullException(nameof(domainController));
            Loaded += AuctionWindow_Loaded;
        }

        private void AuctionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                var auctions = _domainController.GetActiveAuctions();

                AuctionsListBox.ItemsSource = new ObservableCollection<Auction>(auctions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading auctions: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
