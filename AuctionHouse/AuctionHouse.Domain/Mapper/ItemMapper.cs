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
    public class ItemMapper : IMapper<Item, ItemModel>
    {
        public Item MapToDto(ItemModel model)
        {
            if (model == null) return null;
            return new Item
            {
                Id = model.Id,
                Name = model.Name,
                RarityId = model.RarityId,
                RarityName = model.RarityName
            };
        }

        public Collection<Item> MapToDto(Collection<ItemModel> models)
        {
            var dtos = new Collection<Item>();
            foreach (var model in models)
            {
                dtos.Add(MapToDto(model));
            }
            return dtos;
        }

        public ItemModel MapToModel(Item dto)
        {
            if (dto == null) return null;
            return new ItemModel(dto.Id, dto.Name, dto.RarityId, dto.RarityName);

        }

        public Collection<ItemModel> MapToModel(Collection<Item> dtos)
        {
            var models = new Collection<ItemModel>();
            foreach (var dto in dtos)
            {
                models.Add(MapToModel(dto));
            }
            return models;
        }
    }
}
