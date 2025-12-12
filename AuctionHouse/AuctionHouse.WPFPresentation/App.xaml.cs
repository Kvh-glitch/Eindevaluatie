using System.Configuration;
using System.Data;
using System.Windows;
using AuctionHouse.Domain.Mapper;
using AuctionHouse.Domain.DTO;
using AuctionHouse.Domain;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.Repository;
using AuctionHouse.Persistent.Repository;


namespace AuctionHouse.WPFPresentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IMapper<Rarity, RarityModel> rarityMapper = new RarityMapper();
            IRepository<RarityModel> rarityRepository = new RarityRepository(rarityMapper);

            IMapper<Item, ItemModel> itemMapper = new ItemMapper();
            ItemRepository itemRepository = new ItemRepository(itemMapper);

            IMapper<Player, PlayerModel> playerMapper = new PlayerMapper();
            IRepository<PlayerModel> playerRepository = new PlayerRepository(playerMapper);

            IPlayerItemRepository playerItemRepository = new PlayerItemRepository(itemMapper);
            IMapper<PlayerItem, PlayerItemModel> ownedItemMapper = new PlayerItemMapper();

            IMapper<Auction, AuctionModel> auctionMapper = new AuctionMapper();
            IAuctionRepository auctionRepository = new AuctionRepository(auctionMapper);
            DomainController domainController = new DomainController(
                rarityRepository,
                rarityMapper,
                itemRepository,
                itemMapper,
                playerRepository,
                playerMapper,
                playerItemRepository,   
                ownedItemMapper,
                auctionMapper,
                auctionRepository
            );



            AuctionApplication application = new AuctionApplication(domainController);


        }
    }

}
