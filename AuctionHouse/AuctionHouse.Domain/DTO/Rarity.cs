using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.DTO
{
    public class Rarity
    {
        public int Id { get; set; }          
        public string Name { get; set; } = string.Empty;
        public int BaseCost { get; set; }

        public override string ToString() => $"{Name} (Base cost: {BaseCost})";
    }
}
