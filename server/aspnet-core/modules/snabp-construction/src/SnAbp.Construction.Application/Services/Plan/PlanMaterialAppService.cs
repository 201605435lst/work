using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using SnAbp.Construction.Dtos.PlanMaterial;
using SnAbp.Construction.IServices;
using SnAbp.Construction.Plans;
using SnAbp.Resource.Entities;
using SnAbp.StdBasic.Entities;
using SnAbp.Technology.Dtos;
using SnAbp.Technology.IServices;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SnAbp.Construction.Services.Plan
{

	/// <summary>
	/// 施工计划工程量 Service 
	/// </summary>
	// [Authorize]
	public class PlanMaterialAppService : CrudAppService<
			PlanMaterial, PlanMaterialDto, Guid, PlanMaterialSearchDto, PlanMaterialCreateDto,
			PlanMaterialUpdateDto>,
		IPlanMaterialAppService
	{
		private readonly IRepository<PlanContent, Guid> _planContentRepo;
		private readonly IRepository<Equipment, Guid> _equipmentRepo;
		private readonly IRepository<ComponentCategory, Guid> _componentCateRepo;
		private readonly IRepository<ComponentCategoryRltQuota, Guid> _componentCateRltQuotaRepo;
		private readonly IRepository<QuotaItem, Guid> _quotaItemRepo;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IQuantityManagerAppService _technologyQuantityAppService;

		public PlanMaterialAppService(
			IRepository<PlanMaterial, Guid> repository,
			IRepository<PlanContent, Guid> planContentRepo,
			IRepository<Equipment, Guid> equipmentRepo,
			IRepository<ComponentCategory, Guid> componentCateRepo,
			IRepository<ComponentCategoryRltQuota, Guid> componentCateRltQuotaRepo,
			IRepository<QuotaItem, Guid> quotaItemRepo,
			IUnitOfWorkManager unitOfWorkManager,
			IQuantityManagerAppService technologyQuantityAppService
			) : base(repository)
		{
			_planContentRepo = planContentRepo;
			_equipmentRepo = equipmentRepo;
			_componentCateRepo = componentCateRepo;
			_componentCateRltQuotaRepo = componentCateRltQuotaRepo;
			_quotaItemRepo = quotaItemRepo;
			_unitOfWorkManager = unitOfWorkManager;
			_technologyQuantityAppService = technologyQuantityAppService;
			repository.GetListAsync();
		}

		/// <summary>
		/// 更新前的数据验证
		/// </summary>
		protected override void MapToEntity(PlanMaterialUpdateDto updateInput, PlanMaterial entity)
		{
			Console.WriteLine("更新前验证数据");
			base.MapToEntity(updateInput, entity);
		}
		/// <summary>
		/// 创建前的数据验证
		/// </summary>
		protected override PlanMaterial MapToEntity(PlanMaterialCreateDto updateInput)
		{
			Console.WriteLine("创建前验证数据");
			return base.MapToEntity(updateInput);
		}

		/// <summary>
		/// 在 getList 方法 前 构造 query.where 重写
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override IQueryable<PlanMaterial> CreateFilteredQuery(PlanMaterialSearchDto input)
		{
			IQueryable<PlanMaterial> query = Repository.WithDetails(); // include 关联查询,在 对应的 EntityFrameworkCoreModule.cs 文件里面 设置 include 

			// 这里自己手动 写吧,实在是拼不动了…… 
			query = query
				.WhereIf(input.PlanContentId.HasValue, x => x.PlanContentId == input.PlanContentId); // 根据 施工计划详情id 查询 

			return query;
		}
		/// <summary>
		/// 删除多个 施工计划工程量
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		public async Task<bool> DeleteRange(List<Guid> ids)
		{
			await Repository.DeleteAsync(x => ids.Contains(x.Id));
			return true;
		}

		/// <summary>
		/// 给 施工计划详情 设置 工程量
		/// </summary>
		/// <param name="ids"></param>
		/// <param name="planContentId"></param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		[UnitOfWork]
		public async Task<bool> SetPlanMaterial(Guid planContentId, List<Guid> ids)
		{
			//  创建一个新的非事务性UOW作用域
			// using var unitWork = _unitOfWorkManager.Begin(true, false);

			await Repository.DeleteAsync(x => x.PlanContentId == planContentId, true); // 先全删了
																					   // 在慢慢添加 
			List<Equipment> equipments = _equipmentRepo.Where(x => ids.Contains(x.Id)).ToList();
			List<QuantitiesDto> quantitiesDtos = await _technologyQuantityAppService.GetAllList();
			//根据 组件分类 分下组
			var equipmentGroup = equipments.GroupBy(x => x.ComponentCategoryId).ToList();
			foreach (var x in equipmentGroup)
			{
				Guid? componentCateId = x.Key; //分类的id 
				QuantitiesDto quantitiesDto = quantitiesDtos.FirstOrDefault(q => q.Id == componentCateId);
				string spec = quantitiesDto == null ? "无规格" : quantitiesDto.Name;
				ComponentCategory componentCategory =
					_componentCateRepo.FirstOrDefault(c => c.Id == componentCateId);
				string unit = componentCategory is { Unit: null } ? "无单位" : componentCategory?.Unit;
				decimal? quantity = x.Sum(e => e.Quantity);
				List<Guid> quotaIds = _componentCateRltQuotaRepo.WithDetails(c => c.Quota)
					.Where(c => c.ComponentCategoryId == componentCateId).Select(s => s.QuotaId).ToList();
				if (quotaIds.Count == 0) throw new UserFriendlyException($"构件分类id={componentCateId}未找到定额!");
				decimal totalDay =
					_quotaItemRepo.Where(q => quotaIds.Contains(q.QuotaId)).Sum(s => s.Number); // 一个构件分类下的定额列表 工日相加
				decimal workDay = 0;
				if (quantity != null) workDay = quantity.Value * totalDay; //工日 = 设备数量 * 对应定额总工日

				Guid planMaterialId = GuidGenerator.Create();
				await Repository.InsertAsync(new PlanMaterial(planMaterialId)
				{
					PlanContentId = planContentId,
					ComponentCategoryName = componentCategory == null ? "无构件名称" : componentCategory.Name,
					Spec = spec,
					Unit = unit,
					Quantity = quantity ?? 0,
					WorkDay = workDay,
					PlanMaterialRltEquipments = x.Select(y => new PlanMaterialRltEquipment(GuidGenerator.Create())
					{
						EquipmentId = y.Id,
						PlanMaterialId = planMaterialId
					}).ToList()
				}, true);
			}

			decimal sumWorkDay = Repository.Where(x => x.PlanContentId == planContentId).Sum(x => x.WorkDay); // 将 任务计划的工程量的总工日相加
			PlanContent planContent = await _planContentRepo.GetAsync(planContentId); // 获取施工计划详情
																					  // 人工 = 工日/ 工期 
			planContent.WorkerNumber = (int)(sumWorkDay / (decimal)planContent.Period);
			planContent.WorkDay = sumWorkDay;
			await _planContentRepo.UpdateAsync(planContent, true);
			// await unitWork.SaveChangesAsync();
			return true;
		}

	}
}
