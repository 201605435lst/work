using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class StationSimpleSearchDto
    {
        public string Keyword { get; set; }

        public byte? Type { get; set; }
    }
}
