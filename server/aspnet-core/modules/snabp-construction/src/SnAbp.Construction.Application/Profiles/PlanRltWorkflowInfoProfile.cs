
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.Plan.PlanRltWorkflowInfo;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanRltWorkflowInfoProfile : Profile
	{

		public PlanRltWorkflowInfoProfile()
		{
			CreateMap<PlanRltWorkflowInfo, PlanRltWorkflowInfoDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;


		}

	}
}
