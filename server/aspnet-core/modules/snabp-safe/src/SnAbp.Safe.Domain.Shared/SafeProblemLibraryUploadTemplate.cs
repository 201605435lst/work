/**********************************************************************
*******命名空间： SnAbp.Safe
*******类 名 称： SafeProblemLibraryUploadTemplate
*******类 说 明： 问题库导入模板
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 16:09:27
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Safe
{
    /// <summary>
    /// 问题库导入模板
    /// </summary>
    public class SafeProblemLibraryUploadTemplate
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        public string RiskLevel { get; set; }
        /// <summary>
        /// 所属专业
        /// </summary>
        public string Profession { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// 风险因素
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        ///  控制措施
        /// </summary>
        public string Measures { get; set; }
        /// <summary>
        /// 使用范围 单位名称之间用逗号隔开
        /// </summary>
        public string Scops { get; set; }
    }
}
