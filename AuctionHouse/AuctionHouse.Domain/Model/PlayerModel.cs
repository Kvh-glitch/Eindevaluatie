using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Model
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gold { get; set; }
        public PlayerModel(int id, string name, int gold)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (gold < 0)
                throw new ArgumentOutOfRangeException(nameof(gold), "Gold cannot be negative.");

            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be negative.");

            Id = id;
            Name = name.Trim();
            Gold = gold;
        }
        // moet naar domeinlaag verhuizen
        //public void AddGold(int amount)
        //{
        //    if (amount < 0)
        //        throw new ArgumentOutOfRangeException(nameof(amount), "Amount to add cannot be negative.");
        //    Gold += amount;
        //}

        //public void SubtractGold(int amount)
        //{
        //    if (amount < 0)
        //        throw new ArgumentOutOfRangeException(nameof(amount), "Amount to subtract cannot be negative.");
        //    if (amount > Gold)
        //        throw new InvalidOperationException("Insufficient gold to subtract the specified amount.");
        //    Gold -= amount;
        //}

    }
}
