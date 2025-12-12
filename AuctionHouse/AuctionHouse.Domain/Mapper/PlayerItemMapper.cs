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
    public class PlayerItemMapper : IMapper<PlayerItem, PlayerItemModel>
    {
        public PlayerItem MapToDto(PlayerItemModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Model cannot be null.");
            return new PlayerItem
            {
                Id = model.Id,
                PlayerId = model.PlayerId,
                ItemId = model.Item.Id,
                ItemName = model.Item.Name,
                RarityName = model.Item.RarityName,
                Quantity = 1 
            };
        }

        public Collection<PlayerItem> MapToDto(Collection<PlayerItemModel> models)
        {
            var dtos = new Collection<PlayerItem>();
            foreach (var model in models)
            {
                dtos.Add(MapToDto(model));
            }
            return dtos;
        }

        public PlayerItemModel MapToModel(PlayerItem dto)
        {
            throw new NotImplementedException();
        }

        public Collection<PlayerItemModel> MapToModel(Collection<PlayerItem> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
