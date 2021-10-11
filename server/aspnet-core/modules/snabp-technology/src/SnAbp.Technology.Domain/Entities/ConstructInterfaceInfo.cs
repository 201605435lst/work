/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： ConstructInterfaceInfo
*******类 说 明： 接口标记信息表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/23 16:48:06
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Technology.enums;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    /// 接口标记信息表
    /// </summary>
    public class ConstructInterfaceInfo : AuditedEntity<Guid>
    {
        public ConstructInterfaceInfo()
        {

        }
        public void SetId(Guid id) => Id = id;
        public ConstructInterfaceInfo(Guid id) => Id = id;

        /// <summary>
        /// 接口清单id
        /// </summary>
        public virtual Guid? ConstructInterfaceId { get; set; }
        public virtual ConstructInterface ConstructInterface { get; set; }
        /// <summary>
        /// 检查人员
        /// </summary>
        public virtual Guid MarkerId { get; set; }
        public virtual IdentityUser Marker { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual Guid BuilderId { get; set; }
        public virtual DataDictionary Builder { get; set; }
        /// <summary>
        /// 接口检查情况
        /// </summary>
        public virtual MarkType MarkType { get; set; }

        /// <summary>
        /// 检查原因
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// 标记时间
        /// </summary>
        public virtual DateTime? MarkDate { get; set; }

        /// <summary>
        /// 整改人
        /// </summary>
        public virtual Guid? ReformerId { get; set; }
        public virtual IdentityUser Reformer { get; set; }
        /// <summary>
        /// 整改时间
        /// </summary>
        public virtual DateTime? ReformDate { get; set; }

        /// <summary>
        /// 整改说明
        /// </summary>
        public virtual string ReformExplain { get; set; }

        public  virtual List<ConstructInterfaceInfoRltMarkFile> MarkFiles { get; set; }

        public Guid? ProjectTagId { get; set; }
    }
}
