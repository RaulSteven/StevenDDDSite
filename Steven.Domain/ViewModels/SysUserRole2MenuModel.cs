using Steven.Domain.Enums;

namespace Steven.Domain.ViewModels
{
    public class SysUserRole2MenuModel
    {
        public long Id { get; set; }

        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public SysButton Buttons { get; set; }

        public SysButton SelectedButtons { get; set; }

        public string MenuTreePath { get; set; }
    }
}
