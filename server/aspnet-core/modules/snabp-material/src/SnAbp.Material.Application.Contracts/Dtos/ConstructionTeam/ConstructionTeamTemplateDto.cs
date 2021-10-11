using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Dtos
{
    public class ConstructionTeamTemplateDto
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 施工点
        /// </summary>
        public string ConstructionSectionName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string PeopleName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
    }
}
