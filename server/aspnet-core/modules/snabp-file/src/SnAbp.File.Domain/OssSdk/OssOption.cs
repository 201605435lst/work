/**********************************************************************
*******命名空间： SnAbp.File.OssSdk
*******类 名 称： OssOption
*******类 说 明： oss 对象存储配置类，用来初始化不同客户端使用
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 15:13:11
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

namespace SnAbp.File.OssSdk
{
    /// <summary>
    ///     oss 对象存储配置类，用来初始化不同客户端使用
    /// </summary>
    public class OssOption
    {
        /// <summary>
        ///     服务地址
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        ///     账号
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        ///     上传链接过期时间
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        ///     可上传的文件最大长度
        /// </summary>
        public string MaxLength { get; set; }

        /// <summary>
        ///     服务类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     公共桶
        /// </summary>
        public string PublicBucket { get; set; }

        /// <summary>
        ///     加密桶
        /// </summary>
        public string SecurityBucket { get; set; }
    }
}