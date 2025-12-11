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
        private readonly IPlayerInventoryRepository _playerItemRepository;
        private readonly IMapper<OwnedItem, OwnedItemModel> _ownedItemMapper;


        public DomainController(
            IRepository<RarityModel> rarityRepository,
            IMapper<Rarity, RarityModel> mapper,
            IRepository<ItemModel> itemRepository,
            IMapper<Item, ItemModel> itemMapper,
            IRepository<PlayerModel> playerRepository,
            IMapper<Player, PlayerModel> playerMapper,
            IRepository<OwnedItemModel> playerItemRepository,
            IMapper<OwnedItem, OwnedItemModel> ownedItemMapper)
        {
            _rarityRepository = rarityRepository ?? throw new ArgumentNullException(nameof(rarityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
            _playerRepository = playerRepository;
            _playerMapper = playerMapper;
            _playerItemRepository = (IPlayerInventoryRepository)playerItemRepository;
            _ownedItemMapper = ownedItemMapper;
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

        public Collection<OwnedItem> GetInventoryForPlayer(int playerId)
        {
            Collection<OwnedItemModel> models = _playerItemRepository.GetByPlayerId(playerId);
            var dtos = _ownedItemMapper.MapToDto(models);
            var stacked = dtos
                .GroupBy(x => x.ItemId)
                .Select(g =>
                {
                    var first = g.First();
                    first.Quantity = g.Count();
                    return first;
                });
            
            return new Collection<OwnedItem>(stacked.ToList());
        }


    }
}
