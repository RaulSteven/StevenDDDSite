using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.Domain.Repositories;
using AutoMapper;
using Steven.Core.Utilities;

namespace Steven.Domain.Services
{
    public class AgentSvc : BaseSvc, IAgentSvc
    {
        public ISysOperationLogRepository LogRepository { get; set; }
        public IAgentRepository AgentRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }

        public JsonModel Save(AgentModel model)
        {
            var result = new JsonModel();
            var opType = OperationType.Insert;
            Agent agent = null;
            Users user = null;
            if (model.Id > 0)
            {
                agent = AgentRepository.Get(model.Id);
                if (agent == null)
                {
                    result.msg = $"找不到id为{model.Id}的代理";
                    return result;
                }
                opType = OperationType.Update;
                user = UsersRepository.Get(agent.UserId);
                if (user == null)
                {
                    AgentRepository.Delete(agent);
                    result.msg = $"找不到id为{model.Id}的代理";
                    return result;
                }
            }
            else
            {
                agent = new Agent();
                user = new Users();
            }
            //事物

            Mapper.Map(model, user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordSalt = HashUtils.GenerateSalt();
                user.Password = HashUtils.HashPasswordWithSalt(model.Password, user.PasswordSalt);
            }
            user.UserGroup = UserGroup.Agent;
            UsersRepository.Save(user);
            Mapper.Map(model, agent);
            agent.UserId = user.Id;
            AgentRepository.Save(agent);

            LogRepository.Insert(TableSource.Users, opType, user.Id);
            LogRepository.Insert(TableSource.Agent, opType, agent.Id);
            result.code = JsonModelCode.Succ;

            return result;
        }
    }
}
