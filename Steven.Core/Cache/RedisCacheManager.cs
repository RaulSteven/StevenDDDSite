using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using CacheManager.Core;
using CacheManager.Core.Internal;
using CacheManager.Redis;
using CacheManager.SystemRuntimeCaching;
using Steven.Core.Utilities;

namespace Steven.Core.Cache
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly static ICacheManager<object> Cache;

        public RedisCacheManager()
        {
            
        }

        static RedisCacheManager()
        {
            var configKey = ConfigurationManager.AppSettings["RedisConfigKey"];
            var redisHost = ConfigurationManager.AppSettings["RedisHost"];
            var redisPort = StringUtility.ConvertToInt(ConfigurationManager.AppSettings["RedisPort"]);
            Cache = CacheFactory.Build<object>("MLSCache", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("inProcessCache")
                    .EnableStatistics()
                    .EnablePerformanceCounters()
                    .And
                    .WithRedisConfiguration(configKey, config =>
                    {
                        config.WithAllowAdmin()
                            .WithDatabase(0)
                            .WithEndpoint(redisHost, redisPort);
                    })
                    .WithMaxRetries(1000)
                    .WithRetryTimeout(100)
                    .WithRedisBackplane(configKey)
                    .WithRedisCacheHandle(configKey, true)
                    .EnableStatistics()
                    .EnablePerformanceCounters();
            });
        }
        public T Get<T>(string key)
        {
            var obj = Cache.Get(key);
            return Get<T>(obj);
        }

        private T Get<T>(object obj)
        {
            if (obj is T)
            {
                return (T)obj;
            }
            return default(T);
        }
        public T Get<T>(string key, string region)
        {
            var obj = Cache.Get(key, region);
            return Get<T>(obj);
        }

        public void Set(string key, object data, int cacheTime)
        {
            Cache.AddOrUpdate(key, data, m => data);
        }

        public void Set(string key, object data, int cacheTime, string region)
        {
            Cache.AddOrUpdate(key, region, data, m => data);
        }

        public void Add(string key, object data, int cacheTime)
        {
            var cachItem = new CacheItem<object>(key, data, ExpirationMode.Absolute, TimeSpan.FromMinutes(cacheTime));
            Cache.Add(cachItem);
        }

        public void Add(string key, object data, int cacheTime, string region)
        {
            var cachItem = new CacheItem<object>(key,region, data, ExpirationMode.Absolute,TimeSpan.FromMinutes(cacheTime));
            Cache.Add(cachItem);
        }

        public bool Contains(string key)
        {
            var value = Cache.Get(key);
            return value != null;
        }

        public bool Contains(string key, string region)
        {
            var value = Cache.Get(key, region);
            return value != null;
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Remove(string key, string region)
        {
            Cache.Remove(key, region);
        }

        public void RemoveByPattern(string pattern)
        {
            Cache.ClearRegion(pattern);
        }

        public void Clear()
        {
            Cache.Clear();
        }

        public long GetCount(string pattern)
        {
            long totalCount = 0;
            foreach (var baseCacheHandle in Cache.CacheHandles)
            {
                if (baseCacheHandle is RedisCacheHandle<object>)
                {
                    var states = baseCacheHandle.Stats;
                    totalCount += string.IsNullOrEmpty(pattern) ?
                        states.GetStatistic(CacheStatsCounterType.Items) :
                        states.GetStatistic(CacheStatsCounterType.Items, pattern);
                    break;
                }
            }
            return totalCount;
        }

    }
}