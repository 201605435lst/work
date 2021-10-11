using AutoMapper;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Dtos.Material;
using SnAbp.ConstructionBase.Dtos.Worker;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.Enums;
using SnAbp.Utils.EnumHelper;

namespace SnAbp.ConstructionBase
{
	public class ConstructionBaseApplicationAutoMapperProfile : Profile
	{
		public ConstructionBaseApplicationAutoMapperProfile()
		{
			/* You can configure your AutoMapper mapping configuration here.
			 * Alternatively, you can split your mapping configurations
			 * into multiple profile classes for a better organization. */
			// worker 的mapper 
			CreateMap<Worker, ConstructionBaseWorkerDto>();
			CreateMap<ConstructionBaseWorkerDto, Worker>();
			CreateMap<ConstructionBaseWorkerCreateDto, Worker>();
			CreateMap<ConstructionBaseWorkerUpdateDto, Worker>();
			// equipmentTeam 的mapper  后面 考虑分开写
			CreateMap<EquipmentTeam, EquipmentTeamDto>()
				.ForMember(d => d.Type, o => o.MapFrom(s => s.Type.Name)
				);

			CreateMap<EquipmentTeamCreateDto, EquipmentTeam>();
			CreateMap<EquipmentTeamUpdateDto, EquipmentTeam>();

			// material 的mapper  后面 考虑分开写
			CreateMap<ConstructionBaseMaterial, ConstructionBaseMaterialDto>()
				.ForMember(d => d.UnitStr, o => o.MapFrom(s => s.Unit.GetDescription()))
				;
			CreateMap<ConstructionBaseMaterialCreateDto, ConstructionBaseMaterial>();
			CreateMap<ConstructionBaseMaterialUpdateDto, ConstructionBaseMaterial>();
		}
	}
}