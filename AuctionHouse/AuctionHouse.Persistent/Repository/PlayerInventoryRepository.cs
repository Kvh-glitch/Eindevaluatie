using AuctionHouse.Domain.Mapper;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Domain.DTO;
using AuctionHouse.Persistent.DbConnection;
using Microsoft.Data.SqlClient;

namespace AuctionHouse.Persistent.Repository
{
    public class PlayerInventoryRepository : IPlayerInventoryRepository
    {
        private readonly IMapper<Item, ItemModel> _itemMapper;
        public PlayerInventoryRepository(IMapper<Item, ItemModel> itemMapper)
        {
            _itemMapper = itemMapper;
        }

        public OwnedItemModel Add(OwnedItemModel model)
        {
            throw new NotImplementedException();
        }

        public OwnedItemModel Delete(OwnedItemModel model)
        {
            throw new NotImplementedException();
        }

        public Collection<OwnedItemModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public OwnedItemModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Collection<OwnedItemModel> GetByPlayerId(int playerId)
        {
            var inventory = new Collection<OwnedItemModel>();
            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT pi.Id,
                             pi.PlayerId,
                             i.Id AS ItemId,
                             i.Name AS ItemName,
                             i.RarityId,
                             r.Name AS RarityName
                      FROM PlayerItems pi
                      JOIN Items i ON pi.ItemId = i.Id
                      JOIN Rarities r ON i.RarityId = r.Id
                      WHERE pi.PlayerId = @PlayerId",
                    connection);
                command.Parameters.AddWithValue("@PlayerId", playerId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ownedId = reader.GetInt32(0);
                        int pId = reader.GetInt32(1);
                        int itemId = reader.GetInt32(2);
                        string itemName = reader.GetString(3);
                        int rarityId = reader.GetInt32(4);
                        string rarityName = reader.GetString(5);

                        var itemDto = new Item
                        {
                            Id = itemId,
                            Name = itemName,
                            RarityId = rarityId,
                            RarityName = rarityName
                        };

                        ItemModel itemModel = _itemMapper.MapToModel(itemDto);

                        inventory.Add(new OwnedItemModel(ownedId, pId, itemModel));
                    }
                }

            }
            return inventory;
        }

        public OwnedItemModel Update(OwnedItemModel model)
        {
            throw new NotImplementedException();
        }
    }
}
