/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos
*******类 名 称： DiscloseCreateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/30 14:00:13
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class DiscloseCreateDto: EntityDto<Guid>
    {
        public DiscloseCreateDto() { }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 视频地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        public DiscloseDto Parent { get; set; }
        public DiscloseType Type { get; set; }
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
