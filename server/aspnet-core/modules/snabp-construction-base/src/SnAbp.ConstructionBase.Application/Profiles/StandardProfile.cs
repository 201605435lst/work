
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.ConstructionBase.Dtos.Standard;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Profiles
{
	public class StandardProfile : Profile
	{

		public StandardProfile()
		{
			CreateMap<Standard, StandardDto>()
				.ForMember(d => d.Profession, o => o.MapFrom(s => s.Profession.Name))
				;

			CreateMap<StandardCreateDto, Standard>();
			CreateMap<StandardUpdateDto, Standard>();
		}

	}
}
