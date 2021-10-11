using SnAbp.Bpm.Entities;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class FlowTemplateNodeDto : EntityDto<Guid>
    {
        public string Label { get; set; }
        public List<float> Size { get; set; }
        public string Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool Active { get; set; } = false;
        public string Code { get; set; }
        public virtual List<FlowNodeFormItemPermisstion> FormItemPermisstions { get; set; }
        public virtual List<MemberDto> Members { get; set; }
    }
}
