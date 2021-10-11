using System;

namespace SnAbp.Basic.Dtos
{
    public class InstallationSiteDetailDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 机房名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public Guid? UseOrg { get; set; }

        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string UseOrgName { get; set; }

        /// <summary>
        /// 使用类别(数据字典)
        /// </summary>
        public Guid? UseType { get; set; }

        /// <summary>
        /// 使用类别名称
        /// </summary>
        public string UseTypeName { get; set; }

        /// <summary>
        /// 机房类型(数据字典)
        /// </summary>
        public Guid Type { get; set; }

        /// <summary>
        /// 机房类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 位置分类(枚举)
        /// </summary>
        public int LocationType { get; set; }

        /// <summary>
        /// 位置分类名称
        /// </summary>
        public string LocationTypeName { get; set; }

        /// <summary>
        /// 线路描述
        /// </summary>
        public string RailwayDesc { get; set; }

        /// <summary>
        /// 位置描述
        /// </summary>
        public string LocationDesc { get; set; }

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
        public int State { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 投产日期
        /// </summary>
        public DateTime? CommissioningDate { get; set; }

        /// <summary>
        /// 线别Id
        /// </summary>
        public Guid? RailwayId { get; set; }

        /// <summary>
        /// 线别名称
        /// </summary>
        public string RailwayName { get; set; }

        /// <summary>
        /// 站区Id
        /// </summary>
        public Guid? StationId { get; set; }

        /// <summary>
        /// 站区名称
        /// </summary>
        public string StationName { get; set; }
    }
}
