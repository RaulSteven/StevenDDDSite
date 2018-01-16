using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Models
{
    public partial class Shop
    {
        [Write(false)]
        public Users User { get; set; }

        [Write(false)]
        public string AgentName { get; set; }

    }
}
