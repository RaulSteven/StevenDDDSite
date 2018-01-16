using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Steven.Core.Extensions
{
    public static class EnumExtension
    {
        private static readonly Dictionary<string, string> EnumDescriptionDictionary = new Dictionary<string, string>();
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the Description attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriotion(this Enum value)
        {
            // Get the type
            var type = value.GetType();
            var key = string.Format("{0}-{1}", type.FullName, value);
            if (EnumDescriptionDictionary.ContainsKey(key)) return EnumDescriptionDictionary[key];
            lock (EnumDescriptionDictionary)
            {
                if (EnumDescriptionDictionary.ContainsKey(key)) return EnumDescriptionDictionary[key];
                var fieldInfo = type.GetField(value.ToString());
                        
                // Get the Description attributes
                var attribs = fieldInfo.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);
                EnumDescriptionDictionary.Add(key, attribs.Length == 0 ? "" : ((DescriptionAttribute)attribs[0]).Description);
            }
            // Get fieldinfo for this type
            return EnumDescriptionDictionary[key];
        }


        private static readonly Dictionary<string, SelectList> EnumSListDictionary = new Dictionary<string, SelectList>();

        public static SelectList GetSList(this Enum value)
        {
            return GetSList(value, null);
        }

        public static SelectList GetSList(this Enum value, params Enum[] showValues)
        {
            var enumType = value.GetType();
            var key = enumType.FullName;
            if (showValues != null && showValues.Length > 0)
            {
                foreach (var showValue in showValues)
                {
                    key += "-" + showValue;
                }
            }
            if (EnumSListDictionary.ContainsKey(key))
            {
                return EnumSListDictionary[key];
            }
            lock (EnumSListDictionary)
            {
                if (EnumSListDictionary.ContainsKey(key))
                {
                    return EnumSListDictionary[key];
                }
                var dic = new Dictionary<string, string>();
                var fieldInfos = enumType.GetFields();
                foreach (var filed in fieldInfos)
                {
                    if (filed.FieldType.IsEnum && 
                        (showValues == null || (showValues != null && showValues.Any(m=>m.ToString() == filed.Name))))
                    {
                        var objs = filed.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        dic.Add(filed.Name, objs.Length == 0 ? "" : ((DescriptionAttribute)objs[0]).Description);
                    }
                }

                var slist = new SelectList(dic, "key", "value");
                EnumSListDictionary.Add(key, slist);
            }
            return EnumSListDictionary[key];
        }

        private static readonly Dictionary<string, Dictionary<short, string>> EnumDescrptDictionary = new Dictionary<string, Dictionary<short, string>>();

        public static Dictionary<short, string> GetDescriptDict(this Enum value)
        {
            return GetDescriptDict(value, null);
        }

        public static Dictionary<short, string> GetDescriptDict(this Enum value, params Enum[] showValues)
        {
            var enumType = value.GetType();
            var key = enumType.FullName;
            if (showValues != null && showValues.Length > 0)
            {
                foreach (var showValue in showValues)
                {
                    key += "-" + showValue;
                }
            }
            if (EnumDescrptDictionary.ContainsKey(key))
            {
                return EnumDescrptDictionary[key];
            }
            lock (EnumDescrptDictionary)
            {
                if (EnumDescrptDictionary.ContainsKey(key))
                {
                    return EnumDescrptDictionary[key];
                }
                var dic = new Dictionary<short, string>();
                var fieldInfos = enumType.GetFields();
                foreach (var filed in fieldInfos)
                {
                    if (filed.FieldType.IsEnum &&
                        (showValues == null || (showValues != null && showValues.Any(m => m.ToString() == filed.Name))))
                    {
                        var objs = filed.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        dic.Add(Convert.ToInt16(filed.GetValue(null)), objs.Length == 0 ? "" : ((DescriptionAttribute)objs[0]).Description);
                    }
                }
                EnumDescrptDictionary.Add(key, dic);
            }
            return EnumDescrptDictionary[key];
        }

        public static short ToShort(this Enum value)
        {
            return Convert.ToInt16(value);
        }

        private static readonly Dictionary<string, SelectList> EnumSListIntDictionary = new Dictionary<string, SelectList>();

        public static SelectList GetSListInt(this Enum value)
        {
            var enumType = value.GetType();
            var key = enumType.FullName;
            if (EnumSListIntDictionary.ContainsKey(key))
            {
                return EnumSListIntDictionary[key];
            }
            lock (EnumSListIntDictionary)
            {
                if (EnumSListIntDictionary.ContainsKey(key))
                {
                    return EnumSListIntDictionary[key];
                }
                var dic = new Dictionary<int, string>();
                foreach (Enum enumValue in value.GetType().GetEnumValues())
                {
                    dic.Add(enumValue.ToShort(), enumValue.GetDescriotion());
                } 

                var slist = new SelectList(dic, "key", "value");
                EnumSListIntDictionary.Add(key, slist);
            }
            return EnumSListIntDictionary[key];
        }
    }
}