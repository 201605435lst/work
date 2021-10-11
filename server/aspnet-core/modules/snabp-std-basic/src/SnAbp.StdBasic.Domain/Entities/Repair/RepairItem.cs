using SnAbp.Identity;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    public class RepairItem : Entity<Guid>
    {
        protected RepairItem() { }
        public RepairItem(Guid id) { Id = id; }

        /// <summary>
        /// 维修分组
        /// </summary>
        public Guid GroupId { get; set; }
        public RepairGroup Group { get; set; }
     

        /// <summary>
        /// 维修类型
        /// </summary>
        public RepairType Type { get; set; }


        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        [MaxLength(1000)]
        public string Content { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [MaxLength(20)]
        public string Unit { get; set; }

        /// <summary>
        /// 周期：每年/月次数
        /// </summary>
        [MaxLength(50)]
        public string Period { get; set; }

        /// <summary>
        /// 周期单位：每年/月
        /// </summary>
        [MaxLength(50)]
        public RepairPeriodUnit PeriodUnit { get; set; }

        /// <summary>
        /// 是否为月维修项
        /// </summary>
        public bool IsMonth { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }


        [InverseProperty("RepairItem")]
        /// <summary>
        /// 测试项
        /// </summary>
        public List<RepairTestItem> RepairTestItems { get; set; }


        [InverseProperty("RepairItem")]
        /// <summary>
        /// 构件分类
        /// </summary>
        public List<RepairItemRltComponentCategory> RepairItemRltComponentCategories { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public Guid? TagId { get; set; }
        public DataDictionary Tag { get; set; }


        [InverseProperty("RepairItem")]
        /// <summary>
        /// 执行部门类型
        /// </summary>
        public List<RepairItemRltOrganizationType> RepairItemRltOrganizationTypes { get; set; }
    }
}
