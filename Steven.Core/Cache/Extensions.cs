using System;

namespace Steven.Core.Cache
{
    /// <summary>
    /// Extensions
    /// add by lrc
    /// </summary>
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.Contains(key))
            {
                return cacheManager.Get<T>(key);
            }
            var result = acquire();
            cacheManager.Add(key, result, cacheTime);
            return result;
        }

        public static T Get<T>(this ICacheManager cacheManager, string key, string region, Func<T> acquire)
        {
            return Get(cacheManager, key, region, 60, acquire);
        }

        public static T Get<T>(this ICacheManager cacheManager, string key, string region, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.Contains(key, region))
            {
                return cacheManager.Get<T>(key, region);
            }
            var result = acquire();
            cacheManager.Add(key, result, cacheTime, region);
            return result;
        }
    }
}
