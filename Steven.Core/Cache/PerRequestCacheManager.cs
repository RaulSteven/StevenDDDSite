using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace Steven.Core.Cache
{
    /// <summary>
    /// Represents a NopStaticCache
    /// add by lrc
    /// </summary>
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContext _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new instance of the NopRequestCache class
        /// </summary>
        protected IDictionary GetItems()
        {
            if (_context != null)
            {
                return _context.Items;
            }

            return null;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
            {
                return default(T);
            }

            return (T)items[key];
        }

        public T Get<T>(string key, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
            {
                return;
            }

            if (data != null)
            {
                if (items.Contains(key))
                {
                    items[key] = data;
                }
                else
                {
                    items.Add(key, data);
                }
            }
        }

        public void Set(string key, object data, int cacheTime, string region)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, object data, int cacheTime, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool Contains(string key)
        {
            var items = GetItems();
            if (items == null)
            {
                return false;
            }

            return (items[key] != null);
        }

        public bool Contains(string key, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
            {
                return;
            }

            items.Remove(key);
        }

        public void Remove(string key, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                {
                    keysToRemove.Add(enumerator.Key.ToString());
                }
            }

            foreach (string key in keysToRemove)
            {
                items.Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                keysToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string key in keysToRemove)
            {
                items.Remove(key);
            }
        }

        public long GetCount(string pattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Add(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                {
                    return;
                }
                items.Add(key, data);
            }
        }
    }
}