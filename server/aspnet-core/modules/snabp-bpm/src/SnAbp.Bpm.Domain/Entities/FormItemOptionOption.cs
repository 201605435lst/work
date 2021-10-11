using SnAbp.MultiProject.MultiProject;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{

    /// <summary>
    /// 选择型表单项下拉框选项
    /// </summary>
    [NotMapped]
    public class FormItemOptionOption
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}