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
    public class OwnedItemMapper : IMapper<OwnedItem, OwnedItemModel>
    {
        public OwnedItem MapToDto(OwnedItemModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Model cannot be null.");
            return new OwnedItem
            {
                Id = model.Id,
                PlayerId = model.PlayerId,
                ItemId = model.Item.Id,
                ItemName = model.Item.Name,
                RarityName = model.Item.RarityName,
                Quantity = 1 
            };
        }

        public Collection<OwnedItem> MapToDto(Collection<OwnedItemModel> models)
        {
            var dtos = new Collection<OwnedItem>();
            foreach (var model in models)
            {
                dtos.Add(MapToDto(model));
            }
            return dtos;
        }

        public OwnedItemModel MapToModel(OwnedItem dto)
        {
            throw new NotImplementedException();
        }

        public Collection<OwnedItemModel> MapToModel(Collection<OwnedItem> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
