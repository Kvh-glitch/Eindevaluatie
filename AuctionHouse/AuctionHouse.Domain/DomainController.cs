using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Domain.Repository;
using AuctionHouse.Domain.DTO;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.Mapper;
using System.Collections.ObjectModel;

namespace AuctionHouse.Domain
{
    public class DomainController 
    {
        private readonly IRepository<RarityModel> _rarityRepository;
        private readonly IRepository<ItemModel> _itemRepository;
        private readonly IMapper<Rarity, RarityModel> _mapper;
        private readonly IMapper<Item, ItemModel> _itemMapper;
        private readonly IRepository<PlayerModel> _playerRepository;
        private readonly IMapper<Player, PlayerModel> _playerMapper;
        private readonly IPlayerItemRepository _playerItemRepository;
        private readonly IMapper<PlayerItem, PlayerItemModel> _ownedItemMapper;
        private readonly IMapper<Auction, AuctionModel> _auctionMapper;
        private readonly IAuctionRepository _auctionRepository;


        public DomainController(
            IRepository<RarityModel> rarityRepository,
            IMapper<Rarity, RarityModel> mapper,
            IRepository<ItemModel> itemRepository,
            IMapper<Item, ItemModel> itemMapper,
            IRepository<PlayerModel> playerRepository,
            IMapper<Player, PlayerModel> playerMapper,
            IRepository<PlayerItemModel> playerItemRepository,
            IMapper<PlayerItem, PlayerItemModel> ownedItemMapper,
            IMapper<Auction, AuctionModel> auctionMapper,
            IAuctionRepository auctionRepository)
        {
            _rarityRepository = rarityRepository ?? throw new ArgumentNullException(nameof(rarityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
            _playerRepository = playerRepository;
            _playerMapper = playerMapper;
            _playerItemRepository = (IPlayerItemRepository)playerItemRepository;
            _ownedItemMapper = ownedItemMapper;
            _auctionMapper = auctionMapper;
            _auctionRepository = auctionRepository;
        }


        public Collection<Rarity> GetAllRarities()
        {
            
            Collection<RarityModel> rarityModels = _rarityRepository.GetAll();
            return _mapper.MapToDto(rarityModels);
        }

        public Collection<Item> GetAllItems()
        {
            Collection<ItemModel> models = _itemRepository.GetAll();
            return _itemMapper.MapToDto(models);
        }

        public Collection<Player> GetAllPlayers()
        {
            Collection<PlayerModel> models = _playerRepository.GetAll();
            return _playerMapper.MapToDto(models);
        }

        public Collection<PlayerItem> GetInventoryForPlayer(int playerId)
        {
            Collection<PlayerItemModel> models = _playerItemRepository.GetByPlayerId(playerId);
            var dtos = _ownedItemMapper.MapToDto(models);
            var stacked = dtos
                .GroupBy(x => x.ItemId)
                .Select(g =>
                {
                    var first = g.First();
                    first.Quantity = g.Count();
                    return first;
                });
            
            return new Collection<PlayerItem>(stacked.ToList());
        }

        public void GiveRandomItemToPlayer(int playerId)
        {   
            Collection<ItemModel> items = _itemRepository.GetAll();
            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items available to give.");
            }
            Random random = new Random();
            int index = random.Next(items.Count);
            int itemId = items[index].Id;

            _playerItemRepository.AddItem(playerId, itemId);
        }

        public void RemoveItemFromPlayer(int playerId, int itemId)
        {
            _playerItemRepository.DeleteItem(playerId, itemId);
        }

        public Collection<Auction> GetActiveAuctions()
        {
            var models = _auctionRepository.GetActiveAuctions();
            return _auctionMapper.MapToDto(models);
        }

        public bool AuctionExistsForPlayerItem(int playerItemId)
        {
            return _auctionRepository.ExistsActiveAuctionForPlayerItem(playerItemId);
        }
    }
}
