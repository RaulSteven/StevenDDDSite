using System.Collections.Generic;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface ISysMenuRepository:IRepository<SysMenu>
    {
        List<JsTreeJsonModel> GetJsonList();

        string Delete(long id);
        int GetIndexOfParent(SysMenu menu);

    }
}
