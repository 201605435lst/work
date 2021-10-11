
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.PlanMaterial;
using SnAbp.Construction.Entities;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanMaterialProfile : Profile
	{

		public PlanMaterialProfile()
		{
			CreateMap<PlanMaterial, PlanMaterialDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;

			CreateMap<PlanMaterialCreateDto, PlanMaterial>();
			CreateMap<PlanMaterialUpdateDto, PlanMaterial>();
 

		}

	}
}
