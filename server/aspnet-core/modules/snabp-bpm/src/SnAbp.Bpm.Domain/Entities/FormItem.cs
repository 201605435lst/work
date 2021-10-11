using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{
    [NotMapped]
    public class FormItem
    {
        public string Key { get; set; }
        public string Model { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public FormItemOption options { get; set; }
    }
}
