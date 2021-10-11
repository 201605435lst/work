
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.Plan.PlanContentRltAntecedent;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanContentRltAntecedentProfile : Profile
	{

		public PlanContentRltAntecedentProfile()
		{
			CreateMap<PlanContentRltAntecedent, PlanContentRltAntecedentDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;


		}

	}
}
