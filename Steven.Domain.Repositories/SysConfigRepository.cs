using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Steven.Core.Cache;
using Steven.Core.Utilities;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using log4net;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using Dapper;
using Newtonsoft.Json;

namespace Steven.Domain.Repositories
{
    public class SysConfigRepository : Repository<SysConfig>, ISysConfigRepository
    {
        public ICacheManager Cache { get; set; }

        #region constraints

        public string ConfigPatternByKey
        {
            get
            {
                return CacheKey + "ByCode.{0}";
            }
        }
        public const int ConfigCacheTimeOut = 600;

        #endregion

        public SysConfig GetByKey(string key)
        {
            return DbConn.QueryFirstOrDefault<SysConfig>(Query() + " and ConKey =@key", new { key });
        }

        private T GetConfig<T>(MethodBase method)
        {
            try
            {
                var configKey = method.Name;
                if (configKey.StartsWith("get_"))
                {
                    configKey = configKey.Substring(4).Trim();
                }
                else
                {
                    throw new Exception("GetConfig 方法只能在get属性中调用！");
                }
                var cacheKey = string.Format(ConfigPatternByKey, configKey);
                var result = Cache.Get<T>(cacheKey, CacheKey, () =>
                {
                    var config = GetByKey(configKey);
                    if (config == null)
                    {
                        var typeName = typeof(T).Name;
                        config = CreateDefaultConfig(typeName, configKey);
                        Save(config);
                    }
                    T cacheResult;
                    if (string.IsNullOrEmpty(config.ConValue))
                    {
                        cacheResult = default(T);
                    }
                    else
                    {
                        cacheResult = JsonConvert.DeserializeObject<T>(config.ConValue);
                    }
                    return cacheResult;
                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return default(T);
            }
        }

        private SysConfig CreateDefaultConfig(string typeName, string configKey)
        {
            var config = new SysConfig()
            {
                ConKey = configKey,
                Name = configKey,
                ConfigClassify = SysConfigClassify.None,
                ConfigType = SysConfigType.String
            };
            if (typeName.Equals(typeof(string).Name))
            {
                config.ConfigType = SysConfigType.String;
                config.ConValue = JsonConvert.SerializeObject("");
            }
            else if (typeName.Equals(typeof(bool).Name))
            {
                config.ConfigType = SysConfigType.Bool;
                config.ConValue = JsonConvert.SerializeObject(false);
            }
            else if (typeName.Equals(typeof(int).Name))
            {
                config.ConfigType = SysConfigType.Int;
                config.ConValue = JsonConvert.SerializeObject(0);
            }
            else if (typeName.Equals(typeof(long).Name))
            {
                config.ConfigType = SysConfigType.Long;
                config.ConValue = JsonConvert.SerializeObject(0L);
            }
            else if (typeName.Equals(typeof(string[]).Name))
            {
                config.ConfigType = SysConfigType.StringArray;
                config.ConValue = JsonConvert.SerializeObject(new string[0]);
            }

            return config;

        }

        public override void RemoveCache(SysConfig entity)
        {
            var cacheKey = string.Format(ConfigPatternByKey, entity.ConKey);
            Cache.Remove(cacheKey, CacheKey);
            Cache.Remove(cacheKey + "Id", CacheKey);
        }


        #region 网站配置

        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebSiteName
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// 网站CopyRight
        /// </summary>
        public string WebSiteCopyRight
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        public string WebSiteICP
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }


        public string WebSiteUrl
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        public string SitemapPath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string CompanyAddr
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string CompanyTel
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// 日志记录路径
        /// </summary>
        public string LogFilePath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }


        #endregion

        #region 文件上传配置
        public string UploadPath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        public string[] UploadFileTypes
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
        }

        public int UploadFileSizes
        {
            get
            {
                return GetConfig<int>(MethodBase.GetCurrentMethod());
            }
        }

        public string WaterMarkingPath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }


        public string UploadRootDirectory
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        public string UploadThumbDirectory
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region 缩略图设置

        public string[] ImgSites
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region 统计
        public string StatisticsJS
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region icon 列表

        public string[] Icons
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region 查询
        public Pager<SysConfig> GetPager(string keyword, SysConfigClassify? clz, PageSearchModel search)
        {
            var sql = new StringBuilder(AdminQuery());
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and (ConKey like @key or Name like @key)");
                param.Add("@key", $"%{keyword}%");
            }
            if (clz.HasValue)
            {
                sql.Append(" and ConfigClassify = @clz");
                param.Add("@clz", clz.Value);
            }
            return Pager(sql.ToString(), param, search);
        }
        #endregion

        #region 商户系统
       public long ArticleProductDynamicId
        {
            get
            {
                return GetConfig<long>(MethodBase.GetCurrentMethod());
            }
        }
        #endregion
        #region // 微信公众号配置
        public string WxAppId
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }
        public string WxAppSecret
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region // 商户通知模板消息
        public string ShopUserDownTemplateId
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        public string ShopUserPayTemplateId
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }

        public string ShopUserTakeTemplateId
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
        }
        #endregion
    }
}
