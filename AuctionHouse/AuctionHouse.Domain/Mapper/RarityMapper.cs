using AuctionHouse.Domain.DTO;
using AuctionHouse.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Mapper
{
    public class RarityMapper : IMapper<Rarity, RarityModel>
    {
        
        public Rarity MapToDto(RarityModel model)
        {
            if (model == null) return null;

            
            return new Rarity 
            {
                Id = model.Id,
                Name = model.Name,
                BaseCost = model.BaseCost
            };
        }

        public RarityModel MapToModel(Rarity dto)
        {
            if (dto == null) return null;

            
            return new RarityModel(dto.Id, dto.Name, dto.BaseCost); 
        }

        public Collection<Rarity> MapToDto(Collection<RarityModel> models)
        {
            var dtos = new Collection<Rarity>();
            foreach (var model in models)
            {
                dtos.Add(MapToDto(model));
            }
            return dtos;
        }

        public Collection<RarityModel> MapToModel(Collection<Rarity> dtos)
        {
            var models = new Collection<RarityModel>();
            foreach (var dto in dtos)
            {
                models.Add(MapToModel(dto));
            }
            return models;
        }
    }

}
