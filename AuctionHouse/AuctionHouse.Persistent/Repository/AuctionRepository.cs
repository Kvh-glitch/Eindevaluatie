using AuctionHouse.Domain.DTO;
using AuctionHouse.Domain.Mapper;
using AuctionHouse.Domain.Model;
using AuctionHouse.Domain.Repository;
using AuctionHouse.Persistent.DbConnection;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Persistent.Repository
{
    public class AuctionRepository : IAuctionRepository

    {
        private readonly IMapper<Auction, AuctionModel> _mapper;
        public AuctionRepository(IMapper<Auction, AuctionModel> mapper)
        {
            _mapper = mapper;
        }
        public int Create(int sellerPlayerId, int playerItemId, int price, DateTime start, DateTime end)
        {
            using var connection = DataBaseConnection.CreateConnection();
            connection.Open();

            const string sql = @"INSERT INTO Auctions (SellerPlayerId, PlayerItemId, Price, StartTime, EndTime, BuyerPlayerId, SoldTime)
                                 OUTPUT INSERTED.Id VALUES (@sellerPlayerId, @playerItemId, @price, @startTime, @endTime, NULL, NULL);";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@sellerPlayerId", SqlDbType.Int) { Value = sellerPlayerId });
            command.Parameters.Add(new SqlParameter("@playerItemId", SqlDbType.Int) { Value = playerItemId });
            command.Parameters.Add(new SqlParameter("@price", SqlDbType.Int) { Value = price });
            command.Parameters.Add(new SqlParameter("@startTime", SqlDbType.DateTime2) { Value = start });
            command.Parameters.Add(new SqlParameter("@endTime", SqlDbType.DateTime2) { Value = end });

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool ExistsActiveAuctionForPlayerItem(int playerItemId)
        {
           using var connection = DataBaseConnection.CreateConnection();
            connection.Open();
            const string sql = @"SELECT COUNT(*) 
                                FROM Auctions 
                                WHERE PlayerItemId = @playerItemId 
                                  AND BuyerPlayerId IS NULL 
                                  AND EndTime > SYSDATETIME();";
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@playerItemId", SqlDbType.Int) { Value = playerItemId });
            var count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public Collection<AuctionModel> GetActiveAuctions()
        {
            var result = new Collection<AuctionModel>();

            using var connection = DataBaseConnection.CreateConnection();
            connection.Open();

            const string sql = @"SELECT a.Id,
                                        a.PlayerItemId,
                                        a.SellerPlayerId,
                                        p.Name AS SellerName,
                                        pi.ItemId,
                                        i.Name AS ItemName,
                                        r.Name AS RarityName, 
                                        a.Price, 
                                        a.StartTime, 
                                        a.EndTime, 
                                        a.BuyerPlayerId, 
                                        a.SoldTime
                                FROM Auctions a
                                JOIN Players p ON p.Id = a.SellerPlayerId
                                JOIN PlayerItems pi ON pi.Id = a.PlayerItemId
                                JOIN Items i ON i.Id = pi.ItemId
                                JOIN Rarities r ON r.Id = i.RarityId
                                WHERE a.BuyerPlayerId IS NULL AND a.EndTime > SYSDATETIME()
                                ORDER BY a.EndTime ASC;";

            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var record = ReadAuction(reader);
                result.Add(_mapper.MapToModel(record));
            }

            return result;
        
        }

        public Collection<AuctionModel> GetAuctionsBySellerId(int sellerId)
        {
            var result = new Collection<AuctionModel>();
            using var connection = DataBaseConnection.CreateConnection();
            connection.Open();
            const string sql = @"SELECT a.Id,
                                        a.PlayerItemId,
                                        a.SellerPlayerId,
                                        p.Name AS SellerName,
                                        pi.ItemId,
                                        i.Name AS ItemName,
                                        r.Name AS RarityName, 
                                        a.Price, 
                                        a.StartTime, 
                                        a.EndTime, 
                                        a.BuyerPlayerId, 
                                        a.SoldTime
                                FROM Auctions a
                                JOIN PlayerItems pi ON pi.Id = a.PlayerItemId
                                JOIN Items i ON i.Id = pi.ItemId
                                JOIN Rarities r ON r.Id = i.RarityId
                                WHERE a.SellerPlayerId = @sellerId
                                ORDER BY a.EndTime ASC;";
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@sellerId", SqlDbType.Int) { Value = sellerId });
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var record = ReadAuction(reader);
                result.Add(_mapper.MapToModel(record));
            }
            return result;
        }

        public int? GetLowestActivePriceForItem(int itemId)
        {
            using var connection = DataBaseConnection.CreateConnection();
            connection.Open();
            const string sql = @"SELECT MIN(a.Price)
                                FROM Auctions a
                                JOIN PlayerItems pi ON pi.Id = a.PlayerItemId
                                WHERE pi.ItemId = @itemId
                                  AND a.BuyerPlayerId IS NULL
                                  AND a.EndTime > SYSDATETIME();";
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@itemId", SqlDbType.Int) { Value = itemId });
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(result);
        }

        private static Auction ReadAuction(SqlDataReader reader)
        {
            return new Auction
            {
                Id = reader.GetInt32(0),
                PlayerItemId = reader.GetInt32(1),
                SellerPlayerId = reader.GetInt32(2),
                SellerName = reader.GetString(3), // Fix: Change SellerName's type in Auction class to string

                ItemId = reader.GetInt32(4),
                ItemName = reader.GetString(5),
                RarityName = reader.GetString(6),

                Price = reader.GetInt32(7),
                StartTime = reader.GetDateTime(8),
                EndTime = reader.GetDateTime(9),

                BuyerPlayerId = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                SoldTime = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
            };
        }
    }
    
}
