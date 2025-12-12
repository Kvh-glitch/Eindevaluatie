using AuctionHouse.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Repository
{
    public interface IAuctionRepository
    {
        Collection<AuctionModel> GetActiveAuctions();

        Collection<AuctionModel> GetAuctionsBySellerId(int sellerId);

        int? GetLowestActivePriceForItem(int itemId);

        bool ExistsActiveAuctionForPlayerItem(int playerItemId);

        int Create(int sellerPlayerId, int playerItemId, int price, DateTime start, DateTime end);

        

    }
}
