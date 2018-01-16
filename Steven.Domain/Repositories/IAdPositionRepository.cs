using System.Collections.Generic;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IAdPositionRepository : IRepository<AdPosition>
    {
        AdPosition GetByPosKeyCache(AdPosKey value);
    }
}
