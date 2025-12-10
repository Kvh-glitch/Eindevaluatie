using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.DTO
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RarityId { get; set; }

        public string RarityName { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name} [{RarityName}]";
        }

    }
}
