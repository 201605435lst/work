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
    public class CostManagementContractAppService : CostManagementAppService, ICostManagementContractAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Contract, Guid> _contractRepository;
        protected IIdentityUserRepository _userRepository { get; }
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<PeopleCost, Guid> _peopleCostRepository;
        private readonly IRepository<MoneyList, Guid> _moneyListRepository;
        private readonly IRepository<CostOther, Guid> _costOtherRepository;

        private readonly IRepository<ContractRltFile, Guid> _contractRltFileRepository;

        public CostManagementContractAppService(
            IRepository<PeopleCost, Guid> peopleCostRepository,
          IRepository<MoneyList, Guid> moneyListRepository,
          IRepository<CostOther, Guid> costOtherRepository,
        IRepository<Organization, Guid> organizationRepository,
               IHttpContextAccessor httpContextAccessor,
           IGuidGenerator guidGenerator,
               IIdentityUserRepository userRepository,
               IRepository<Contract, Guid> contractRepository,
                IRepository<ContractRltFile, Guid> contractRltFileRepository
            )
        {
            _peopleCostRepository = peopleCostRepository;
            _moneyListRepository = moneyListRepository;
            _costOtherRepository = costOtherRepository;
            _organizationRepository = organizationRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _contractRltFileRepository = contractRltFileRepository;
            _guidGenerator = guidGenerator;
            _contractRepository = contractRepository;
        }
        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CostContractDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var contract = _contractRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (contract == null) throw new UserFriendlyException("当前数据不存在");
            var result = ObjectMapper.Map<Contract, CostContractDto>(contract);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CostContractDto>> GetList(CostContractSearchDto input)
        {
            var result = new PagedResultDto<CostContractDto>();
            var contract = _contractRepository.WithDetails()
                  .WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name))
                .WhereIf(input.TypeId != Guid.Empty && input.TypeId != null, x => x.TypeId == input.TypeId)
             .WhereIf(input.StartTime != null, x => x.Date >= input.StartTime)
                  .WhereIf(input.EndTime != null, x => x.Date < input.EndTime);

            var res = ObjectMapper.Map<List<Contract>, List<CostContractDto>>(contract.OrderByDescending(x => x.Date).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            result.Items = res;
            result.TotalCount = contract.Count();
            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CostContractDto> Create(CostContractCreateDto input)
        {
            var contract = new Contract();
            ObjectMapper.Map(input, contract);
            //1、保存基本信息
            contract.SetId(_guidGenerator.Create());
            ///保存附件
            contract.ContractRltFiles = new List<ContractRltFile>();
            foreach (var contractRltFiles in input.ContractRltFiles)
            {
                contract.ContractRltFiles.Add(
                    new ContractRltFile(_guidGenerator.Create())
                    {
                        FileId = contractRltFiles.FileId,
                    });
            }
            await _contractRepository.InsertAsync(contract);
            var contractDto = ObjectMapper.Map<Contract, CostContractDto>(contract);
            return contractDto;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CostContractDto> Update(CostContractUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var contract = await _contractRepository.GetAsync(input.Id);
            if (contract == null) throw new UserFriendlyException("当前数据不存在");
            //清除保存的附件关联表信息
            await _contractRltFileRepository.DeleteAsync(x => x.ContractId == input.Id);
            ObjectMapper.Map(input, contract);
            ///保存附件
            contract.ContractRltFiles = new List<ContractRltFile>();
            foreach (var contractRltFiles in input.ContractRltFiles)
            {
                contract.ContractRltFiles.Add(
                    new ContractRltFile(_guidGenerator.Create())
                    {
                        FileId = contractRltFiles.FileId,
                    });
            }

            await _contractRepository.UpdateAsync(contract);
            return ObjectMapper.Map<Contract, CostContractDto>(contract);
        }
        public async Task<bool> Delete(List<Guid> Ids)
        {
            if (Ids == null || Ids.Count == 0) throw new UserFriendlyException("选择你要删除的数据");

            await _contractRepository.DeleteAsync(x => Ids.Contains(x.Id));
            return true;
        }
        /// <summary>
        /// 盈亏分析
        /// </summary>
        /// <returns></returns>
        public Task<BreakevenAnalysisDto> GetStatistics()
        {
            var breakevenAnalysisDto = new BreakevenAnalysisDto();
            // 新增条件过滤
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationName = organization != null ? organization.Name : null;
            //得到组织机构名字
            breakevenAnalysisDto.OrganizationName = organizationName;
            /*得到合同创建的最早时间*/
            var contractDate = _contractRepository.OrderBy(x => x.Date).FirstOrDefault()?.Date;
            var otherCostDate = _costOtherRepository.OrderBy(x => x.Date).FirstOrDefault()?.Date;
            var finalDate = (otherCostDate == null || contractDate < otherCostDate) ? contractDate : otherCostDate;
            var peopleCostDate = _peopleCostRepository.OrderBy(x => x.Date).FirstOrDefault()?.Date;
            finalDate = (peopleCostDate == null || finalDate < peopleCostDate) ? finalDate : peopleCostDate;
            decimal sumCount = 0;
            decimal sumContract = 0;
            if (finalDate != null)
            {
                for (var date = (DateTime)finalDate; date <= DateTime.Now; date = date.AddMonths(1))
                {
                    var _peopleList = _peopleCostRepository.WithDetails().ToList().Where(x => string.Format("{0:yyyy-MM}", x.Date) == string.Format("{0:yyyy-MM}", date)).ToList().Sum(x => x.Money);
                    var _otherList = _costOtherRepository.WithDetails().ToList().Where(x => string.Format("{0:yyyy-MM}", x.Date) == string.Format("{0:yyyy-MM}", date)).ToList().Sum(x => x.Money);
                    var _contractList = _contractRepository.WithDetails().ToList().Where(x => string.Format("{0:yyyy-MM}", x.Date) == string.Format("{0:yyyy-MM}", date)).ToList().Sum(x => x.Money);
                    sumCount = sumCount + _peopleList + _otherList;
                    sumContract += _contractList;
                    breakevenAnalysisDto.Dates.Add(string.Format("{0:yyyy-MM}", date));
                    breakevenAnalysisDto.ContractAmount.Add(sumContract);
                    breakevenAnalysisDto.TotalExpenditure.Add(sumCount);
                }
            }
            return Task.FromResult(breakevenAnalysisDto);
        }
    }
}