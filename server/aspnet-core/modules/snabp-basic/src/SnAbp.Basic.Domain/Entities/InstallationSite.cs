using SnAbp.Basic.Enums;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Basic.Entities
{
    public class InstallationSite : FullAuditedEntity<Guid>, IGuidKeyTree<InstallationSite>
    {
        /// <summary>
        /// 父级
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        public virtual InstallationSite Parent { get; set; }
        public virtual List<InstallationSite> Children { get; set; }

        /// <summary>
        /// 安装位置(机房、区域、位置)
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// CSRG编码
        /// </summary>
        public string CSRGCode { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 编码名称
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 使用类别(枚举)
        /// </summary>
        public virtual InstallationSiteUseType? UseType { get; set; }

        /// <summary>
        /// 分类(数据字典) 一类、二类等
        /// </summary>
        public virtual Guid? TypeId { get; set; }
        public virtual DataDictionary Type { get; set; }

        /// <summary>
        /// 类别(数据字典) 中继站、变电所等
        /// </summary>
        public virtual Guid? CategoryId { get; set; }
        public virtual DataDictionary Category { get; set; }

        /// <summary>
        /// 位置类型(枚举)
        /// </summary>
        public virtual InstallationSiteLocationType? LocationType { get; set; }

        /// <summary>
        /// 线路描述
        /// </summary>
        public virtual RailwayDirection? RailwayDirection { get; set; }

        /// <summary>
        /// 机房位置
        /// </summary>
        [MaxLength(100)]
        public string Location { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 状态(枚举)
        /// </summary>
        public virtual InstallationSiteState State { get; set; }

        /// <summary>
        /// 投产日期
        /// </summary>
        public DateTime? UseDate { get; set; }

        /// <summary>
        /// 线别Id
        /// </summary>
        public Guid? RailwayId { get; set; }
        public virtual Railway Railway { get; set; }

        /// <summary>
        /// 站区Id
        /// </summary>
        public Guid? StationId { get; set; }
        public virtual Station Station { get; set; }

        /// <summary>
        /// 起始公里标
        /// </summary>
        public int KMMark { get; set; }

         public Guid? ProjectTagId { get; set; }

        protected InstallationSite() { }
        public InstallationSite(Guid id)
        {
            Id = id;
        }

        public static explicit operator Organization(InstallationSite v)
        {
            throw new NotImplementedException();
        }
    }
}
