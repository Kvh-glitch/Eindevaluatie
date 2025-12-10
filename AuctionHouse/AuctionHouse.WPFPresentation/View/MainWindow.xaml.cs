using AuctionHouse.Domain.DTO;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AuctionHouse.Domain;
using System;

namespace AuctionHouse.WPFPresentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DomainController _domainController;

        private ObservableCollection<Rarity> _rarities = new();

        public MainWindow(DomainController domainController)
        {
            InitializeComponent();

            _domainController = domainController
                ?? throw new ArgumentNullException(nameof(domainController));

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var rarities = _domainController.GetAllRarities();
                _rarities = new ObservableCollection<Rarity>(rarities);
                RarityListBox.ItemsSource = _rarities;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rarities: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}