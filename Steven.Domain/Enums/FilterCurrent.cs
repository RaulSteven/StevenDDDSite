using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum FilterCurrent
    {
        [Description("{当前用户}")]
        CurrentUserId = 0,
        [Description("{当前角色}")]
        CurrentRoleId = 1,
        [Description("{当前部门}")]
        CurrentDeptId = 2
    }
}
