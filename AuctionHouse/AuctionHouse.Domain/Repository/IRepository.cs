using AuctionHouse.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Repository
{
    public interface IRepository<M>
    {
        public M Add(M model);

        public M Update(M model);

        public M Delete(M model);

        public Collection<M> GetAll();
        public M GetById(int id);
        
    }
}
