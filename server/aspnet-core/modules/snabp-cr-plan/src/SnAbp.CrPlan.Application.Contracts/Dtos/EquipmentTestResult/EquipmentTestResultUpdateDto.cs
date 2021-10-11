using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.EquipmentTestResult
{
    /// <summary>
    /// 设备测试项实体，修改使用
    /// </summary>
    public class EquipmentTestResultUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 测试结果
        /// </summary>
        [MaxLength(100)]
        public string TestResult { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public Guid? FileId { get; set; }

        /// <summary>
        /// 验收结果
        /// </summary>
        [MaxLength(100)]
        public string CheckResult { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}

