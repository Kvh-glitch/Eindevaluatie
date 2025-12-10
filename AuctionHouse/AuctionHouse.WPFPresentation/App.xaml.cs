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
            IRepository<ItemModel> itemRepository = new ItemRepository(itemMapper);
          
            DomainController domainController = new DomainController(rarityRepository, rarityMapper, itemRepository, itemMapper);

            AuctionApplication application = new AuctionApplication(domainController);


        }
    }

}
