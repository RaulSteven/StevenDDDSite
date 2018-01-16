using System.Collections.Generic;
using Steven.Domain.Enums;
using System;

namespace Steven.Domain.ViewModels
{
    [Serializable]
    public class FilterGroup
    {
        public List<FilterRule> ListRule { get; set; }
        public FilterGroupOp Op { get; set; }
        public List<FilterGroup> ListGroup { get; set; }

    }

    [Serializable]
    public class FilterRule
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public FilterRuleOp Op { get; set; }

    }
}
