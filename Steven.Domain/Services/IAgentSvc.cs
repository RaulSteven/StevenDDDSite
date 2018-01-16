using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
namespace Steven.Domain.Services
{
    public interface IAgentSvc
    {
        JsonModel Save(AgentModel model);
    }
}
