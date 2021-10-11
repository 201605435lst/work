
using System;
using AutoMapper;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.Construction.Plans;

namespace SnAbp.Construction.Profiles
{
    public class PlanContentProfile : Profile
	{

		public PlanContentProfile()
		{
			CreateMap<PlanContent, PlanContentDto>()
				.ForMember(d => d.WorkerNumber, o => o.MapFrom(s => s.WorkerNumber == 0 ? 1 : s.WorkerNumber))  // 工日为0 的话 自动转换成1 
				;

			CreateMap<PlanContentCreateDto, PlanContent>()
				.ForMember(d => d.PlanStartTime, o => o.MapFrom(s => DateTime.Parse(s.StartDate)))
				.ForMember(d => d.PlanEndTime, o => o.MapFrom(s => DateTime.Parse(s.EndDate)))
				.ForMember(d => d.Period, o => o.MapFrom(s => s.Duration))
				.ForMember(d => d.WorkerNumber, o => o.MapFrom(s => s.WorkerNumber == 0 ? 0 : s.WorkerNumber))
				;
			CreateMap<PlanContentUpdateDto, PlanContent>();
			CreateMap<PlanContentGanttUpdateDto, PlanContent>()
				.ForMember(d => d.PlanStartTime, o => o.MapFrom(s => DateTime.Parse(s.StartDate)))
				.ForMember(d => d.PlanEndTime, o => o.MapFrom(s => DateTime.Parse(s.EndDate)))
				.ForMember(d => d.Period, o => o.MapFrom(s => s.Duration))
				.ForMember(d => d.WorkerNumber, o => o.MapFrom(s => s.WorkerNumber == 0 ? 0 : s.WorkerNumber))
				;
			
			CreateMap<MasterPlanContentDto, PlanContent>()
				.ForMember(x => x.Children, opt => opt.Ignore()) //忽略 children 属性,不然报错(而且我也不需要children 属性 )
				.ForMember(x => x.SubItemContent, opt => opt.Ignore()) //忽略 children 属性,不然报错(而且我也不需要children 属性 )
				.ForMember(x => x.Antecedents, opt => opt.Ignore()) //忽略 children 属性,不然报错(而且我也不需要children 属性 )
				.ForMember(x => x.PlanMaterials, opt => opt.Ignore()) //忽略 children 属性,不然报错(而且我也不需要children 属性 )
				.ForMember(x => x.Files, opt => opt.Ignore()) //忽略 children 属性,不然报错(而且我也不需要children 属性 )
				;
 

		}

	}
}
