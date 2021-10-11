using System;
using System.Linq;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.Identity;

namespace SnAbp.ConstructionBase.Validator.EquipmentTeam
{
	// /// <summary>
	// /// 创建 equipment dto  验证
	// /// </summary>
	// public class EquipmentTeamCreateDtoValidator : AbstractValidator<EquipmentTeamCreateDto>
	// {
	// 	// private readonly IDataDictionaryRepository _dataDictionaryRepository;
	//
	// 	// public EquipmentTeamCreateDtoValidator(IDataDictionaryRepository dataDictionaryRepository)
	// 	public EquipmentTeamCreateDtoValidator(IDataDictionaryRepository dataDictionaryRepository)
	// 	{
	// 		// _dataDictionaryRepository = dataDictionaryRepository;
	// 		RuleFor(x => x.Name).Length(1, 2);
	// 		
	// 		// 	.NotNull()
	// 		// 	.NotEmpty()
	// 		// 	.WithName("设备名称")
	// 		// 	.WithMessage("{PropertyName}不能为空!")
	// 		// 	;
	// 		// RuleFor(x => x.TypeId)
	// 		// 	.NotNull()
	// 		// 	.NotEmpty()
	// 		// 	.WithName("设备类型")
	// 		// 	.WithMessage("{PropertyName}不能为空!")
	// 		// 	.Must(NotContainsEquipmentType)
	// 		// 	.WithMessage("请选择{PropertyName}!")
	// 		// 	;
	// 		// RuleFor(x => x.Cost)
	// 		// 	.NotNull()
	// 		// 	.NotEmpty()
	// 		// 	.WithName("设备成本")
	// 		// 	.Must(NotZero)
	// 		// 	.WithMessage("{PropertyName}不能为0!")
	// 		// 	;
	// 	}
	//
	// 	private bool NotZero(double x)
	// 	{ 
	// 		return x == 0;
	// 	}
	//
	// 	// private bool NotContainsEquipmentType(Guid id)
	// 	// {
	// 	// 	return !_dataDictionaryRepository.WithDetails().Where(x => x.Key.Contains("Progress.EquipmentType."))
	// 	// 		.Select(x => x.Id).Contains(id);
	// 	// }
	// }
}