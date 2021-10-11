using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Dtos
{
   public class ConstructInterfaceTemplateDto
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 接口编号
        /// </summary>
        public virtual string Code { get; set; }

        //// 施工地点
        //public string ConstructionSection{ get; set; }
        /// <summary>
        /// 接口位置
        /// </summary>
        public virtual string Position { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public string InterfaceManagementType { get; set; }

        /// <summary>
        /// 专业 DataDictionaryId
        /// </summary>
        public string Profession { get; set; }
        /// <summary>
        /// 材料名称
        /// </summary>
        public string MarerialName { get; set; }
        /// <summary>
        /// 材料规格
        /// </summary>
        public string MaterialSpec { get; set; }


        /// <summary>
        /// 材料数量
        /// </summary>
        public string MarerialCount { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual string Builder { get; set; }
        /// <summary>
        /// 设备分组
        /// </summary>
        public virtual string EquipmentGroup { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public virtual string Equipment { get; set; }
      
        ///// <summary>
        ///// 接口检查情况
        ///// </summary>
        //public virtual string MarkType { get; set; }

        /// <summary>
        /// 接口数据
        /// </summary>
        public virtual string GisData { get; set; }


    }
}
