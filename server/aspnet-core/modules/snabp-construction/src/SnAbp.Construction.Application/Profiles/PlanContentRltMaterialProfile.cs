
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.Plan.PlanContentRltMaterial;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanContentRltMaterialProfile : Profile
	{

		public PlanContentRltMaterialProfile()
		{
			CreateMap<PlanContentRltMaterial, PlanContentRltMaterialDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;


		}

	}
}
