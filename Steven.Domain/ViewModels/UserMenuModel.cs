using Steven.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steven.Domain.ViewModels
{
    [Serializable]
    public class UserMenuModel
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

        public SysButton Buttons { get; set; }

        //public string FilterGroups { get; set; }

        public FilterGroup FilterGroup
        {
            get; set;
        }

        public bool HasFilter()
        {
            return null != FilterGroup
                && ((FilterGroup.ListRule != null && FilterGroup.ListRule.Count > 0)
                   || (FilterGroup.ListGroup != null && FilterGroup.ListGroup.Count > 0));
        }

        public List<UserMenuModel> Children { get; set; }

        public bool HasChildren
        {
            get
            {
                return Children != null && Children.Any();
            }
        }
    }
}
