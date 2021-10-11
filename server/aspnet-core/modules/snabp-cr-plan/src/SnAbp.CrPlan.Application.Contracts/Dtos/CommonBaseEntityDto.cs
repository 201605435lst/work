using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    /// <summary>
    /// 添加了通用字段的Dto基类
    /// </summary>
    public class CommonBaseEntityDto : EntityDto<Guid>
    {
        public string RepairTag { get; set; }
    }
}
