
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanRltContentRltAntecedent;
using SnAbp.Construction.MasterPlans.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class MasterPlanRltContentRltAntecedentProfile : Profile
	{

		public MasterPlanRltContentRltAntecedentProfile()
		{
			CreateMap<MasterPlanRltContentRltAntecedent, MasterPlanRltContentRltAntecedentDto>()
				.ForMember(d => d.Name, o => o.MapFrom(s => s.FrontMasterPlanContent.Name))
				;


		}

	}
}
