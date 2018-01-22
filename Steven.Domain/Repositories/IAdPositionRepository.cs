using Steven.Domain.Enums;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IAdPositionRepository : IRepository<AdPosition>
    {
        AdPosition GetByPosKeyCache(AdPosKey value);
    }
}
