using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AI_Assistant_Win.Utils
{
    public static class EnumHelper
    {
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName<T>(this int status)
        {
            return Enum.GetName(typeof(T), status);
        }
        /// <summary>
        /// 获取枚举名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetNamesArr<T>()
        {
            return Enum.GetNames(typeof(T));
        }
        /// <summary>
        /// 将枚举转换成字典集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> getEnumDic<T>()
        {

            Dictionary<string, int> resultList = new Dictionary<string, int>();
            Type type = typeof(T);
            var strList = GetNamesArr<T>().ToList();
            foreach (string key in strList)
            {
                string val = Enum.Format(type, Enum.Parse(type, key), "d");
                resultList.Add(key, int.Parse(val));
            }
            return resultList;
        }
        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetDic<TEnum>()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add(item.ToString(), (int)item);
            }

            return dic;
        }
        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetNameDescs<TEnum>()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type t = typeof(TEnum);

            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                int val = (int)item;
                Type enumtype = val.GetType();
                FieldInfo fieldInfo = t.GetField(Enum.GetName(t, val));
                DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                dic.Add(item.ToString(), attr.Description);
            }

            return dic;
        }

        /// <summary>
        /// 如何获取C# 如何从枚举值获取Description？
        /// https://www.cnblogs.com/plain-coder/p/16077349.html
        /// https://zhuanlan.zhihu.com/p/397632335
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string GetDescriptionOfEnum<T>(string enumName) where T : Enum
        {
            var enumNames = typeof(T).GetEnumNames();
            if (enumNames.Contains(enumName))
            {
                var @enum = (T)Enum.Parse(typeof(T), enumName, true);
                return @enum.GetDescription();
            }
            return null;
        }

        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (string.IsNullOrWhiteSpace(name))
                return value.ToString();

            var field = type.GetField(name);
            var des = field?.GetCustomAttribute<DescriptionAttribute>();
            if (des == null)
                return value.ToString();

            return des.Description;
        }
    }
}
