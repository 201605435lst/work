
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.MasterPlans.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class MasterPlanContentProfile : Profile
	{

		public MasterPlanContentProfile()
		{
			CreateMap<MasterPlanContent, MasterPlanContentDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;

			CreateMap<MasterPlanContentCreateDto, MasterPlanContent>()
				.ForMember(d => d.PlanStartTime, o => o.MapFrom(s => DateTime.Parse(s.StartDate)))
				.ForMember(d => d.PlanEndTime, o => o.MapFrom(s => DateTime.Parse(s.EndDate)))
				.ForMember(d => d.Period, o => o.MapFrom(s => s.Duration))
				;
			CreateMap<MasterPlanContentUpdateDto, MasterPlanContent>() ;
			
			CreateMap<MasterPlanContentGanttUpdateDto, MasterPlanContent>()
				.ForMember(d => d.PlanStartTime, o => o.MapFrom(s => DateTime.Parse(s.StartDate)))
				.ForMember(d => d.PlanEndTime, o => o.MapFrom(s => DateTime.Parse(s.EndDate)))
				.ForMember(d => d.Period, o => o.MapFrom(s => s.Duration))
				;
		}

	}
}
