using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace AuctionHouse.Domain.Model
    {
        public class RarityModel
        {
            private int _id;
            private string _name;
            private int _baseCost;
            public int Id
            {
                get => _id;
               
                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException(nameof(Id), "Id cannot be negative.");
                    _id = value;
                }
            }
            public string Name
            {
                get => _name;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Name cannot be empty.", nameof(Name));
                    _name = value.Trim();
                }
            }
            public int BaseCost
            {
                get => _baseCost;
                set
                {
                    if (value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(BaseCost), "Base cost must be greater than zero.");
                    _baseCost = value;
                }
            }
            public RarityModel(int id, string name, int baseCost)
            {
                
                Id = id;
                Name = name;
                BaseCost = baseCost;
            }
            public override string ToString() => $"{Name} (Base cost: {BaseCost})";
        }
    }

