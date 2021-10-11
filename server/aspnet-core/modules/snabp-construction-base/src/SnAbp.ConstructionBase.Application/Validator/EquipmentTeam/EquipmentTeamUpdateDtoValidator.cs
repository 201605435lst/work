//using FluentValidation;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Extension;

namespace SnAbp.ConstructionBase.Validator.EquipmentTeam
{
	// /// <summary>
	// /// 更新 equipment dto  验证
	// /// </summary>
	// public class EquipmentTeamUpdateDtoValidator : AbstractValidator<EquipmentTeamCreateDto>
	// {
	// 	public EquipmentTeamUpdateDtoValidator()
	// 	{
	// 		RuleFor(x => x.Name)
	// 			.NotNull()
	// 			.NotEmpty()
	// 			.WithName("设备名称")
	// 			.WithMessage("{PropertyName}不能为空!")
	// 			;
	// 		RuleFor(x => x.TypeId)
	// 			.NotNull()
	// 			.NotEmpty()
	// 			.WithName("设备类型")
	// 			.WithMessage("{PropertyName}不能为空!")
	// 			.Must(x => x.GuidNotEmpty())
	// 			.WithMessage("请选择{PropertyName}!")
	// 			;
	// 	}
	// }
}