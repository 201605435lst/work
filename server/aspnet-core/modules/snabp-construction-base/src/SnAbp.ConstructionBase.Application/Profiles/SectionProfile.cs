
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.ConstructionBase.Dtos.Section;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Profiles
{
	public class SectionProfile : Profile
	{

		public SectionProfile()
		{
			CreateMap<Section, SectionDto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;

			CreateMap<SectionCreateDto, Section>();
			CreateMap<SectionUpdateDto, Section>();
		}

	}
}
