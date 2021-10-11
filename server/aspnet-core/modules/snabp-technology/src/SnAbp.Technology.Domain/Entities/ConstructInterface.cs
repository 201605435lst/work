/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： ConstructInterface
*******类 说 明： 接口表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/23 15:10:55
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Entities;
using SnAbp.Technology.enums;

using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    /// 接口表
    /// </summary>
    public class ConstructInterface : AuditedEntity<Guid>
    {
        public ConstructInterface() { }
        public ConstructInterface(Guid id) { Id = id; }
        public void SetId(Guid id) { Id = id; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 接口编号
        /// </summary>
        public virtual string Code { get; set; }

        //// 施工地点
        //public virtual Guid? ConstructionSectionId { get; set; }
        //public virtual ConstructionSection ConstructionSection { get; set; }
        /// <summary>
        /// 专业 DataDictionaryId
        /// </summary>
        public virtual Guid? ProfessionId { get; set; }
        public virtual Identity.DataDictionary Profession { get; set; }
        /// <summary>
        /// 接口位置
        /// </summary>
        public virtual string Position { get; set; }
        /// <summary>
        /// 材料规格
        /// </summary>
        public string MaterialSpec { get; set; }
        /// <summary>
        /// 材料名称
        /// </summary>
        public string MarerialName { get; set; }

        /// <summary>
        /// 材料数量
        /// </summary>
        public string MarerialCount { get; set; }
        /// <summary>
        /// 接口数据
        /// </summary>
        public virtual string GisData { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual Guid? BuilderId { get; set; }
        public virtual DataDictionary Builder { get; set; }
        /// <summary>
        /// 接口检查情况
        /// </summary>
        public virtual MarkType MarkType { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public virtual Guid? InterfaceManagementTypeId { get; set; }
        public virtual DataDictionary InterfaceManagementType { get; set; }
        

        /// <summary>
        /// 设备id
        /// </summary>
        public virtual Guid? EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        /// <summary>
        /// 情况标记信息表
        /// </summary>
        public virtual List<ConstructInterfaceInfo> ConstructInterfaceInfos { get; set; }
        public Guid? ProjectTagId { get; set; }


    }
}
