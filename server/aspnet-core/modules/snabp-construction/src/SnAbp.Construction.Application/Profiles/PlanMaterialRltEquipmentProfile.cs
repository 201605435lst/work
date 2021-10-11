
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.PlanMaterialRltEquipment;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanMaterialRltEquipmentProfile : Profile
	{

		public PlanMaterialRltEquipmentProfile()
		{
			CreateMap<PlanMaterialRltEquipment, PlanMaterialRltEquipmentDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;


		}

	}
}
