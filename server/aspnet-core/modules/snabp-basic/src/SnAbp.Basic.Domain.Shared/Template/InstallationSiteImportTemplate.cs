using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Template
{
    public class InstallationSiteImportTemplate
    {
        public int Index { get; set; }

        public string CSRGCode { get; set; }

        public string Name { get; set; }

        //维护单位
        public string MaintainUnit { get; set; }

        public string Organization { get; set; }

        //产权单位
        public string PropertyRightUnit { get; set; }

        public string UseType { get; set; }

        public string Type { get; set; }

        public string LocationType { get; set; }

        public string Railway  { get; set; }

        public string Location { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; }

        public string State { get; set; }

        //结构
        public string Construction { get; set; }

        //面积
        public double Proportion { get; set; }

        //净高
        public double ClearHeight { get; set; }

        //海拔
        public double Elevation { get; set; }

        public string UseDate { get; set; }

        //录入单位
        public string EnteringUnit { get; set; }

        //更新人
        public string UpdatePerson { get; set; }
    }
}
