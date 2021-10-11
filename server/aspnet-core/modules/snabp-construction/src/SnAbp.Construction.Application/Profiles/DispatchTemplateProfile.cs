
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Construction.Dtos;
using SnAbp.Construction.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class DispatchTemplateProfile : Profile
	{

		public DispatchTemplateProfile()
		{
			CreateMap<DispatchTemplate, DispatchTemplateDto>();
			CreateMap<DispatchTemplateCreateDto, DispatchTemplate>();
			CreateMap<DispatchTemplateUpdateDto, DispatchTemplate>();
			CreateMap<DispatchTemplate, DispatchTemplateUpdateDto>();
		}

	}
}
