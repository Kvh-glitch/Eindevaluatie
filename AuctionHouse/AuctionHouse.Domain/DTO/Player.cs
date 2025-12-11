using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.DTO
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gold { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Gold} gold)";
        }
    }
}
