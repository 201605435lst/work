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
    public class CostManagementPeopleCostAppService : CostManagementAppService, ICostManagementPeopleCostAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<PeopleCost, Guid> _peopleCostRepository;
        protected IIdentityUserRepository _userRepository { get; }

        public CostManagementPeopleCostAppService(
           IGuidGenerator guidGenerator,
               IIdentityUserRepository userRepository,
               IRepository<PeopleCost, Guid> peopleCostRepository
            )
        {
            _userRepository = userRepository;
            _guidGenerator = guidGenerator;
            _peopleCostRepository = peopleCostRepository;
        }
        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PeopleCostDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var peopleCost = _peopleCostRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (peopleCost == null) throw new UserFriendlyException("当前数据不存在");
            var result = ObjectMapper.Map<PeopleCost, PeopleCostDto>(peopleCost);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PeopleCostDto>> GetList(PeopleCostSearchDto input)
        {
            var result = new PagedResultDto<PeopleCostDto>();
            var peopleCost = _peopleCostRepository.WithDetails()
                  .WhereIf(input.PayeeId != Guid.Empty && input.PayeeId != null, x => x.PayeeId == input.PayeeId)
                  .WhereIf(input.ProfessionalId != Guid.Empty && input.ProfessionalId != null, x => x.ProfessionalId == input.ProfessionalId)
                .WhereIf(input.StartTime != null, x => x.Date >= input.StartTime)
                  .WhereIf(input.EndTime != null, x => x.Date < input.EndTime);
            var res = ObjectMapper.Map<List<PeopleCost>, List<PeopleCostDto>>(peopleCost.OrderByDescending(x => x.Date).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
          
            result.Items = res;
            result.TotalCount = peopleCost.Count();
            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PeopleCostDto> Create(PeopleCostCreateDto input)
        {
            var peopleCost = new PeopleCost();
            ObjectMapper.Map(input, peopleCost);
            //1、保存基本信息
            peopleCost.SetId(_guidGenerator.Create());
            await _peopleCostRepository.InsertAsync(peopleCost);
            var peopleCostDto = ObjectMapper.Map<PeopleCost, PeopleCostDto>(peopleCost);
            return peopleCostDto;
        }

        public async Task<bool> Delete(List<Guid> Ids)
        {
            if (Ids == null || Ids.Count==0) throw new UserFriendlyException("选择你要删除的数据");
          
                await _peopleCostRepository.DeleteAsync(x=>Ids.Contains(x.Id));
           

            return true;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PeopleCostDto> Update(PeopleCostUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var peopleCost = await _peopleCostRepository.GetAsync(input.Id);
            if (peopleCost == null) throw new UserFriendlyException("当前数据不存在");
            ObjectMapper.Map(input, peopleCost);
            await _peopleCostRepository.UpdateAsync(peopleCost);
            return ObjectMapper.Map<PeopleCost, PeopleCostDto>(peopleCost);
        }
     
    }
}
