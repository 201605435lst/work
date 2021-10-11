using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Profiles
{
	public class SubItemProfile : Profile
	{

		public SubItemProfile()
		{
			// material 的mapper  后面 考虑分开写
			CreateMap<SubItem, SubItemDto>()
				.ForMember(d => d.IsDrawUp,    o => o.MapFrom(s => s.SubItemContent != null))
				.ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator == null ? "无" : s.Creator.Name))
				.ForMember(d => d.CreateTime,  o => o.MapFrom(s => s.CreationTime.ToString("yyyy-MM-dd")))
				;

			CreateMap<SubItemCreateDto, SubItem>();
			CreateMap<SubItemUpdateDto, SubItem>();
		}

	}
}