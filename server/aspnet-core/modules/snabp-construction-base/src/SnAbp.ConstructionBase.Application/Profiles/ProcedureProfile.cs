using System.Linq;
using AutoMapper;
using SnAbp.ConstructionBase.Dtos.Procedure;
using SnAbp.ConstructionBase.Entities;
using SnAbp.Utils.EnumHelper;

namespace SnAbp.ConstructionBase.Profiles
{
	public class ProcedureProfile : Profile
	{
		public ProcedureProfile()
		{
			// material 的mapper  后面 考虑分开写
			CreateMap<Procedure, ProcedureDto>()
				.ForMember(d => d.Type, o => o.MapFrom(s => s.Type.Name))
				.ForMember(d => d.ProcedureWorkers, o => o.MapFrom(s => s.ProcedureWorkers.Select(x => x.Worker)))
				.ForMember(d => d.ProcedureEquipmentTeams,
					o => o.MapFrom(s => s.ProcedureEquipmentTeams.Select(x => x.EquipmentTeam)))
				.ForMember(d => d.ProcedureMaterials, o => o.MapFrom(s => s.ProcedureMaterials))
				.ForMember(d => d.ProcedureRtlFiles, o => o.MapFrom(s => s.ProcedureRtlFiles.Select(x => x.File)))
				;
			CreateMap<ProcedureCreateDto, Procedure>();
			CreateMap<ProcedureUpdateDto, Procedure>();

			CreateMap<ConstructionBaseMaterial, ProcedureMaterialDto>()
				.ForMember(d => d.UnitStr, o => o.MapFrom(s => s.Unit.GetDescription()))
				;
			CreateMap<ProcedureMaterial, ProcedureMaterialDto>()
				.ForMember(d => d.Id, o => o.MapFrom(s => s.MaterialId))
				.ForMember(d => d.Name, o => o.MapFrom(s => s.ConstructionBaseMaterial.Name))
				.ForMember(d => d.Count, o => o.MapFrom(s => s.Count))
				;
			CreateMap<File.Entities.File, ConstructionBaseFileDto>();
		}
	}
}