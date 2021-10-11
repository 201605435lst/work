using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{
    [NotMapped]
    public class FormItemOption
    {
        public string Action { get; set; }
        public bool? AllowHalf { get; set; }


        public bool? Banner { get; set; }


        public bool? Clearable { get; set; }
        public bool? Closable { get; set; }
        public bool? Chinesization { get; set; }


        public string Data { get; set; }
        public object DefaultValue { get; set; }
        public bool? Disabled { get; set; }
        public bool? Dynamic { get; set; }
        public string DynamicKey { get; set; }
        public string DynamicFun { get; set; }
        public string Drag { get; set; }
        public string Description { get; set; }


        public int? Limit { get; set; }


        public bool? Filterable { get; set; }
        public string Format { get; set; }


        public int? Height { get; set; }
        public string Handle { get; set; }


        public int? Max { get; set; }
        public int? MaxRows { get; set; }
        public int? MaxLength { get; set; }
        public int? Min { get; set; }
        public int? MinRows { get; set; }
        public bool? Multiple { get; set; }


        public string Placeholder { get; set; }


        public bool? Range { get; set; }
        public List<string> RangeDefaultValue { get; set; }
        public List<string> RangePlaceholder { get; set; }


        public bool? ShowTime { get; set; }
        public bool? ShowLabel { get; set; }
        public bool? ShowInput { get; set; }
        public bool? ShowIcon { get; set; }
        public bool? ShowRequiredMark { get; set; }


        public string Step { get; set; }


        public string Type { get; set; }
        public string TextAlign { get; set; }


        public string Width { get; set; }


        public List<FormItemOptionOption> Options { get; set; }
    }
}