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

        public DomainController(
            IRepository<RarityModel> rarityRepository,
            IMapper<Rarity, RarityModel> mapper,
            IRepository<ItemModel> itemRepository,
            IMapper<Item, ItemModel> itemMapper)
        {
            _rarityRepository = rarityRepository ?? throw new ArgumentNullException(nameof(rarityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
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


    }
}
