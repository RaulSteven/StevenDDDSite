using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Models
{
    public partial class Article
    {
        [Write(false)]
        public string ClassifyName { get; set; }
    }
}
