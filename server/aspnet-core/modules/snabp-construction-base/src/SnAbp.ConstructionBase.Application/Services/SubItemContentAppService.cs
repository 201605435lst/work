using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Dtos.Material;
using SnAbp.ConstructionBase.Dtos.Procedure;
using SnAbp.ConstructionBase.Dtos.RltProcedure;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.ConstructionBase.Dtos.Worker;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.Enums;
using SnAbp.ConstructionBase.IServices;
using SnAbp.Utils.TreeHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Services
{
	// [Authorize]
	public class SubItemContentAppService :
		CrudAppService<SubItemContent, SubItemContentDto, Guid, SubItemContentSearchDto, SubItemContentCreateDto,
			SubItemContentUpdateDto>,
		ISubItemContentAppService
	{
		private readonly IRepository<SubItemContentRltProcedure, Guid> _subItemContentRltProcedureRepo;
		private readonly IRepository<Procedure, Guid> _procedureRepo;
		private readonly IRepository<ProcedureMaterial, Guid> _procedureRltMatRepo;
		private readonly IRepository<ProcedureWorker, Guid> _procedureRltWorkerRepo;
		private readonly IRepository<ConstructionBaseMaterial, Guid> _matRepo;
		private readonly IRepository<EquipmentTeam, Guid> _equipRepo;
		private readonly IRepository<Worker, Guid> _workerRepo;
		private readonly IRepository<ProcedureRltFile, Guid> _procedureRltFileRepo;
		private readonly IRepository<ProcedureEquipmentTeam, Guid> _procedureRltEquipRepo;
		private readonly IRepository<RltProcedureRltWorker, Guid> _rltProcedureRltWorkerRepo;
		private readonly IRepository<RltProcedureRltEquipmentTeam, Guid> _rltProcedureRltEquipmentTeamRepo;
		private readonly IRepository<RltProcedureRltMaterial, Guid> _rltProcedureRltMaterialRepo;
		private readonly IRepository<RltProcedureRltFile, Guid> _rltProcedureRltFileRepo;
		private readonly IRepository<SubItem, Guid> _subItemRepos;

		public SubItemContentAppService(
			IRepository<SubItemContent, Guid> repository,
			IRepository<SubItemContentRltProcedure, Guid> subItemContentRltProcedureRepo,
			IRepository<Procedure, Guid> procedureRepo,
			IRepository<ProcedureMaterial, Guid> procedureRltMatRepo,
			IRepository<ProcedureWorker, Guid> procedureRltWorkerRepo,
			IRepository<ConstructionBaseMaterial, Guid> matRepo,
			IRepository<EquipmentTeam, Guid> equipRepo,
			IRepository<Worker, Guid> workerRepo,
			IRepository<ProcedureRltFile, Guid> procedureRltFileRepo,
			IRepository<ProcedureEquipmentTeam, Guid> procedureRltEquipRepo,
			IRepository<RltProcedureRltWorker, Guid> rltProcedureRltWorkerRepo,
			IRepository<RltProcedureRltEquipmentTeam, Guid> rltProcedureRltEquipmentTeamRepo,
			IRepository<RltProcedureRltMaterial, Guid> rltProcedureRltMaterialRepo,
			IRepository<RltProcedureRltFile, Guid> rltProcedureRltFileRepo,
			IRepository<SubItem, Guid> subItemRepos
			) : base(repository)
		{
			_subItemContentRltProcedureRepo = subItemContentRltProcedureRepo;
			_procedureRepo = procedureRepo;
			_procedureRltMatRepo = procedureRltMatRepo;
			_procedureRltWorkerRepo = procedureRltWorkerRepo;
			_matRepo = matRepo;
			_equipRepo = equipRepo;
			_workerRepo = workerRepo;
			_procedureRltFileRepo = procedureRltFileRepo;
			_procedureRltEquipRepo = procedureRltEquipRepo;
			_rltProcedureRltWorkerRepo = rltProcedureRltWorkerRepo;
			_rltProcedureRltEquipmentTeamRepo = rltProcedureRltEquipmentTeamRepo;
			_rltProcedureRltMaterialRepo = rltProcedureRltMaterialRepo;
			_rltProcedureRltFileRepo = rltProcedureRltFileRepo;
			_subItemRepos = subItemRepos;
		}

		/// <summary>
		/// 重写添加单个
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override async Task<SubItemContentDto> CreateAsync(SubItemContentCreateDto input)
		{
			if (input.Name.IsNullOrWhiteSpace()) throw new UserFriendlyException("分布分项名称不能为空!");
			if ((int)input.NodeType==0) throw new UserFriendlyException("请选择正确的节点类型!");
			if (input.ParentId == null || input.ParentId.Value == Guid.Empty) throw new UserFriendlyException("请输入父id!");

			var inputParentId = input.ParentId;
			// 找到所有 同级 的 content  的数量
			int count                     = Repository.Count(x => x.ParentId== inputParentId);
			SubItemContent subItemContent = ObjectMapper.Map<SubItemContentCreateDto     , SubItemContent>(input);
			subItemContent.Order          = count + 1;
			SubItemContent insertAsync    = await Repository.InsertAsync(subItemContent);
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(insertAsync);
		}

		
		public async Task<SubItemContentDto> InitContent(Guid subItemId)
		{
			SubItem subItem = await _subItemRepos.GetAsync(subItemId);
			if (subItem.SubItemContent != null) throw new UserFriendlyException($"错误,{subItem.Name} 已被编制过,不能调用 app/api/subitemContent/initContent 方法!");
			SubItemContent subItemContent = await Repository.InsertAsync(new SubItemContent(GuidGenerator.Create()) {Name = subItem.Name,  SubItemId =  subItem.Id, NodeType =  SubItemNodeType.All,Order = 1});
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(subItemContent);
		}


		/// <summary>
		/// 根据 contentId 获取 单个 content 树
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public  SubItemContentDto GetSingleTree(Guid id)
		{
			List<SubItemContent> allList = Repository.WithDetails().ToList();
			List<SubItemContent> masterPlanContents = allList.Where(x => x.Id == id).ToList();
			SubItemContent subItemContent = masterPlanContents.FirstOrDefault();

			if (subItemContent!=null)
			{
				// 定义 个递归排序函数 
				List<SubItemContent> RecOrder(List<SubItemContent> list)
				{
					List<SubItemContent> newList = list.OrderBy(x => x.Order).ToList();
					newList.ForEach(x => { x.Children = RecOrder(x.Children); });
					return newList;
				}

				subItemContent.Children = RecOrder(subItemContent.Children);
			}
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(subItemContent);
		}

		/// <summary>
		/// 根据 subItemId 或取 单个 content 树
		/// </summary>
		/// <param name="subItemId"></param>
		/// <returns></returns>
		public async Task<SubItemContentDto> GetSingleTreeBySubItemId(Guid subItemId)
		{
			if (_subItemRepos.Any(x=>x.Id==subItemId)) throw new UserFriendlyException($"subItem不存在! subItemId={subItemId}");
			SubItemContent subItemContent = Repository.WithDetails().FirstOrDefault(x => x.SubItemId == subItemId);
			if (subItemContent==null) return new SubItemContentDto();
			SubItemContentDto singleTree =  GetSingleTree(subItemContent.Id);
			return singleTree;
		}

		public Task<SubItemContentDto> GetSingleTreeWithProcedure(Guid id)
		{
			SubItemContentDto subItemContentDto = GetSingleTree(id);
			// 递归下,把 nodeType 是 NodeType.WorkSur 的 找到先~
			SubItemContentDto workSurContent = GetWorkSurContent(subItemContentDto);
			return Task.FromResult(workSurContent);
		}

		public Task<List<SubItemContentRltProcedureDto>> GetRltProceduresByContentId(Guid contentId)
		{
			List<SubItemContentRltProcedure> subItemContentRltProcedures = _subItemContentRltProcedureRepo.WithDetails().Where(x => x.SubItemContentId == contentId).OrderBy(x=>x.Sort).ToList();
			return Task.FromResult(ObjectMapper.Map<List<SubItemContentRltProcedure>,List<SubItemContentRltProcedureDto>>(subItemContentRltProcedures)); 
		}

		private SubItemContentDto GetWorkSurContent(SubItemContentDto content)
		{
			if (content.NodeType==SubItemNodeType.WorkSur)
			{
				List<SubItemContentDto> procedures = _subItemContentRltProcedureRepo.WithDetails()
					.Where(x=>x.SubItemContentId==content.Id)
					.OrderBy(x=>x.Sort)
					// 这里返回 的 是 关联表的 主键id ,名称 是 工序名
					.Select(x=>new SubItemContentDto(){Id = x.Id,Name = x.Procedure.Name,NodeType = SubItemNodeType.Procedure})
					.ToList();
				content.Children = procedures;
				return content;
			}
			content.Children.ForEach(x => GetWorkSurContent(x));
			return content;
		}

		/// <summary>
		/// 根据 rltProcedureId  删除 关联工序表
		/// </summary>
		/// <returns></returns>
		public async Task<bool> DeleteRltProcedure(Guid rltProcedureId)
		{
			await _subItemContentRltProcedureRepo.DeleteAsync(rltProcedureId);
			// 关联工序-工种 关联 查到在删
			_rltProcedureRltWorkerRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltWorkerRepo.DeleteAsync(x));
			// 关联工序-设备 关联 查到在删
			_rltProcedureRltEquipmentTeamRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltEquipmentTeamRepo.DeleteAsync(x));
			// 关联工序-工程量 关联 查到在删
			_rltProcedureRltMaterialRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltMaterialRepo.DeleteAsync(x));
			// 关联工序-文件 关联 查到在删
			_rltProcedureRltFileRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltFileRepo.DeleteAsync(x));
			return true;
		}

		/// <summary>
		/// 重写获取 列表 (获取多个树)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override Task<PagedResultDto<SubItemContentDto>> GetListAsync(SubItemContentSearchDto input)
		{
			// List<SubItemContent> subItemContents = Repository.WithDetails().Where(x=>x.NodeType==NodeType.All).ToList();
			List<SubItemContent> subItemContents = GetSubItemContents();
			// 把 别的儿子 孙子 过滤掉，只留下 爷爷 (顶级节点) ,如果 在 query where 过滤 的话 ,就查不到 孙子了…… 还不知道为啥 ……
			// 所以先 全查出来 在过滤 
			// List<SubItemContent> itemContents = subItemContents.Where(x => x.NodeType == NodeType.All).ToList();
			List<SubItemContentDto> subItemContentDtos = ObjectMapper.Map<List<SubItemContent>,List<SubItemContentDto>>(subItemContents);
			List<SubItemContentDto> dto = GuidKeyTreeHelper<SubItemContentDto>.GetTree(subItemContentDtos);
			return Task.FromResult(new PagedResultDto<SubItemContentDto>(1, dto));
			// return Task.FromResult(new PagedResultDto<SubItemContentDto>(1,
			// 	ObjectMapper.Map<List<SubItemContent>, List<SubItemContentDto>>(itemContents)));
		}

		// public async Task<SubItemContentDto> AddProcedure(Guid id, List<Guid> procedureIds)
		// {
		// 	int count = _procedureRepo.AsQueryable().Count(x => procedureIds.Contains(x.Id));
		// 	if (count!=procedureIds.Count) throw new UserFriendlyException("传入的工序id列表部分id在工序表中找不到!");
		// 	SubItemContent subItemContent = await Repository.GetAsync(id);
		// 	if (subItemContent.NodeType!=NodeType.WorkSur) throw new UserFriendlyException("该分布分项节点不是作业面不能设置工序!");
		// 	subItemContent.SubItemContentRltProcedures = procedureIds
		// 		.Select(( procedureId,i ) =>
		// 		{
		// 			Guid rltProcedureId = GuidGenerator.Create();
		// 			List<ProcedureWorker> workers = _procedureRltWorkerRepo.WithDetails().Where(x=>x.ProcedureId==procedureId).ToList();
		// 			List<ProcedureRltFile> procedureRltFiles = _procedureRltFileRepo.WithDetails().Where(x=>x.ProcedureId==procedureId).ToList();
		// 			List<ProcedureEquipmentTeam> equipmentTeams = _procedureRltEquipRepo.WithDetails().Where(x=>x.ProcedureId==procedureId).ToList();
		// 			List<ProcedureMaterial> materials = _procedureRltMatRepo.WithDetails().Where(x=>x.ProcedureId==procedureId).ToList();
		// 			List<RltProcedureRltWorker> procedureWorkers = workers.Select(x=>new RltProcedureRltWorker(GuidGenerator.Create()){RltProcedureId =rltProcedureId,WorkerId = x.WorkerId}).ToList();
		// 			List<RltProcedureRltFile> procedureFiles = procedureRltFiles.Select(x=>new RltProcedureRltFile(GuidGenerator.Create()){RltProcedureId =rltProcedureId,FileId = x.FileId}).ToList();
		// 			List<RltProcedureRltEquipmentTeam> procedureEquipmentTeams = equipmentTeams.Select(x=>new RltProcedureRltEquipmentTeam(GuidGenerator.Create()){RltProcedureId =rltProcedureId,EquipmentTeamId = x.EquipmentTeamId}).ToList();
		// 			List<RltProcedureRltMaterial> procedureMaterials = materials.Select(x=>new RltProcedureRltMaterial(GuidGenerator.Create()){RltProcedureId =rltProcedureId,MaterialId = x.MaterialId,Count = x.Count}).ToList();
		// 			return new SubItemContentRltProcedure(rltProcedureId)
		// 			{
		// 				ProcedureId = procedureId, Sort = i ,ProcedureWorkers = procedureWorkers,ProcedureRtlFiles = procedureFiles,ProcedureEquipmentTeams = procedureEquipmentTeams,ProcedureMaterials = procedureMaterials
		// 			};
		// 		})
		// 		.ToList();
		// 	SubItemContent updateAsync = await Repository.UpdateAsync(subItemContent);
		// 	return ObjectMapper.Map<SubItemContent, SubItemContentDto>(updateAsync);
		// }
		/// <summary>
		/// TODO 老方法,关联的时候 不太管用 ，到时候 在看看 …… 目前 用的 AddProcedure 方法 来配置 content  关联工序 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="procedureIds"></param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<SubItemContentDto> AddProcedureOld(Guid id, List<Guid> procedureIds)
		{
			int count = _procedureRepo.AsQueryable().Count(x => procedureIds.Contains(x.Id));
			if (count != procedureIds.Count) throw new UserFriendlyException("传入的工序id列表部分id在工序表中找不到!");
			SubItemContent subItemContent = await Repository.GetAsync(id);
			if (subItemContent.NodeType != SubItemNodeType.WorkSur) throw new UserFriendlyException("该分布分项节点不是作业面不能设置工序!");
			subItemContent.SubItemContentRltProcedures = procedureIds
				.Select((x, i) => new SubItemContentRltProcedure(GuidGenerator.Create()) {ProcedureId = x, Sort = i})
				.ToList();
			SubItemContent updateAsync = await Repository.UpdateAsync(subItemContent);
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(updateAsync);
		}
		public async Task<bool> AddProcedure(Guid id, List<Guid> procedureIds)
		{
			// 先验证下 
			int count = _procedureRepo.AsQueryable().Count(x => procedureIds.Contains(x.Id));
			if (count!=procedureIds.Count) throw new UserFriendlyException("传入的工序id列表部分id在工序表中找不到!");
			SubItemContent subItemContent = await Repository.GetAsync(id);
			if (subItemContent.NodeType!=SubItemNodeType.WorkSur) throw new UserFriendlyException("该分布分项节点不是作业面不能设置工序!");
			// rltProcedure  表 先删除在添加 
			List<SubItemContentRltProcedure> subItemContentRltProcedures = _subItemContentRltProcedureRepo.Where(x => x.SubItemContentId == id).ToList();
			subItemContentRltProcedures.ForEach(rltProcedure =>
			{
				// 关联工序-工种 关联 查到在删
				_rltProcedureRltWorkerRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltWorkerRepo.DeleteAsync(x));
				// 关联工序-设备 关联 查到在删
				_rltProcedureRltEquipmentTeamRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltEquipmentTeamRepo.DeleteAsync(x));
				// 关联工序-工程量 关联 查到在删
				_rltProcedureRltMaterialRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltMaterialRepo.DeleteAsync(x));
				// 关联工序-文件 关联 查到在删
				_rltProcedureRltFileRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltFileRepo.DeleteAsync(x));
				
				// 关联 工序 表删除 (这个放到最后面在删除,放到最前面删除会报 更新或操作违反了别的表的外键约束 的 错)
				 _subItemContentRltProcedureRepo.DeleteAsync(rltProcedure);
				
			});
			int i = 0;
			procedureIds.ForEach( procedureId =>
			{
				Guid rltProcedureId = GuidGenerator.Create(); // 定义 rltProcedure 关联工序id 
				// 关联 工序 表 添加  
				_subItemContentRltProcedureRepo.InsertAsync(new SubItemContentRltProcedure(rltProcedureId) {SubItemContentId = id, ProcedureId = procedureId,Sort = i});
				// 查询 工序-xxx 关联表,把里面的数据 转移到 rltProcedureRlt xxx 表中
				_procedureRltMatRepo.Where(x => x.ProcedureId == procedureId).ToList().ForEach(x => _rltProcedureRltMaterialRepo.InsertAsync(new RltProcedureRltMaterial(GuidGenerator.Create()) {RltProcedureId = rltProcedureId,MaterialId = x.MaterialId,Count = x.Count}));
				_procedureRltEquipRepo.Where(x=>x.ProcedureId==procedureId).ToList().ForEach(x=>_rltProcedureRltEquipmentTeamRepo.InsertAsync(new RltProcedureRltEquipmentTeam(GuidGenerator.Create()){RltProcedureId = rltProcedureId,EquipmentTeamId = x.EquipmentTeamId}));
				_procedureRltWorkerRepo.Where(x=>x.ProcedureId==procedureId).ToList().ForEach(x=>_rltProcedureRltWorkerRepo.InsertAsync(new RltProcedureRltWorker(GuidGenerator.Create()){RltProcedureId = rltProcedureId,WorkerId = x.WorkerId}));
				_procedureRltFileRepo.Where(x=>x.ProcedureId==procedureId).ToList().ForEach(x=>_rltProcedureRltFileRepo.InsertAsync(new RltProcedureRltFile(GuidGenerator.Create()){RltProcedureId = rltProcedureId,FileId = x.FileId}));
				i++;
			});
			return true;
		}

		/// <summary>
		/// 获取 content 所有 列表 
		/// </summary>
		/// <returns></returns>
		private List<SubItemContent> GetSubItemContents()
		{
			List<SubItemContent> subItemContents = Repository.WithDetails()
				.OrderBy(x=>x.Order) // 根据 排序 排序 ……
				.ToList();
			return subItemContents;
		}

		public async Task<bool> MoveRltProcedure(Guid id,MoveType moveType )
		{
			SubItemContentRltProcedure now = await _subItemContentRltProcedureRepo.GetAsync(id);
			int nowOrder = now.Sort;
			if (now.SubItemContentId==null|| now.SubItemContentId == Guid.Empty) throw new UserFriendlyException(" subitemContentRltProcedure表里面  找不到subItemContentId!");
			List<SubItemContentRltProcedure> subItemContents = _subItemContentRltProcedureRepo.WithDetails().Where(x=>x.SubItemContentId==now.SubItemContentId).OrderBy(x=>x.Sort).ToList();
			if (subItemContents.Count==1) throw new UserFriendlyException("只有一个,不能上移也不能下移!");
			int index = subItemContents.FindIndex(x => x.Id == id);
			if (index == 0 && moveType == MoveType.Up) throw new UserFriendlyException("位于首位,不能上移了!");
			if (subItemContents.Last().Id==id && moveType == MoveType.Down) throw new UserFriendlyException("位于最后一位,不能下移了!");
			switch (moveType)
			{
				case MoveType.Up:
					// 找到 上一位 
					SubItemContentRltProcedure prev = subItemContents[index-1];
					int lastOrder = prev.Sort;
					prev.Sort = nowOrder;
					now.Sort = lastOrder;
					await _subItemContentRltProcedureRepo.UpdateAsync(now);
					await _subItemContentRltProcedureRepo.UpdateAsync(prev);
					break;
				case MoveType.Down:
					// 找到下一位
					SubItemContentRltProcedure next = subItemContents[index+1];
					int nextOrder = next.Sort;
					next.Sort = nowOrder;
					now.Sort = nextOrder;
					await _subItemContentRltProcedureRepo.UpdateAsync(now);
					await _subItemContentRltProcedureRepo.UpdateAsync(next);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
			}

			return true;
		}

		/// <summary>
		/// 根据  RltProcedureId 获取 关联 的选择 id 列表 
		/// </summary>
		/// <param name="rltProcedureId"></param>
		/// <returns></returns>
		public async Task<dynamic> GetSelectRltIdsByRltProcedureId(Guid rltProcedureId)
		{
			// 获取选择的文件id 
			List<Guid> selectFileIds = _rltProcedureRltFileRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.FileId).ToList();
			// 获取工种的文件id 
			List<Guid> selectWorkerIds = _rltProcedureRltWorkerRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.WorkerId).ToList();
			// 获取台班的文件id 
			List<Guid> selectEquipmentTeamIds = _rltProcedureRltEquipmentTeamRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.EquipmentTeamId).ToList();
			// 获取工程量的文件id 
			List<Guid> selectMaterialIds = _rltProcedureRltMaterialRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.MaterialId).ToList();
			return new {selectFileIds, selectMaterialIds, selectWorkerIds, selectEquipmentTeamIds};
		}

		/// <summary>
		/// 获取 关联工序表 关联 的其他 表 的 列表 (worker ,material ,equipment )
		/// </summary>
		/// <returns></returns>
		public async Task<dynamic> GetRltProcedureRltOtherList(Guid rltProcedureId)
		{
			#region  获取原始表

			List<ConstructionBaseWorkerDto> workerDtos = ObjectMapper.Map<List<Worker>,List<ConstructionBaseWorkerDto>>( await _workerRepo.GetListAsync());
			List<EquipmentTeamDto> equipmentTeamDtos = ObjectMapper.Map<List<EquipmentTeam>,List<EquipmentTeamDto>>(  _equipRepo.WithDetails().ToList());
			List<ConstructionBaseMaterialDto> materialDtos = ObjectMapper.Map<List<ConstructionBaseMaterial>,List<ConstructionBaseMaterialDto>>( await _matRepo.GetListAsync());

			#endregion
			
			#region 获取 工序 关联表 rltProcedureRlt xxx 

			List<RltProcedureRltWorker> rltProcedureRltWorkers = _rltProcedureRltWorkerRepo.WithDetails().Where(x => x.RltProcedureId == rltProcedureId).ToList();
			List<RltProcedureRltEquipmentTeam> rltProcedureRltEquipmentTeams = _rltProcedureRltEquipmentTeamRepo.WithDetails().Where(x => x.RltProcedureId == rltProcedureId).ToList();
			List<RltProcedureRltMaterial> rltProcedureRltMaterials = _rltProcedureRltMaterialRepo.WithDetails().Where(x => x.RltProcedureId == rltProcedureId).ToList();

			#endregion

			#region 工序 关联表 rltProcedureRlt xxx  里面 有数量的属性,把 原始表的局部 元素的 count  属性 覆盖成 关联表的count
			// newWorkerDtos: [
			// {count:0}, //(这个是原始的)
			// {count:33} //(这个是覆盖的)
			// ]
			workerDtos.ForEach(dto =>
			{
				RltProcedureRltWorker procedureRltWorker = rltProcedureRltWorkers.FirstOrDefault(rltProcedureRltWorker => rltProcedureRltWorker.WorkerId == dto.Id);
				if (procedureRltWorker!=null) dto.Count = procedureRltWorker.Count;
			});
			equipmentTeamDtos.ForEach(dto =>
			{
				RltProcedureRltEquipmentTeam procedureRltEquipmentTeam = rltProcedureRltEquipmentTeams.FirstOrDefault(rltProcedureRltEquipmentTeam =>rltProcedureRltEquipmentTeam.EquipmentTeamId==dto.Id );
				if (procedureRltEquipmentTeam!=null) dto.Count = procedureRltEquipmentTeam.Count;
			});
			materialDtos.ForEach(dto =>
			{
				RltProcedureRltMaterial rltProcedureRltMaterial = rltProcedureRltMaterials.FirstOrDefault(rltMaterial => rltMaterial.MaterialId == dto.Id);
				if (rltProcedureRltMaterial!=null) dto.Count = rltProcedureRltMaterial.Count;
			});
			#endregion
			return new {workers= workerDtos,equipmentTeams= equipmentTeamDtos,materials= materialDtos};
		}

		/// <summary>
		/// subItem 排序 移动  
		/// </summary>
		/// <param name="id"></param>
		/// <param name="moveType"></param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public async Task<bool> Move(Guid id ,MoveType moveType)
		{
			SubItemContent now = await Repository.GetAsync(id);
			int nowOrder = now.Order;
			if (!now.ParentId.HasValue|| now.ParentId.Value == Guid.Empty) throw new UserFriendlyException("找不到父id!");
			List<SubItemContent> subItemContents = Repository.Where(x=>x.ParentId==now.ParentId.Value).OrderBy(x=>x.Order).ToList();
			if (subItemContents.Count==1) throw new UserFriendlyException("只有一个,不能上移也不能下移!");
			int index = subItemContents.FindIndex(x => x.Id == id);
			if (index == 0 && moveType == MoveType.Up) throw new UserFriendlyException("位于首位,不能上移了!");
			if (subItemContents.Last().Id==id && moveType == MoveType.Down) throw new UserFriendlyException("位于最后一位,不能下移了!");
			switch (moveType)
			{
				case MoveType.Up:
					// 找到 上一位 
					SubItemContent prev = subItemContents[index-1];
					int lastOrder = prev.Order;
					prev.Order = nowOrder;
					now.Order = lastOrder;
					await Repository.UpdateAsync(now);
					await Repository.UpdateAsync(prev);
					break;
				case MoveType.Down:
					// 找到下一位
					SubItemContent next = subItemContents[index+1];
					int nextOrder = next.Order;
					next.Order = nowOrder;
					now.Order = nextOrder;
					await Repository.UpdateAsync(now);
					await Repository.UpdateAsync(next);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
			}

			return true;
		}
	}
}