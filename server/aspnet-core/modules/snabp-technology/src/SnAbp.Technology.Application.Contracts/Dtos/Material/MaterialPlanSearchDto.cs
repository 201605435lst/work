/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.Material
*******类 名 称： MaterialSearchDto
*******类 说 明： 材料信息查询dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:52:29 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Technology.enums;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos.Material
{
    /// <summary>
    /// 材料信息查询 
    /// </summary>
    public class MaterialPlanSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }
        /// <summary>
        /// 审核状态（采购计划的审核状态）
        /// </summary>
        public ApprovalStatus State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否查询审批数据（根据此字段过滤）
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// 是否获取待我审批的数据
        /// </summary>
        public bool Waiting { get; set; }
        /// <summary>
        /// 是否计划选择模式
        /// </summary>
        public bool IsSelect { get; set; }
        /// <summary>
        /// 是否提交了采购计划生成
        /// </summary>
        public string  Submit { get; set; }
        /// 物资管理模块查询
        /// </summary>
        public bool MaterialUse { get; set; }

    }
}
