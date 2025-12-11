using AuctionHouse.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Repository
{
    public interface IPlayerInventoryRepository : IRepository<OwnedItemModel>
    {
        Collection<OwnedItemModel> GetByPlayerId(int playerId);
    }
}
