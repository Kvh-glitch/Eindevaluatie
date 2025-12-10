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
        private readonly IMapper<Rarity, RarityModel> _mapper; 

        public DomainController(
            IRepository<RarityModel> rarityRepository,
            IMapper<Rarity, RarityModel> mapper)
        {
            _rarityRepository = rarityRepository ?? throw new ArgumentNullException(nameof(rarityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        
        public Collection<Rarity> GetAllRarities()
        {
            
            Collection<RarityModel> rarityModels = _rarityRepository.GetAll();
            return _mapper.MapToDto(rarityModels);
        }

        
    }
}
