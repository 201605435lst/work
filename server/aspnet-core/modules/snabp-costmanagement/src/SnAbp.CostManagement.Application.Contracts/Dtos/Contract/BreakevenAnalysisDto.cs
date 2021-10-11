using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.CostManagement.Dtos.Contract
*文件名：BreakevenAnalysisDto
*创建人： liushengtao
*创建时间：2021/8/17 13:34:46
*描述：
*
***********************************************************************/
namespace SnAbp.CostManagement.Dtos
{
    public class BreakevenAnalysisDto
    {
        /// <summary>
        /// 当前机构名称
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public List<string> Dates { get; set; } = new List<string>();
        /// <summary>
        /// 合同额
        /// </summary>
        public List<Decimal> ContractAmount { get; set; } = new List<decimal>();
        /// <summary>
        /// 总支出
        /// </summary>
        public List<Decimal> TotalExpenditure { get; set; } = new List<decimal>();
    }
}
