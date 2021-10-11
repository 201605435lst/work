using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 年月表变更提交审核
    /// </summary>
    public class YearMonthChangeExamDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 生成月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 生成类型(年表,月表,年度月表)
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrgId { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 附件文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 附件文件地址
        public Guid? FileId { get; set; }

        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 新增续期: 添加年月表变更记录
        /// 判断是否为保存？
        /// </summary>
        //public bool IsSubmit { get; set; }

        public Guid? AlterRecordId { get; set; }


    }
}
