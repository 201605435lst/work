using Microsoft.AspNetCore.Authorization;
using SnAbp.Oa.Dtos;
using SnAbp.Oa.Entities;
using SnAbp.Oa.IServices;
using SnAbp.Oa.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Oa.Services
{
    [Authorize]
    public class OaDutyScheduleAppService : OaAppService, IOaDutyScheduleAppService
    {
        private readonly IRepository<DutyScheduleRltUser, Guid> _repositoryDutyScheduleRltUser;
        private readonly IRepository<DutySchedule, Guid> _repositoryDutySchedule;
        private readonly IGuidGenerator _guidGenerate;

        public OaDutyScheduleAppService(
             IRepository<DutySchedule, Guid> repositoryDutySchedule,
              IRepository<DutyScheduleRltUser, Guid> repositoryDutyScheduleRltUser,
            IGuidGenerator guidGenerate)
        {
            _repositoryDutyScheduleRltUser = repositoryDutyScheduleRltUser;
            _repositoryDutySchedule = repositoryDutySchedule;
            _guidGenerate = guidGenerate;
        }
        /// <summary>
        /// 获取单个值班信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DutyScheduleDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("值班id不能为空");
            var dutySchedule = _repositoryDutySchedule.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (dutySchedule == null) throw new UserFriendlyException("值班记录不存在");
            var result = ObjectMapper.Map<DutySchedule, DutyScheduleDto>(dutySchedule);
            return Task.FromResult(result);
        }

        public Task<List<DutyScheduleDto>> GetList(DutyScheduleSearchDto input)
        {
            var dutySchedule = _repositoryDutySchedule.WithDetails()
                 .Where(x => x.OrganizationId == input.OrganizationId)
                 .WhereIf(!string.IsNullOrEmpty(input.name), x => x.DutyScheduleRltUsers.Select(s => s.User).Any(m => m.Name.Contains(input.name)))
                 .WhereIf(input.Date != null, x => x.Date.ToString().Contains(input.Date.ToString()));
            var result = ObjectMapper.Map<List<DutySchedule>, List<DutyScheduleDto>>(dutySchedule.ToList());
            return Task.FromResult(result);

        }

        [Authorize(OaPermissions.DutySchedule.Create)]
        public async Task<DutyScheduleDto> Create(DutyScheduleCreateDto input)
        {
            if (input.DutyScheduleRltUsers.Count == 0) throw new UserFriendlyException("值班人不能为空");
            if (input.Date == null) throw new UserFriendlyException("值班日期不能为空");
            var dutySchedule = new DutySchedule(_guidGenerate.Create());
            dutySchedule.Date = input.Date;
            dutySchedule.OrganizationId = input.OrganizationId;
            dutySchedule.Remark = input.Remark;
            dutySchedule.DutyScheduleRltUsers = new List<DutyScheduleRltUser>();
            foreach (var dutyScheduleRltUsers in input.DutyScheduleRltUsers)
            {
                dutySchedule.DutyScheduleRltUsers.Add(
               new DutyScheduleRltUser(_guidGenerate.Create())
               {
                   UserId = dutyScheduleRltUsers.UserId,
               }
                );
            }
            await _repositoryDutySchedule.InsertAsync(dutySchedule);
            return ObjectMapper.Map<DutySchedule, DutyScheduleDto>(dutySchedule);
        }


        [Authorize(OaPermissions.DutySchedule.Update)]
        public async Task<DutyScheduleDto> Update(DutyScheduleUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入值班表id");
            var dutySchedule = await _repositoryDutySchedule.GetAsync(input.Id);
            if (dutySchedule == null) throw new UserFriendlyException("值班表不存在");
            if (input.DutyScheduleRltUsers.Count == 0) throw new UserFriendlyException("值班人不能为空");
            if (input.Date == null) throw new UserFriendlyException("值班日期不能为空");
            //清除之前保存的关联表信息
            await _repositoryDutyScheduleRltUser.DeleteAsync(x => x.DutyScheduleId == dutySchedule.Id);
            dutySchedule.Date = input.Date;
            dutySchedule.OrganizationId = input.OrganizationId;
            dutySchedule.Remark = input.Remark;
            //重新保存关联表信息
            dutySchedule.DutyScheduleRltUsers = new List<DutyScheduleRltUser>();
            foreach (var dutyScheduleRltUsers in input.DutyScheduleRltUsers)
            {
                dutySchedule.DutyScheduleRltUsers.Add(
              new DutyScheduleRltUser(_guidGenerate.Create())
              {
                  UserId = dutyScheduleRltUsers.UserId,
              }
               );
            }

            await _repositoryDutySchedule.UpdateAsync(dutySchedule);
            return ObjectMapper.Map<DutySchedule, DutyScheduleDto>(dutySchedule);
        }

        [Authorize(OaPermissions.DutySchedule.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的值班表id");
            var dutySchedule = _repositoryDutySchedule.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (dutySchedule.DutyScheduleRltUsers.Count == 0) throw new UserFriendlyException("该值班表不存在");
            await _repositoryDutySchedule.DeleteAsync(id);
            return true;
        }
    }
}
