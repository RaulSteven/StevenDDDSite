using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.UnitTest.Dependency
{
    public class BaseTest
    {
        public BaseTest()
        {
            DependencyConfig.Register();
        }
    }
}
