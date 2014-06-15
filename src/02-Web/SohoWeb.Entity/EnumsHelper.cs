using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Soho.Utility;

namespace SohoWeb.Entity
{
    public static class EnumsHelper
    {
        /// <summary>
        /// 获取枚举内容项，并以KeyValuePair列表的方式返回；其中，Key=枚举的值，Value=枚举的描述
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="appendType">选择是否在列表项前插入一条选项的选项类型，比如是否插入所有|请选择等Key为Null的选项</param>
        /// <param name="customApplyDesc">如果要在列表项前插入一条选项，该选项的自定义描述</param>
        /// <returns></returns>
        public static List<KeyValuePair<Nullable<TEnum>, string>> GetKeyValuePairs<TEnum>(EnumAppendItemType appendType, params string[] customApplyDesc) where TEnum : struct
        {
            List<KeyValuePair<Nullable<TEnum>, string>> keyValuePairList = new List<KeyValuePair<Nullable<TEnum>, string>>();
            Type enumType = typeof(TEnum);
            if (enumType.IsEnum || IsGenericEnum(enumType))
            {
                Dictionary<TEnum, string> dic = EnumHelper.GetDescriptions<TEnum>();
                if (dic != null && dic.Count > 0)
                {
                    foreach (TEnum e in dic.Keys)
                    {
                        keyValuePairList.Add(new KeyValuePair<Nullable<TEnum>, string>(e, dic[e]));
                    }
                }
                if (appendType != EnumAppendItemType.None)
                {
                    if (customApplyDesc != null && customApplyDesc.Length > 0 && !string.IsNullOrEmpty(customApplyDesc[0]))
                    {
                        KeyValuePair<Nullable<TEnum>, string> kv = new KeyValuePair<Nullable<TEnum>, string>(null, customApplyDesc[0]);
                        keyValuePairList.Insert(0, kv);
                    }
                    else
                    {
                        string desc = GetEnumDescription(appendType);
                        if (!string.IsNullOrEmpty(desc))
                        {
                            KeyValuePair<Nullable<TEnum>, string> kv = new KeyValuePair<Nullable<TEnum>, string>(null, desc);
                            keyValuePairList.Insert(0, kv);
                        }
                    }
                }
            }

            return keyValuePairList;
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            return value == null ? string.Empty : EnumHelper.GetDescription(value);
        }

        private static bool IsGenericEnum(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    && type.GetGenericArguments() != null
                    && type.GetGenericArguments().Length == 1 && type.GetGenericArguments()[0].IsEnum);
        }
    }

    public enum EnumAppendItemType
    {
        None,
        /// <summary>
        /// 默认“所有”项
        /// </summary>
        [Description("--所有--")]
        All,
        /// <summary>
        /// 默认“请选择”项
        /// </summary>
        [Description("--请选择--")]
        Select
    }
}
