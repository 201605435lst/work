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
		/// ??????????????????
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override async Task<SubItemContentDto> CreateAsync(SubItemContentCreateDto input)
		{
			if (input.Name.IsNullOrWhiteSpace()) throw new UserFriendlyException("??????????????????????????????!");
			if ((int)input.NodeType==0) throw new UserFriendlyException("??????????????????????????????!");
			if (input.ParentId == null || input.ParentId.Value == Guid.Empty) throw new UserFriendlyException("????????????id!");

			var inputParentId = input.ParentId;
			// ???????????? ?????? ??? content  ?????????
			int count                     = Repository.Count(x => x.ParentId== inputParentId);
			SubItemContent subItemContent = ObjectMapper.Map<SubItemContentCreateDto     , SubItemContent>(input);
			subItemContent.Order          = count + 1;
			SubItemContent insertAsync    = await Repository.InsertAsync(subItemContent);
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(insertAsync);
		}

		
		public async Task<SubItemContentDto> InitContent(Guid subItemId)
		{
			SubItem subItem = await _subItemRepos.GetAsync(subItemId);
			if (subItem.SubItemContent != null) throw new UserFriendlyException($"??????,{subItem.Name} ???????????????,???????????? app/api/subitemContent/initContent ??????!");
			SubItemContent subItemContent = await Repository.InsertAsync(new SubItemContent(GuidGenerator.Create()) {Name = subItem.Name,  SubItemId =  subItem.Id, NodeType =  SubItemNodeType.All,Order = 1});
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(subItemContent);
		}


		/// <summary>
		/// ?????? contentId ?????? ?????? content ???
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
				// ?????? ????????????????????? 
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
		/// ?????? subItemId ?????? ?????? content ???
		/// </summary>
		/// <param name="subItemId"></param>
		/// <returns></returns>
		public async Task<SubItemContentDto> GetSingleTreeBySubItemId(Guid subItemId)
		{
			if (_subItemRepos.Any(x=>x.Id==subItemId)) throw new UserFriendlyException($"subItem?????????! subItemId={subItemId}");
			SubItemContent subItemContent = Repository.WithDetails().FirstOrDefault(x => x.SubItemId == subItemId);
			if (subItemContent==null) return new SubItemContentDto();
			SubItemContentDto singleTree =  GetSingleTree(subItemContent.Id);
			return singleTree;
		}

		public Task<SubItemContentDto> GetSingleTreeWithProcedure(Guid id)
		{
			SubItemContentDto subItemContentDto = GetSingleTree(id);
			// ?????????,??? nodeType ??? NodeType.WorkSur ??? ?????????~
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
					// ???????????? ??? ??? ???????????? ??????id ,?????? ??? ?????????
					.Select(x=>new SubItemContentDto(){Id = x.Id,Name = x.Procedure.Name,NodeType = SubItemNodeType.Procedure})
					.ToList();
				content.Children = procedures;
				return content;
			}
			content.Children.ForEach(x => GetWorkSurContent(x));
			return content;
		}

		/// <summary>
		/// ?????? rltProcedureId  ?????? ???????????????
		/// </summary>
		/// <returns></returns>
		public async Task<bool> DeleteRltProcedure(Guid rltProcedureId)
		{
			await _subItemContentRltProcedureRepo.DeleteAsync(rltProcedureId);
			// ????????????-?????? ?????? ????????????
			_rltProcedureRltWorkerRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltWorkerRepo.DeleteAsync(x));
			// ????????????-?????? ?????? ????????????
			_rltProcedureRltEquipmentTeamRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltEquipmentTeamRepo.DeleteAsync(x));
			// ????????????-????????? ?????? ????????????
			_rltProcedureRltMaterialRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltMaterialRepo.DeleteAsync(x));
			// ????????????-?????? ?????? ????????????
			_rltProcedureRltFileRepo.Where(x=>x.RltProcedureId==rltProcedureId).ToList().ForEach(x=>_rltProcedureRltFileRepo.DeleteAsync(x));
			return true;
		}

		/// <summary>
		/// ???????????? ?????? (???????????????)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override Task<PagedResultDto<SubItemContentDto>> GetListAsync(SubItemContentSearchDto input)
		{
			// List<SubItemContent> subItemContents = Repository.WithDetails().Where(x=>x.NodeType==NodeType.All).ToList();
			List<SubItemContent> subItemContents = GetSubItemContents();
			// ??? ???????????? ?????? ????????????????????? ?????? (????????????) ,?????? ??? query where ?????? ?????? ,???????????? ??????????????? ?????????????????? ??????
			// ????????? ???????????? ????????? 
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
		// 	if (count!=procedureIds.Count) throw new UserFriendlyException("???????????????id????????????id????????????????????????!");
		// 	SubItemContent subItemContent = await Repository.GetAsync(id);
		// 	if (subItemContent.NodeType!=NodeType.WorkSur) throw new UserFriendlyException("??????????????????????????????????????????????????????!");
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
		/// TODO ?????????,??????????????? ???????????? ???????????? ????????? ?????? ?????? ?????? AddProcedure ?????? ????????? content  ???????????? 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="procedureIds"></param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<SubItemContentDto> AddProcedureOld(Guid id, List<Guid> procedureIds)
		{
			int count = _procedureRepo.AsQueryable().Count(x => procedureIds.Contains(x.Id));
			if (count != procedureIds.Count) throw new UserFriendlyException("???????????????id????????????id????????????????????????!");
			SubItemContent subItemContent = await Repository.GetAsync(id);
			if (subItemContent.NodeType != SubItemNodeType.WorkSur) throw new UserFriendlyException("??????????????????????????????????????????????????????!");
			subItemContent.SubItemContentRltProcedures = procedureIds
				.Select((x, i) => new SubItemContentRltProcedure(GuidGenerator.Create()) {ProcedureId = x, Sort = i})
				.ToList();
			SubItemContent updateAsync = await Repository.UpdateAsync(subItemContent);
			return ObjectMapper.Map<SubItemContent, SubItemContentDto>(updateAsync);
		}
		public async Task<bool> AddProcedure(Guid id, List<Guid> procedureIds)
		{
			// ???????????? 
			int count = _procedureRepo.AsQueryable().Count(x => procedureIds.Contains(x.Id));
			if (count!=procedureIds.Count) throw new UserFriendlyException("???????????????id????????????id????????????????????????!");
			SubItemContent subItemContent = await Repository.GetAsync(id);
			if (subItemContent.NodeType!=SubItemNodeType.WorkSur) throw new UserFriendlyException("??????????????????????????????????????????????????????!");
			// rltProcedure  ??? ?????????????????? 
			List<SubItemContentRltProcedure> subItemContentRltProcedures = _subItemContentRltProcedureRepo.Where(x => x.SubItemContentId == id).ToList();
			subItemContentRltProcedures.ForEach(rltProcedure =>
			{
				// ????????????-?????? ?????? ????????????
				_rltProcedureRltWorkerRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltWorkerRepo.DeleteAsync(x));
				// ????????????-?????? ?????? ????????????
				_rltProcedureRltEquipmentTeamRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltEquipmentTeamRepo.DeleteAsync(x));
				// ????????????-????????? ?????? ????????????
				_rltProcedureRltMaterialRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltMaterialRepo.DeleteAsync(x));
				// ????????????-?????? ?????? ????????????
				_rltProcedureRltFileRepo.Where(x=>x.RltProcedureId==rltProcedure.Id).ToList().ForEach(x=>_rltProcedureRltFileRepo.DeleteAsync(x));
				
				// ?????? ?????? ????????? (??????????????????????????????,??????????????????????????? ???????????????????????????????????????????????? ??? ???)
				 _subItemContentRltProcedureRepo.DeleteAsync(rltProcedure);
				
			});
			int i = 0;
			procedureIds.ForEach( procedureId =>
			{
				Guid rltProcedureId = GuidGenerator.Create(); // ?????? rltProcedure ????????????id 
				// ?????? ?????? ??? ??????  
				_subItemContentRltProcedureRepo.InsertAsync(new SubItemContentRltProcedure(rltProcedureId) {SubItemContentId = id, ProcedureId = procedureId,Sort = i});
				// ?????? ??????-xxx ?????????,?????????????????? ????????? rltProcedureRlt xxx ??????
				_procedureRltMatRepo.Where(x => x.ProcedureId == procedureId).ToList().ForEach(x => _rltProcedureRltMaterialRepo.InsertAsync(new RltProcedureRltMaterial(GuidGenerator.Create()) {RltProcedureId = rltProcedureId,MaterialId = x.MaterialId,Count = x.Count}));
				_procedureRltEquipRepo.Where(x=>x.ProcedureId==procedureId).ToList().ForEach(x=>_rltProcedureRltEquipmentTeamRepo.InsertAsync(new RltProcedureRltEquipmentTeam(GuidGenerator.Create()){RltProcedureId = rltProcedureId,EquipmentTeamId = x.EquipmentTeamId}));
				_procedureRltWorkerRepo.Where(x=>x.ProcedureId==procedureId).ToList().ForEach(x=>_rltProcedureRltWorkerRepo.InsertAsync(new RltProcedureRltWorker(GuidGenerator.Create()){RltProcedureId = rltProcedureId,WorkerId = x.WorkerId}));
				_procedureRltFileRepo.Where(x=>x.ProcedureId==procedureId).ToList().ForEach(x=>_rltProcedureRltFileRepo.InsertAsync(new RltProcedureRltFile(GuidGenerator.Create()){RltProcedureId = rltProcedureId,FileId = x.FileId}));
				i++;
			});
			return true;
		}

		/// <summary>
		/// ?????? content ?????? ?????? 
		/// </summary>
		/// <returns></returns>
		private List<SubItemContent> GetSubItemContents()
		{
			List<SubItemContent> subItemContents = Repository.WithDetails()
				.OrderBy(x=>x.Order) // ?????? ?????? ?????? ??????
				.ToList();
			return subItemContents;
		}

		public async Task<bool> MoveRltProcedure(Guid id,MoveType moveType )
		{
			SubItemContentRltProcedure now = await _subItemContentRltProcedureRepo.GetAsync(id);
			int nowOrder = now.Sort;
			if (now.SubItemContentId==null|| now.SubItemContentId == Guid.Empty) throw new UserFriendlyException(" subitemContentRltProcedure?????????  ?????????subItemContentId!");
			List<SubItemContentRltProcedure> subItemContents = _subItemContentRltProcedureRepo.WithDetails().Where(x=>x.SubItemContentId==now.SubItemContentId).OrderBy(x=>x.Sort).ToList();
			if (subItemContents.Count==1) throw new UserFriendlyException("????????????,???????????????????????????!");
			int index = subItemContents.FindIndex(x => x.Id == id);
			if (index == 0 && moveType == MoveType.Up) throw new UserFriendlyException("????????????,???????????????!");
			if (subItemContents.Last().Id==id && moveType == MoveType.Down) throw new UserFriendlyException("??????????????????,???????????????!");
			switch (moveType)
			{
				case MoveType.Up:
					// ?????? ????????? 
					SubItemContentRltProcedure prev = subItemContents[index-1];
					int lastOrder = prev.Sort;
					prev.Sort = nowOrder;
					now.Sort = lastOrder;
					await _subItemContentRltProcedureRepo.UpdateAsync(now);
					await _subItemContentRltProcedureRepo.UpdateAsync(prev);
					break;
				case MoveType.Down:
					// ???????????????
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
		/// ??????  RltProcedureId ?????? ?????? ????????? id ?????? 
		/// </summary>
		/// <param name="rltProcedureId"></param>
		/// <returns></returns>
		public async Task<dynamic> GetSelectRltIdsByRltProcedureId(Guid rltProcedureId)
		{
			// ?????????????????????id 
			List<Guid> selectFileIds = _rltProcedureRltFileRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.FileId).ToList();
			// ?????????????????????id 
			List<Guid> selectWorkerIds = _rltProcedureRltWorkerRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.WorkerId).ToList();
			// ?????????????????????id 
			List<Guid> selectEquipmentTeamIds = _rltProcedureRltEquipmentTeamRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.EquipmentTeamId).ToList();
			// ????????????????????????id 
			List<Guid> selectMaterialIds = _rltProcedureRltMaterialRepo.Where(x => x.RltProcedureId == rltProcedureId).Select(x => x.MaterialId).ToList();
			return new {selectFileIds, selectMaterialIds, selectWorkerIds, selectEquipmentTeamIds};
		}

		/// <summary>
		/// ?????? ??????????????? ?????? ????????? ??? ??? ?????? (worker ,material ,equipment )
		/// </summary>
		/// <returns></returns>
		public async Task<dynamic> GetRltProcedureRltOtherList(Guid rltProcedureId)
		{
			#region  ???????????????

			List<ConstructionBaseWorkerDto> workerDtos = ObjectMapper.Map<List<Worker>,List<ConstructionBaseWorkerDto>>( await _workerRepo.GetListAsync());
			List<EquipmentTeamDto> equipmentTeamDtos = ObjectMapper.Map<List<EquipmentTeam>,List<EquipmentTeamDto>>(  _equipRepo.WithDetails().ToList());
			List<ConstructionBaseMaterialDto> materialDtos = ObjectMapper.Map<List<ConstructionBaseMaterial>,List<ConstructionBaseMaterialDto>>( await _matRepo.GetListAsync());

			#endregion
			
			#region ?????? ?????? ????????? rltProcedureRlt xxx 

			List<RltProcedureRltWorker> rltProcedureRltWorkers = _rltProcedureRltWorkerRepo.WithDetails().Where(x => x.RltProcedureId == rltProcedureId).ToList();
			List<RltProcedureRltEquipmentTeam> rltProcedureRltEquipmentTeams = _rltProcedureRltEquipmentTeamRepo.WithDetails().Where(x => x.RltProcedureId == rltProcedureId).ToList();
			List<RltProcedureRltMaterial> rltProcedureRltMaterials = _rltProcedureRltMaterialRepo.WithDetails().Where(x => x.RltProcedureId == rltProcedureId).ToList();

			#endregion

			#region ?????? ????????? rltProcedureRlt xxx  ?????? ??????????????????,??? ?????????????????? ????????? count  ?????? ????????? ????????????count
			// newWorkerDtos: [
			// {count:0}, //(??????????????????)
			// {count:33} //(??????????????????)
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
		/// subItem ?????? ??????  
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
			if (!now.ParentId.HasValue|| now.ParentId.Value == Guid.Empty) throw new UserFriendlyException("????????????id!");
			List<SubItemContent> subItemContents = Repository.Where(x=>x.ParentId==now.ParentId.Value).OrderBy(x=>x.Order).ToList();
			if (subItemContents.Count==1) throw new UserFriendlyException("????????????,???????????????????????????!");
			int index = subItemContents.FindIndex(x => x.Id == id);
			if (index == 0 && moveType == MoveType.Up) throw new UserFriendlyException("????????????,???????????????!");
			if (subItemContents.Last().Id==id && moveType == MoveType.Down) throw new UserFriendlyException("??????????????????,???????????????!");
			switch (moveType)
			{
				case MoveType.Up:
					// ?????? ????????? 
					SubItemContent prev = subItemContents[index-1];
					int lastOrder = prev.Order;
					prev.Order = nowOrder;
					now.Order = lastOrder;
					await Repository.UpdateAsync(now);
					await Repository.UpdateAsync(prev);
					break;
				case MoveType.Down:
					// ???????????????
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