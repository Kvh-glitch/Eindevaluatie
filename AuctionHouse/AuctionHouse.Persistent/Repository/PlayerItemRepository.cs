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
using System.Data;

namespace AuctionHouse.Persistent.Repository
{
    public class PlayerItemRepository : IPlayerItemRepository
    {
        private readonly IMapper<Item, ItemModel> _itemMapper;
        public PlayerItemRepository(IMapper<Item, ItemModel> itemMapper)
        {
            _itemMapper = itemMapper;
        }

        public PlayerItemModel Add(PlayerItemModel model)
        {
            throw new NotImplementedException();
        }

        public void AddItem(int playerId, int itemId)
        {
            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();

                using (var cmd = new SqlCommand(
                    "INSERT INTO PlayerItems (PlayerId, ItemId) VALUES (@playerId, @itemId);",
                    connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@playerId", SqlDbType.Int) { Value = playerId });
                    cmd.Parameters.Add(new SqlParameter("@itemId", SqlDbType.Int) { Value = itemId });

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public PlayerItemModel Delete(PlayerItemModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int playerId, int itemId)
        {
            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                using (var cmd = new SqlCommand(
                    "DELETE TOP (1) FROM PlayerItems WHERE PlayerId = @playerId AND ItemId = @itemId;",
                    connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@playerId", SqlDbType.Int) { Value = playerId });
                    cmd.Parameters.Add(new SqlParameter("@itemId", SqlDbType.Int) { Value = itemId });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Collection<PlayerItemModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public PlayerItemModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Collection<PlayerItemModel> GetByPlayerId(int playerId)
        {
            var inventory = new Collection<PlayerItemModel>();
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
                command.Parameters.Add(new SqlParameter("@PlayerId", SqlDbType.Int) { Value = playerId });
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

                        inventory.Add(new PlayerItemModel(ownedId, pId, itemModel));
                    }
                }

            }
            return inventory;
        }

        public PlayerItemModel Update(PlayerItemModel model)
        {
            throw new NotImplementedException();
        }
    }
}
