using AuctionHouse.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Repository
{
    public interface IPlayerItemRepository : IRepository<PlayerItemModel>
    {
        Collection<PlayerItemModel> GetByPlayerId(int playerId);
        void AddItem(int playerId, int itemId);
        void DeleteItem(int playerId, int itemId);
    }
}
