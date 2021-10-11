using Microsoft.AspNetCore.Http;
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
    public class CostManagementMoneyListAppService : CostManagementAppService, ICostManagementMoneyListAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<MoneyList, Guid> _moneyListRepository;
        protected IIdentityUserRepository _userRepository { get; }
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CostManagementMoneyListAppService(
           IGuidGenerator guidGenerator,
           IRepository<Organization, Guid> organizationRepository,
        IIdentityUserRepository userRepository,
               IHttpContextAccessor httpContextAccessor,
               IRepository<MoneyList, Guid> moneyListRepository
            )
        {
            _organizationRepository = organizationRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _guidGenerator = guidGenerator;
            _moneyListRepository = moneyListRepository;
        }
        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MoneyListDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var moneyList = _moneyListRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (moneyList == null) throw new UserFriendlyException("当前数据不存在");
            var result = ObjectMapper.Map<MoneyList, MoneyListDto>(moneyList);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MoneyListDto>> GetList(MoneyListSearchDto input)
        {
            var result = new PagedResultDto<MoneyListDto>();
            var moneyList = _moneyListRepository.WithDetails()
                   .WhereIf(input.PayeeId != Guid.Empty && input.PayeeId != null, x => x.PayeeId == input.PayeeId)
                .WhereIf(input.TypeId != Guid.Empty && input.TypeId != null, x => x.TypeId == input.TypeId)
            .WhereIf(input.StartTime != null, x => x.Date >= input.StartTime)
                  .WhereIf(input.EndTime != null, x => x.Date < input.EndTime);
            var res = ObjectMapper.Map<List<MoneyList>, List<MoneyListDto>>(moneyList.OrderByDescending(x => x.Date).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            result.Items = res;
            result.TotalCount = moneyList.Count();
            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MoneyListDto> Create(MoneyListCreateDto input)
        {
            var moneyList = new MoneyList();
            ObjectMapper.Map(input, moneyList);
            //1、保存基本信息
            moneyList.SetId(_guidGenerator.Create());
            await _moneyListRepository.InsertAsync(moneyList);
            var moneyListDto = ObjectMapper.Map<MoneyList, MoneyListDto>(moneyList);
            return moneyListDto;
        }

        public async Task<bool> Delete(List<Guid> Ids)
        {
            if (Ids == null || Ids.Count == 0) throw new UserFriendlyException("选择你要删除的数据");

            await _moneyListRepository.DeleteAsync(x => Ids.Contains(x.Id));


            return true;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MoneyListDto> Update(MoneyListUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var moneyList = await _moneyListRepository.GetAsync(input.Id);
            if (moneyList == null) throw new UserFriendlyException("当前数据不存在");
            ObjectMapper.Map(input, moneyList);
            await _moneyListRepository.UpdateAsync(moneyList);
            return ObjectMapper.Map<MoneyList, MoneyListDto>(moneyList);
        }

        //public Task<MoneyListOrganizationDto> GetOrganization()
        //{
        //    // 新增条件过滤
        //    var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
        //    var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
        //    var organizationName = organization != null ? organization.Name : null;
        //    var moneyListOrganizationDto = new MoneyListOrganizationDto();
        //    moneyListOrganizationDto.OrganizationName = organizationName;
        //    return Task.FromResult(moneyListOrganizationDto);

        //}

        public Task<MoneyListStatisticsDto> GetStatistics(MoneyListSearchDto input)
        {
            var moneyListStatisticsDto = new MoneyListStatisticsDto();
            // 新增条件过滤
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationName = organization != null ? organization.Name : null;
            //得到组织机构名字
            moneyListStatisticsDto.OrganizationName = organizationName;
            var moneyList = _moneyListRepository.WithDetails()
              .WhereIf(input.StartTime != null, x => x.Date >= input.StartTime)
                  .WhereIf(input.EndTime != null, x => x.Date < input.EndTime).OrderBy(x => x.Date).ToList();
            foreach (var items in moneyList)
            {
                var date = items.Date;
                var _moneyList = moneyList.Where(x => string.Format("{0:yyyy-MM}", x.Date) == string.Format("{0:yyyy-MM}", date)).ToList();
                var moneyListId = _moneyList.Select(x => x.Id).ToList();
                if (_moneyList.Count > 0)
                {
                    Decimal _due = 0;
                    Decimal _paid = 0;
                    Decimal _receivable = 0;
                    Decimal _received = 0;
                    foreach (var item in _moneyList)
                    {
                        _due = _due + item.Due;
                        _paid = _paid + item.Paid;
                        _receivable = _due + item.Receivable;
                        _received = _due + item.Received;
                    }
                    moneyListStatisticsDto.Dates.Add(string.Format("{0:yyyy-MM}", date));
                    moneyListStatisticsDto.Dues.Add(_due);
                    moneyListStatisticsDto.Paids.Add(_paid);
                    moneyListStatisticsDto.Receivables.Add(_receivable);
                    moneyListStatisticsDto.Receiveds.Add(_received);
                }
                moneyList = moneyList.Where(x => !moneyListId.Contains(x.Id)).ToList();
            }
            return Task.FromResult(moneyListStatisticsDto);
        }
    }
}
