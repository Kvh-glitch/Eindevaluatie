using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.DTO
{
    public class PlayerItem
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;
        public string RarityName { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public override string ToString()
        {
            return Quantity > 1
                ? $"{ItemName} x{Quantity} [{RarityName}]"
                : $"{ItemName} [{RarityName}]";
        }

    }
}
