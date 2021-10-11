/**********************************************************************
*******命名空间： SnAbp.Resource.Entities
*******类 名 称： Profession
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 4:44:32 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 专业实体 ，用户工程量的统计
    /// </summary>
    public class Profession
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 专业名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
    }
}
