using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Domain.DTO;
using AuctionHouse.Domain.Repository;
using Microsoft.Data.SqlClient;
using AuctionHouse.Persistent.DbConnection;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.Mapper;
using System.Collections.ObjectModel;


namespace AuctionHouse.Persistent.Repository
{

    public class RarityRepository : IRepository<RarityModel>
    {
        // Voeg de mapper dependency toe
        private readonly IMapper<Rarity, RarityModel> _mapper;

        // Voeg een constructor toe voor Dependency Injection
        public RarityRepository(IMapper<Rarity, RarityModel> mapper)
        {
            _mapper = mapper;
        }

        public RarityModel Add(RarityModel model)
        {
            throw new NotImplementedException();
        }

        public RarityModel Delete(RarityModel model)
        {
            throw new NotImplementedException();
        }

        

        public Collection<RarityModel> GetAll() 
        {
            var models = new Collection<RarityModel>();

            using (var connection = DataBaseConnection.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, BaseCost FROM Rarities", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        var rarityDto = new Rarity 
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            BaseCost = reader.GetInt32(2)
                        };

                       
                        RarityModel model = _mapper.MapToModel(rarityDto);

                        models.Add(model);
                    }
                }
            }
            return models;
        }

        public RarityModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public RarityModel Update(RarityModel model)
        {
            throw new NotImplementedException();
        }
    }
}
