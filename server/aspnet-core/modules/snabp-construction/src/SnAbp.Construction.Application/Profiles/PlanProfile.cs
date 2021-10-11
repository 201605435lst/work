
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.Plan;
using SnAbp.Construction.Dtos.Plan.Plan;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanProfile : Profile
	{

		public PlanProfile()
		{
			CreateMap<Plan, PlanDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;

			CreateMap<PlanCreateDto, Plan>();
			CreateMap<PlanUpdateDto, Plan>();
 

		}

	}
}
