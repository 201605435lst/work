using System.Linq;
using AutoMapper;
using SnAbp.ConstructionBase.Dtos.RltProcedure;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.Enums;
using SnAbp.Utils.EnumHelper;

namespace SnAbp.ConstructionBase.Profiles
{
	public class SubItemContentProfile : Profile
	{
		public SubItemContentProfile()
		{
			// material 的mapper  后面 考虑分开写
			CreateMap<SubItemContent, SubItemContentDto>()
				.ForMember(d => d.NodeTypeStr, o => o.MapFrom(s => s.NodeType.GetDescription()))
				.ForMember(d => d.Procedures, o => o.MapFrom(s => s.SubItemContentRltProcedures.Select(x=>x.Procedure)))
				;

			CreateMap<SubItemContentCreateDto, SubItemContent>();
			CreateMap<SubItemContentUpdateDto, SubItemContent>();
			
			CreateMap<RltProcedureRltWorker, RltProcedureRltWorkerDto>();
			CreateMap<RltProcedureRltEquipmentTeam, RltProcedureRltEquipmentTeamDto>();
			CreateMap<RltProcedureRltMaterial, RltProcedureRltMaterialDto>();
			CreateMap<RltProcedureRltFile, RltProcedureRltFileDto>();
			
			CreateMap<Procedure, SubItemContentDto>() // 这个映射方便 树转换 
				.ForMember(d => d.NodeType,    o => o.MapFrom(s => SubItemNodeType.Procedure))
				.ForMember(d => d.NodeTypeStr, o => o.MapFrom(s => SubItemNodeType.Procedure.GetDescription()))
				; 


			CreateMap<SubItemContentRltProcedure, SubItemContentRltProcedureDto>();
			
			
		}
	}
}