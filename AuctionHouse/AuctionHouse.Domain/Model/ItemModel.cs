using AuctionHouse.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AuctionHouse.Domain.Model
{
    public class ItemModel
    {
        public int Id { get; }
        public string Name { get; }
        public int RarityId { get; }
        public string RarityName { get; }

        public ItemModel(int id, string name, int rarityId, string rarityName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (rarityId <= 0)
                throw new ArgumentOutOfRangeException(nameof(rarityId), "RarityId must be positive.");

            if (string.IsNullOrWhiteSpace(rarityName))
                throw new ArgumentException("RarityName cannot be empty.", nameof(rarityName));

            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be negative.");

            Id = id;
            Name = name.Trim();
            RarityId = rarityId;
            RarityName = rarityName.Trim();
        }
    }
}
