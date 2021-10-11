
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos.Plan.PlanContentRltFile;
using SnAbp.Construction.Plans;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class PlanContentRltFileProfile : Profile
	{

		public PlanContentRltFileProfile()
		{
			CreateMap<PlanContentRltFile, PlanContentRltFileDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;


		}

	}
}
