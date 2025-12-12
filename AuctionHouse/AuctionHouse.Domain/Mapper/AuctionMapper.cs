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
    public class AuctionMapper : IMapper<Auction, AuctionModel>
    {
        public Auction MapToDto(AuctionModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Model cannot be null.");
            return new Auction
            {
                Id = model.Id,
                PlayerItemId = model.PlayerItemId,
                ItemId = model.ItemId,
                ItemName = model.ItemName,
                SellerName = model.SellerName,
                RarityName = model.RarityName,
                Price = model.Price,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                SellerPlayerId = model.SellerPlayerId,
                BuyerPlayerId = model.BuyerPlayerId,
                SoldTime = model.SoldTime,
            };
        }

        public Collection<Auction> MapToDto(Collection<AuctionModel> models)
        {
           var dtos = new Collection<Auction>();
            foreach (var model in models)
            {
                dtos.Add(MapToDto(model));
            }
            return dtos;
        }

        public AuctionModel MapToModel(Auction dto)
        {
            if (dto == null) return null;

            return new AuctionModel(
                dto.Id,
                dto.PlayerItemId,
                dto.SellerPlayerId,
                dto.SellerName,
                dto.ItemId,
                dto.ItemName,
                dto.RarityName,
                dto.Price,
                dto.StartTime,
                dto.EndTime,
                dto.BuyerPlayerId,
                dto.SoldTime
            );
        }

        public Collection<AuctionModel> MapToModel(Collection<Auction> dtos)
        {
            var models = new Collection<AuctionModel>();
            foreach (var dto in dtos)
            {
                models.Add(MapToModel(dto));
            }
            return models;
        }
    }
}
