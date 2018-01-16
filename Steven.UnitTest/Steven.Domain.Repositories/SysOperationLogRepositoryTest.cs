using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Repositories;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.UnitTest.Dependency;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steven.Domain.Infrastructure;

namespace Steven.UnitTest.Steven.Domain.Repositories
{
    [TestClass]
    public class SysOperationLogRepositoryTest:BaseTest
    {
        public ISysOperationLogRepository SysLogRepository { get; set; }
        public IUsersRepository UserRepository { get; set; }

        public SysOperationLogRepositoryTest()
        {
            var timeScrop = DependencyConfig.Container.BeginLifetimeScope();
            SysLogRepository = timeScrop.Resolve<ISysOperationLogRepository>();
            UserRepository = timeScrop.Resolve<IUsersRepository>();
        }


        [TestMethod]
        public void Insert()
        {
            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                var srcId = random.Next(100);
                var desc = "插入数据 - " + srcId;
                var id = SysLogRepository.Insert(TableSource.None, OperationType.Insert, srcId);

                Assert.AreNotEqual(id, 0);
            }
        }

        [TestMethod]
        public void Update()
        {
            Random random = new Random();
            var srcId = random.Next(100);
            var desc = "插入数据 - " + srcId;
            var id =  SysLogRepository.Insert(TableSource.None, OperationType.Insert, srcId);

            var log =   SysLogRepository.Get(id);
            var oldTime = log.UpdateTime;

            var save =   SysLogRepository.Update(log);

            log =   SysLogRepository.Get(id);
            Assert.AreNotEqual(oldTime, log.UpdateTime);
        }

        [TestMethod]
        public void Delete()
        {
            Random random = new Random();
            var srcId = random.Next(100);
            var desc = "插入数据 - " + srcId;
            var id =  SysLogRepository.Insert(TableSource.None, OperationType.Insert, srcId);
            var log =  SysLogRepository.Get(id);
            Assert.IsNotNull(log);

             SysLogRepository.Delete(log);
            log =  SysLogRepository.Get(id);
            Assert.IsNull(log);
        }

        [TestMethod]
        public void GetPagedList()
        {
            var search = new PageSearchModel()
            {
                Limit = 20,
                Offset = 0,
                Order = "Desc",
                Sort = "Id"
            };
            var list =  SysLogRepository.GetPager(search,"og", "211",TableSource.Users,null,"","",null,null);

            Assert.IsNotNull(list);
        }
    }
}
