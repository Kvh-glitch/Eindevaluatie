using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Model
{
    public class AuctionModel
    {
        public int Id { get; set; }

        public int PlayerItemId { get; set; }      
        public int SellerPlayerId { get; set; }

        public string SellerName { get; set; }

        public int ItemId { get; set; }            
        public string ItemName { get; set; }
        public string RarityName { get; set; }

        public int Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int? BuyerPlayerId { get; set; }
        public DateTime? SoldTime { get; set; }

        public AuctionModel(
            int id,
            int playerItemId,
            int sellerPlayerId,
            string sellerName,
            int itemId,
            string itemName,
            string rarityName,
            int price,
            DateTime startTime,
            DateTime endTime,
            int? buyerPlayerId,
            DateTime? soldTime)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (playerItemId <= 0) throw new ArgumentOutOfRangeException(nameof(playerItemId));
            if (sellerPlayerId <= 0) throw new ArgumentOutOfRangeException(nameof(sellerPlayerId));
            if (itemId <= 0) throw new ArgumentOutOfRangeException(nameof(itemId));
            if (string.IsNullOrWhiteSpace(itemName)) throw new ArgumentException("ItemName required", nameof(itemName));
            if (string.IsNullOrWhiteSpace(rarityName)) throw new ArgumentException("RarityName required", nameof(rarityName));
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
            if (endTime <= startTime) throw new ArgumentException("EndTime must be after StartTime.");

            Id = id;
            PlayerItemId = playerItemId;
            SellerPlayerId = sellerPlayerId;
            SellerName = sellerName;
            ItemId = itemId;
            ItemName = itemName.Trim();
            RarityName = rarityName.Trim();
            Price = price;
            StartTime = startTime;
            EndTime = endTime;
            BuyerPlayerId = buyerPlayerId;
            SoldTime = soldTime;
        }

        public bool IsExpired(DateTime now) => now >= EndTime;
        public bool IsSold => BuyerPlayerId.HasValue;
    }
}
