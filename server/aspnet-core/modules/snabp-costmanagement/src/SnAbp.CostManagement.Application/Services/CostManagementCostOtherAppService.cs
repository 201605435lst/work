using SnAbp.CostManagement.Dtos;
using SnAbp.CostManagement.Entities;
using SnAbp.CostManagement.IServices;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.CostManagement.Services
{
   public class CostManagementCostOtherAppService : CostManagementAppService, ICostManagementCostOtherAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<CostOther, Guid> _costOtherRepository;
        protected IIdentityUserRepository _userRepository { get; }

        public CostManagementCostOtherAppService(
           IGuidGenerator guidGenerator,
               IIdentityUserRepository userRepository,
               IRepository<CostOther, Guid> costOtherRepository
            )
        {
            _userRepository = userRepository;
            _guidGenerator = guidGenerator;
            _costOtherRepository = costOtherRepository;
        }
        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CostOtherDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var costOther = _costOtherRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (costOther == null) throw new UserFriendlyException("当前数据不存在");
            var result = ObjectMapper.Map<CostOther, CostOtherDto>(costOther);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CostOtherDto>> GetList(CostOtherSearchDto input)
        {
            var result = new PagedResultDto<CostOtherDto>();
            var costOther = _costOtherRepository.WithDetails()
                .WhereIf(input.TypeId != Guid.Empty && input.TypeId != null, x => x.TypeId == input.TypeId)
            .WhereIf(input.StartTime != null, x => x.Date >= input.StartTime)
                  .WhereIf(input.EndTime != null, x => x.Date < input.EndTime);


            var res = ObjectMapper.Map<List<CostOther>, List<CostOtherDto>>(costOther.OrderBy(x => x.Type.Name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            foreach (var item in res)
            {
                var user = await _userRepository.GetAsync(item.CreatorId.GetValueOrDefault());
                item.CreatorName = user.Name;
            }
            result.Items = res;
            result.TotalCount = costOther.Count();
            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CostOtherDto> Create(CostOtherCreateDto input)
        {
            var costOther = new CostOther();
            ObjectMapper.Map(input, costOther);
            //1、保存基本信息
            costOther.SetId(_guidGenerator.Create());
            await _costOtherRepository.InsertAsync(costOther);
            var costOtherDto = ObjectMapper.Map<CostOther, CostOtherDto>(costOther);
            return costOtherDto;
        }

        public async Task<bool> Delete(List<Guid> Ids)
        {
            if (Ids == null || Ids.Count == 0) throw new UserFriendlyException("选择你要删除的数据");

            await _costOtherRepository.DeleteAsync(x => Ids.Contains(x.Id));


            return true;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CostOtherDto> Update(CostOtherUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var costOther = await _costOtherRepository.GetAsync(input.Id);
            if (costOther == null) throw new UserFriendlyException("当前数据不存在");
            ObjectMapper.Map(input, costOther);
            await _costOtherRepository.UpdateAsync(costOther);
            return ObjectMapper.Map<CostOther, CostOtherDto>(costOther);
        }

    }
}
