using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Domain.DTO;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.Mapper;
using System.Collections.ObjectModel;
using AuctionHouse.Persistent.DbConnection;
using Microsoft.Data.SqlClient;
using AuctionHouse.Domain.Repository;

namespace AuctionHouse.Persistent.Repository
{
    public class PlayerRepository : IRepository<PlayerModel>
    {
        private readonly IMapper<Player, PlayerModel> _mapper;
        public PlayerRepository(IMapper<Player, PlayerModel> mapper)
        {
            _mapper = mapper;
        }

        public PlayerModel Add(PlayerModel model)
        {
            throw new NotImplementedException();
        }

        public PlayerModel Delete(PlayerModel model)
        {
            throw new NotImplementedException();
        }

        public Collection<PlayerModel> GetAll()
        {
            var models = new Collection<PlayerModel>();
            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, Gold FROM Players", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var playerDto = new Player
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Gold = reader.GetInt32(2)
                        };
                        var playerModel = _mapper.MapToModel(playerDto);
                        models.Add(playerModel);
                    }
                }
            }
            return models;
        }

        public PlayerModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public PlayerModel Update(PlayerModel model)
        {
            throw new NotImplementedException();
        }
    }
}
