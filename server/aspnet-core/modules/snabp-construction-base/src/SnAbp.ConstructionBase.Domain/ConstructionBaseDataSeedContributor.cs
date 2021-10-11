using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.Enums;
using SnAbp.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.ConstructionBase
{
	/// <summary>
	/// 种子数据 
	/// </summary>
	public class ConstructionBaseDataSeedContributor : IDataSeedContributor, ITransientDependency
	{
		// private readonly IRepository<Worker       , Guid> _workerRepo;
		// private readonly IRepository<EquipmentTeam, Guid> _equipmentTeamRepo;
		// private readonly IRepository<Material     , Guid> _materialRepo;
		// private readonly IRepository<Procedure    , Guid> _procedureRepo;
		// private readonly IGuidGenerator            _guidGenerator;
		// private readonly IDataDictionaryRepository _dataDictionaryRepository;

		public ConstructionBaseDataSeedContributor(
			// IDataDictionaryRepository dataDictionaryRepository,
			// IGuidGenerator guidGenerator,
			// IRepository<Worker       , Guid> workerRepo,
			// IRepository<EquipmentTeam, Guid> equipmentTeamRepo,
			// IRepository<Material     , Guid> materialRepo,
			// IRepository<Procedure    , Guid> procedureRepo
			
		)
		{
			// _workerRepo               = workerRepo;
			// _equipmentTeamRepo        = equipmentTeamRepo;
			// _materialRepo             = materialRepo;
			// _procedureRepo            = procedureRepo;
			// _guidGenerator            = guidGenerator;
			// _dataDictionaryRepository = dataDictionaryRepository;
		}

		[UnitOfWork]
		public async Task SeedAsync(DataSeedContext context)
		{
			// 注释 种子数据 ,别的同事电脑 ide 报错 了 …… 
			// // 添加 设备类型数据字典 Progress.EquipmentType 
			// if (_dataDictionaryRepository.Count(x => x.Key.Contains("Progress.EquipmentType")) <= 0)
			// {
			// 	Guid parentId = Guid.NewGuid();
			// 	Guid waJue    = Guid.NewGuid();
			// 	Guid tianWa   = Guid.NewGuid();
			// 	Guid yunShu   = Guid.NewGuid();
			// 	Guid qiDiao   = Guid.NewGuid();
			// 	if (new List<DataDictionary>()
			// 	{
			// 		new DataDictionary(parentId)
			// 		{
			// 			Name = "设备类型",
			// 			Key = "Progress.EquipmentType",
			// 			IsStatic = true,
			// 			Children = new List<DataDictionary>()
			// 			{
			// 				new DataDictionary(waJue) { Name  = "挖掘设备", Key = "Progress.EquipmentType.Dig"    , IsStatic = true, ParentId = parentId },
			// 				new DataDictionary(tianWa) { Name = "填挖设备", Key = "Progress.EquipmentType.FillDig", IsStatic = true, ParentId = parentId },
			// 				new DataDictionary(yunShu) { Name = "运输设备", Key = "Progress.EquipmentType.Traffic", IsStatic = true, ParentId = parentId },
			// 				new DataDictionary(qiDiao) { Name = "起吊设备", Key = "Progress.EquipmentType.Hoist"  , IsStatic = true, ParentId = parentId },
			// 			}
			// 		},
			// 	}.Any())
			// 	{
			// 		foreach (var item in new List<DataDictionary>()
			// 		{
			// 			new DataDictionary(parentId)
			// 			{
			// 				Name = "设备类型",
			// 				Key = "Progress.EquipmentType",
			// 				IsStatic = true,
			// 				Children = new List<DataDictionary>()
			// 				{
			// 					new DataDictionary(waJue) { Name  = "挖掘设备", Key = "Progress.EquipmentType.Dig"    , IsStatic = true, ParentId = parentId },
			// 					new DataDictionary(tianWa) { Name = "填挖设备", Key = "Progress.EquipmentType.FillDig", IsStatic = true, ParentId = parentId },
			// 					new DataDictionary(yunShu) { Name = "运输设备", Key = "Progress.EquipmentType.Traffic", IsStatic = true, ParentId = parentId },
			// 					new DataDictionary(qiDiao) { Name = "起吊设备", Key = "Progress.EquipmentType.Hoist"  , IsStatic = true, ParentId = parentId },
			// 				}
			// 			},
			// 		})
			// 		{
			// 			await _dataDictionaryRepository.InsertAsync(item);
			// 		}
			//
			// 		// 设备台班种子数据 
			// 		if (await _equipmentTeamRepo.GetCountAsync() <= 0)
			// 		{
			// 			await _equipmentTeamRepo.InsertAsync(new EquipmentTeam(_guidGenerator.Create()) { Name = "装载机"   , Spec = "Ca4101", Cost = 20.00, TypeId = waJue });
			// 			await _equipmentTeamRepo.InsertAsync(new EquipmentTeam(_guidGenerator.Create()) { Name = "混凝土搅拌机", Spec = "Ca4101", Cost = 20.00, TypeId = tianWa });
			// 			await _equipmentTeamRepo.InsertAsync(new EquipmentTeam(_guidGenerator.Create()) { Name = "推土机"   , Spec = "Ca4101", Cost = 20.00, TypeId = yunShu });
			// 			await _equipmentTeamRepo.InsertAsync(new EquipmentTeam(_guidGenerator.Create()) { Name = "插入式震动棒", Spec = "Ca4101", Cost = 20.00, TypeId = qiDiao });
			// 		}
			// 	}
			// }
			//
			//
			// // 添加 工程类型数据字典 Progress.ProjectType
			// if (_dataDictionaryRepository.Count(x => x.Key.Contains("Progress.ProjectType")) <= 0)
			// {
			// 	// 建筑工程、市政工程、公路工程  类型 id 
			// 	Guid parentId = Guid.NewGuid();
			// 	DataDictionary projectTypeDic = new DataDictionary(parentId)
			// 	{
			// 		Name = "工程类型", Key = "Progress.ProjectType", IsStatic = true, Children = new List<DataDictionary>()
			// 		{
			// 			new DataDictionary(_guidGenerator.Create()) {Name = "建筑工程", Key = "Progress.ProjectType.Build", IsStatic = true, ParentId = parentId},
			// 			new DataDictionary(_guidGenerator.Create()) {Name = "市政工程", Key = "Progress.ProjectType.City" , IsStatic = true, ParentId = parentId},
			// 			new DataDictionary(_guidGenerator.Create()) {Name = "公路工程", Key = "Progress.ProjectType.Road" , IsStatic = true, ParentId = parentId},
			// 		}
			// 	};
			// 	await _dataDictionaryRepository.InsertAsync(projectTypeDic);
			// }
			//
			// // 插入 工种信息 种子数据 
			// if (await _workerRepo.GetCountAsync() <= 0)
			// {
			// 	await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "电工"});
			// 	await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "混凝土工"});
			// 	await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "钳工"});
			// 	await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "木工"});
			// }
			//
			// // 插入 工种信息 种子数据 
			// if (await _materialRepo.GetCountAsync() <= 0)
			// {
			// 	await _materialRepo.InsertAsync(new Material(_guidGenerator.Create()) { Code = "001", Name = "SBS改性理清防水", Unit = MaterialEnum.Square    , IsSelf = false, IsPartyAProvide = true , PresentDays = 0 , PrePurchaseDays = 0 }); 
			// 	await _materialRepo.InsertAsync(new Material(_guidGenerator.Create()) { Code = "002", Name = "标砖"       , Unit = MaterialEnum.A         , IsSelf = true , IsPartyAProvide = false, PresentDays = 10, PrePurchaseDays = 10 }); 
			// 	await _materialRepo.InsertAsync(new Material(_guidGenerator.Create()) { Code = "003", Name = "空心砖"      , Unit = MaterialEnum.A         , IsSelf = true , IsPartyAProvide = false, PresentDays = 5 , PrePurchaseDays = 5 }); 
			// 	await _materialRepo.InsertAsync(new Material(_guidGenerator.Create()) { Code = "004", Name = "挖基础土方"    , Unit = MaterialEnum.CubicMeter, IsSelf = false, IsPartyAProvide = true , PresentDays = 0 , PrePurchaseDays = 0 });
			// }
		}
	}
}