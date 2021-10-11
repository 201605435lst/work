/**********************************************************************
*******命名空间： SnAbp.Message
*******类 名 称： MessageExtension
*******类 说 明： 消息模块中扩展方法的封装
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/17 10:48:47
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnAbp.Message
{
    public static class MessageExtension
    {
        public static IReadOnlyList<string> ToStringList(this IReadOnlyList<Guid> ids) =>
            ids.Select(a => a.ToString()).ToList();

        public static List<T> AddNewRange<T>(this List<T> list, IEnumerable<T> newList)
        {
            list.AddRange(newList);
            return list;
        }

        /// <summary>
        ///     枚举类型转枚举名称的字符串名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string EnumToString<T>(this T t) where T : Enum => Enum.GetName(enumType: typeof(T), t);

        /// <summary>
        ///     字符串转对应的枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(this string str) where T : Enum =>
            (T) Enum.Parse(enumType: typeof(T), str, true);

        /// <summary>
        ///     格式化对象为字符串，用于返回前端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T t) => t is string?t.ToString():JsonConvert.SerializeObject(t, GetJsonSerializerSettings());

        private static JsonSerializerSettings GetJsonSerializerSettings() => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        ///     获取消息类型
        /// </summary>
        /// <typeparam name="data"></typeparam>
        /// <returns></returns>
        public static T GetMessage<T>(this byte[] data) where T : class
        {
            try
            {
                using var ms = new MemoryStream(data);
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms) as T;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<byte[]> ConvertToBytes(this Stream stream)
        {
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            buffer.Position = 0;
            var bys = new byte[buffer.Length];
            await buffer.ReadAsync(bys, 0, bys.Length);
            return bys;
        }

        public static byte[] GetBytes(this object obj)
        {
            using var stream = new MemoryStream();
            var serialize = new BinaryFormatter();
            serialize.Serialize(stream, obj);
            return stream.GetBuffer();
        }
    }
}