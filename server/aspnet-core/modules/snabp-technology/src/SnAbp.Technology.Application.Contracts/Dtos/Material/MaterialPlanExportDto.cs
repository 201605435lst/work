using SnAbp.Bpm.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Dtos
{
    public class MaterialPlanExportDto : MaterialPlanDto
    {
        public List<SingleFlowNodeDto> Nodes { get; set; }
    }
}
