namespace Steven.Domain.ViewModels
{
    public class SysApartModel
    {
        public long Id { get; set; }

        public virtual long Pid
        {
            get;
            set;
        }

        public virtual int Sort
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

        public long[] RoleIds { get; set; }

    }
}