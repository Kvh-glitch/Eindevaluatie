using AuctionHouse.Domain.Mapper;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Persistent.DbConnection;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using AuctionHouse.Domain.Repository;

namespace AuctionHouse.Persistent.Repository
{
    public class ItemRepository : IRepository<ItemModel>
    {
        private readonly IMapper<Item, ItemModel> _mapper;
        public ItemRepository(IMapper<Item, ItemModel> mapper)
        {
            _mapper = mapper;
        }

        public ItemModel Add(ItemModel model)
        {
            throw new NotImplementedException();
        }

        public ItemModel Delete(ItemModel model)
        {
            throw new NotImplementedException();
        }

        public Collection<ItemModel> GetAll()
        {
            var result = new Collection<ItemModel>();

            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT i.Id,
                         i.Name,
                         i.RarityId,
                         r.Name AS RarityName
                  FROM Items i
                  JOIN Rarities r ON r.Id = i.RarityId",
                    connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dto = new Item
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            RarityId = reader.GetInt32(2),
                            RarityName = reader.GetString(3)
                        };

                        ItemModel model = _mapper.MapToModel(dto);
                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public ItemModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ItemModel Update(ItemModel model)
        {
            throw new NotImplementedException();
        }
    }
}
