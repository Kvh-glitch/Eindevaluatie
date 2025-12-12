using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Model
{
    public class PlayerItemModel
    {
        public int Id { get; }
        public int PlayerId { get; }
        public ItemModel Item { get; }

        public PlayerItemModel(int id, int playerId, ItemModel item)
        {
            if (playerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(playerId));
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            Id = id;
            PlayerId = playerId;
            Item = item;
        }
    }

}
