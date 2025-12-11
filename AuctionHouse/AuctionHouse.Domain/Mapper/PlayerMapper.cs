using AuctionHouse.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Domain.DTO;
using System.Collections.ObjectModel;

namespace AuctionHouse.Domain.Mapper
{
    public class PlayerMapper : IMapper<Player, PlayerModel>
    {
        public Player MapToDto(PlayerModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Model cannot be null.");
            return new Player
            {
                Id = model.Id,
                Name = model.Name,
                Gold = model.Gold
            };
        }

        public Collection<Player> MapToDto(Collection<PlayerModel> models)
        {
           var dtos = new Collection<Player>();
            foreach (var model in models)
            {
                dtos.Add(MapToDto(model));
            }
            return dtos;
        }

        public PlayerModel MapToModel(Player dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "DTO cannot be null.");
            return new PlayerModel(dto.Id, dto.Name, dto.Gold);
        }

        public Collection<PlayerModel> MapToModel(Collection<Player> dtos)
        {
            var models = new Collection<PlayerModel>();
            foreach (var dto in dtos)
            {
                models.Add(MapToModel(dto));
            }
            return models;

        }
    }
}
