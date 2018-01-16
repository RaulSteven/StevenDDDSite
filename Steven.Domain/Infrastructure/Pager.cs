using System;
using System.Collections.Generic;

namespace Steven.Domain.Infrastructure
{
    public class Pager<T>
    {
        public int total { get; set; }
        public IEnumerable<T> rows { get; set; }
    }

    public class PageSearchModel
    {
        public string Sort { get; set; }
        public string Order { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
    public class PageSearchSortModel
    {
        public string Sort { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
