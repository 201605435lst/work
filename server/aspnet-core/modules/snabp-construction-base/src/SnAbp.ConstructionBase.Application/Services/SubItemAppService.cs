using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.Enums;
using SnAbp.ConstructionBase.IServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Services
{
	// [Authorize]
	public class SubItemAppService :
		CrudAppService<SubItem, SubItemDto, Guid, SubItemSearchDto, SubItemCreateDto, SubItemUpdateDto>,
		ISubItemAppService
	{
		private readonly IRepository<SubItemContent, Guid> _contentRepo;

		public SubItemAppService(
			IRepository<SubItem       , Guid> repository,
			IRepository<SubItemContent, Guid> contentRepo
		) : base(repository)
		{
			_contentRepo = contentRepo;
		}

		/// <summary>
		/// 重写获取列表 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override Task<PagedResultDto<SubItemDto>> GetListAsync(SubItemSearchDto input)
		{
			IQueryable<SubItem> query    = Repository.WithDetails();
			int total                    = query.Count();
			List<SubItem> list           = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
			List<SubItemDto> subItemDtos = ObjectMapper.Map<List<SubItem>, List<SubItemDto>>(list);
			subItemDtos.ForEach(x=>x.HasWorkSur=HasWorkSur(x));
			return Task.FromResult(new PagedResultDto<SubItemDto>(total, subItemDtos));
		}
		/// <summary>
		/// 是否含有作业面
		/// </summary>
		/// <returns></returns>
		private  bool HasWorkSur(SubItemDto subItem)
		{
			if (subItem.SubItemContent==null) return false;
			List<SubItemContent> subItemContents = _contentRepo.GetListAsync().Result;
			SubItemContent subItemContent        = subItemContents.FirstOrDefault(x => x.Id == subItem.SubItemContent.Id);
			return  CheckWorkSur(subItemContent);
		}

		/// <summary>
		/// 检查是否含有作业面 - 递归 
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		private bool CheckWorkSur(SubItemContent content)
		{
			if (content.NodeType       == SubItemNodeType.WorkSur) return true;
			if (content.Children       == null)             return false;
			if (content.Children.Count == 0)                return false;
			return content.Children.Any(CheckWorkSur);
		}
	}
}