using Steven.Core.Utilities;
using Steven.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Steven.Domain.ViewModels
{
    public class SysMenuModel
    {
        public long Id { get; set; }

        public virtual long Pid
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Remark
        {
            get;
            set;
        }

        public virtual string Url
        {
            get;
            set;
        }

        public virtual string Icon
        {
            get;
            set;
        }

        public virtual int Sort
        {
            get;
            set;
        }

        public virtual string Source
        {
            get;
            set;
        }

        public SysButton[] ButtonArray { get; set; }

        
    }
}
