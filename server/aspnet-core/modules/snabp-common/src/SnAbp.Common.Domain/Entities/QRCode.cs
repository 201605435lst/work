/**********************************************************************
*******命名空间： SnAbp.Common.Entities
*******类 名 称： QRCode
*******类 说 明： 二维码配置实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/12 11:51:06
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Common.Entities
{
    /// <summary>
    /// 二维码配置实体
    /// </summary>
    public class QRCode:Entity<Guid>
    {
        public QRCode(Guid id) => this.Id = id;
        /// <summary>
        /// 是否显示边框
        /// </summary>

        public bool Border { get; set; }

        /// <summary>
        /// 二维码尺寸
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 二维码版本
        /// </summary>
        public decimal Version { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public string ImageBase64Str { get; set; }
        /// <summary>
        /// 图片大小
        /// </summary>
        public decimal ImageSize { get; set; }
        /// <summary>
        /// 是否显示小图标
        /// </summary>
        public bool ShowLog { get; set; }
    }
}
