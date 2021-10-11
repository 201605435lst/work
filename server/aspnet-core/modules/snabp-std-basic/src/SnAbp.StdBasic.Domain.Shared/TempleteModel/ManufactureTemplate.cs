using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
    public class ManufactureTemplate
    {
        public int Index { get; set; }

        public string CSRGCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Introduction { get; set; }

        public string Principal { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }

        
    }
}
