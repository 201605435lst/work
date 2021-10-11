using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.TempleteModel
{
    public class EquipmentModel
    {
        public int Index { get; set; }

        public string Organization { get; set; }

        public string EquipmentSubclass { get; set; }

        public string ElementType { get; set; }

        public string Group { get; set; }

        public string SectionIFD { get; set; }

        public string CSRGCode { get; set; }

        public string Name { get; set; }

        public string SystemName { get; set; }

        /// <summary>
        /// 车站-所属线路
        /// </summary>
        public string StaRailwayId { get; set; }

        /// <summary>
        /// 车站-车站名称
        /// </summary>
        public string StaStationId { get; set; }

        /// <summary>
        /// 车站-安装地点
        /// </summary>
        public string StaMachineRoomId { get; set; }

        /// <summary>
        /// 区间-所属线路
        /// </summary>
        public string ZoneRailwayId { get; set; }

        /// <summary>
        /// 区间-公里标
        /// </summary>
        public string ZoneKilometerMark { get; set; }

        /// <summary>
        /// 区间-安装地点
        /// </summary>
        public string ZoneMachineRoomId { get; set; }

        /// <summary>
        /// 其它-所属线路
        /// </summary>
        public string OthRailwayId { get; set; }

        /// <summary>
        /// 其它-安装地点
        /// </summary>
        public string OthMachineRoomId { get; set; }

        public string InstallationSiteCode { get; set; }

        public string MaintenanceOrganizationCode { get; set; }

        public string Unit { get; set; }

        public string Manufacturer { get; set; }

        public string ProductCategory { get; set; }

        public string UseDate { get; set; }

        public string State { get; set; }
    }
}
