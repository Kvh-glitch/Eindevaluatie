using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AuctionHouse.Domain.Mapper
{
    public interface IMapper<D, M>
    {
        public D MapToDto(M model);
        public M MapToModel(D dto);

        public Collection<D> MapToDto(Collection<M> models);
        
        public Collection<M> MapToModel(Collection<D> dtos);


    }
}
