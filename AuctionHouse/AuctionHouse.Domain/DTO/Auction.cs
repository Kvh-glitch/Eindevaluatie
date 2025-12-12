using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.DTO
{
    public class Auction
    {
        public int Id { get; set; }
        public int PlayerItemId { get; set; }
        public int SellerPlayerId { get; set; }

        public string SellerName { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string RarityName { get; set; } = string.Empty;
        public int Price { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int? BuyerPlayerId { get; set; }
        public DateTime? SoldTime { get; set; }

        public override string ToString()
        {
            return $"{ItemName} [{RarityName}] - Price: {Price} Gold - Ends: {EndTime}, Sold by {SellerName}";
        }
    }
}
