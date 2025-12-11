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
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private readonly DomainController _domainController;
        private ObservableCollection<Player> _players = new();
        private ObservableCollection<OwnedItem> _inventory = new();
        public PlayerWindow(DomainController domainController)
        {
            InitializeComponent();

            _domainController = domainController
                ?? throw new ArgumentNullException(nameof(domainController));
            Loaded += PlayerWindow_Loaded;
        }

        private void PlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var players = _domainController.GetAllPlayers(); 
                _players = new ObservableCollection<Player>(players);
                PlayerListBox.ItemsSource = _players;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading players: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlayerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayerListBox.SelectedItem is Player selectedPlayer)
            {
                GoldTextBlock.Text = $"{selectedPlayer.Gold}";

                try
                {
                    var ownedItems = _domainController.GetInventoryForPlayer(selectedPlayer.Id);
                    _inventory = new ObservableCollection<OwnedItem>(ownedItems);
                    InventoryListBox.ItemsSource = _inventory;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading inventory items: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                GoldTextBlock.Text = string.Empty;
                InventoryListBox.ItemsSource = null;
            }
        }
    }
}
