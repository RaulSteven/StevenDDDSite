using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IAdvertRepository : IRepository<Advert>
    {
        Pager<AdvertBizModel> GetList(long adPosId, string name, AdvertStatus? status, PageSearchModel search);
        List<Advert> GetListByAdPostIdCache(long id, AdvertType? type, int takeSize);
    }
}
