using SnAbp.Technology.Dtos;
using SnAbp.Technology.Entities;
using SnAbp.Technology.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using System.Linq;
using SnAbp.Technology.Enums;
using SnAbp.Technology.enums;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using SnAbp.Identity;

namespace SnAbp.Technology.Services
{
    public class ConstructInterfaceInfoAppService : TechnologyAppService, IConstructInterfaceInfoAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IRepository<ConstructInterfaceInfo, Guid> _constructInterfaceInfoResposotory;
        private readonly IRepository<ConstructInterfaceInfoRltMarkFile, Guid> _constructInterfaceInfoRltMarkFileResposotory;
        private readonly IRepository<ConstructInterface, Guid> _constructInterfaceResposotory;
        public ConstructInterfaceInfoAppService(
            IGuidGenerator guidGenerator,
            IOrganizationRepository organizationRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<ConstructInterface, Guid> constructInterfaceResposotory,
            IRepository<ConstructInterfaceInfo, Guid> constructInterfaceInfoResposotory,
            IRepository<ConstructInterfaceInfoRltMarkFile, Guid> constructInterfaceInfoRltMarkFileResposotory
            )
        {
            _organizationRepository = organizationRepository;
            _httpContextAccessor = httpContextAccessor;
            _constructInterfaceResposotory = constructInterfaceResposotory;
            _guidGenerator = guidGenerator;
            _constructInterfaceInfoResposotory = constructInterfaceInfoResposotory;
            _constructInterfaceInfoRltMarkFileResposotory = constructInterfaceInfoRltMarkFileResposotory;

        }
        /// <summary>
        /// 接口标记
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(ConstructInterfaceInfoCreateDto input)
        {
            var constructInterfaceInfo = new ConstructInterfaceInfo();
            ObjectMapper.Map(input, constructInterfaceInfo);
            //1、保存基本信息
            constructInterfaceInfo.SetId(_guidGenerator.Create());
            ///保存附件
            constructInterfaceInfo.MarkFiles = new List<ConstructInterfaceInfoRltMarkFile>();
            foreach (var markFiles in input.MarkFiles)
            {
                constructInterfaceInfo.MarkFiles.Add(
                    new ConstructInterfaceInfoRltMarkFile(_guidGenerator.Create())
                    {
                        MarkFileId = markFiles.MarkFileId,
                        Type = InterfaceFlagType.InterfaceFlag,
                    });
            }
            await _constructInterfaceInfoResposotory.InsertAsync(constructInterfaceInfo);
            var constructInterface = _constructInterfaceResposotory.WithDetails().Where(x => x.Id == input.ConstructInterfaceId).FirstOrDefault();
            constructInterface.MarkType = input.MarkType;
            await _constructInterfaceResposotory.UpdateAsync(constructInterface);
            return true;
        }
        public Task<ConstructInterfaceInfoDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var constructInterfaceInfo = _constructInterfaceInfoResposotory.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (constructInterfaceInfo == null) throw new UserFriendlyException("当前数据不存在");
            var res = ObjectMapper.Map<ConstructInterfaceInfo, ConstructInterfaceInfoDto>(constructInterfaceInfo);
            return Task.FromResult(res);
        }

        public Task<List<ConstructInterfaceInfoDto>> GetInterfanceReform(ConstructInterfaceInfoReformSimpleDto input)
        {
            if (input.ConstructInterfaceId == null || input.ConstructInterfaceId == Guid.Empty) throw new UserFriendlyException("请确定要获取的数据");
            var constructInterfaceInfo = _constructInterfaceInfoResposotory.WithDetails().Where(x => x.ConstructInterfaceId == input.ConstructInterfaceId).OrderBy(y=>y.MarkDate);
            var res = ObjectMapper.Map<List<ConstructInterfaceInfo>, List<ConstructInterfaceInfoDto>>(constructInterfaceInfo.ToList());
            return Task.FromResult(res);
        }

        /// <summary>
        /// 接口整改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPost]
        public async Task<bool> InterfanceReform(ConstructInterfaceInfoReformDto input)
        {
            if (input.ConstructInterfaceId == null || input.ConstructInterfaceId == Guid.Empty) throw new UserFriendlyException("请确定要整改的数据");
            var constructInterfaceInfo = _constructInterfaceInfoResposotory.WithDetails().Where(x => x.ConstructInterfaceId == input.ConstructInterfaceId && x.MarkType == MarkType.NoQualified).FirstOrDefault();
            if (constructInterfaceInfo == null) throw new UserFriendlyException("当前数据不存在");
            constructInterfaceInfo.ReformDate = input.ReformDate;
            constructInterfaceInfo.ReformerId = input.ReformerId;
            constructInterfaceInfo.ReformExplain = input.ReformExplain;
            /////保存附件
            //constructInterfaceInfo.MarkFiles = new List<ConstructInterfaceInfoRltMarkFile>();
            foreach (var markFiles in input.MarkFiles)
            {
                var constructInterfaceInfoRltMarkFile = new ConstructInterfaceInfoRltMarkFile(_guidGenerator.Create())
                {
                    ConstructInterfaceInfoId = constructInterfaceInfo.Id,
                    MarkFileId = markFiles.MarkFileId,
                    Type = InterfaceFlagType.InterfaceFlagReform,
                };
                await _constructInterfaceInfoRltMarkFileResposotory.InsertAsync(constructInterfaceInfoRltMarkFile);
            }

            await _constructInterfaceInfoResposotory.UpdateAsync(constructInterfaceInfo);
            var constructInterface = _constructInterfaceResposotory.WithDetails().Where(x => x.Id == input.ConstructInterfaceId).FirstOrDefault();
            constructInterface.MarkType = MarkType.Qualified;
            await _constructInterfaceResposotory.UpdateAsync(constructInterface);
            return true;
        }
        /// <summary>
        /// 标记修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(ConstructInterfaceInfoUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var constructInterfaceInfo = await _constructInterfaceInfoResposotory.GetAsync(input.Id);
            if (constructInterfaceInfo == null) throw new UserFriendlyException("当前数据不存在");
            //清除保存的附件关联表信息
            await _constructInterfaceInfoRltMarkFileResposotory.DeleteAsync(x => x.ConstructInterfaceInfoId == input.Id && x.Type == InterfaceFlagType.InterfaceFlag);
            ///保存附件
            constructInterfaceInfo.MarkFiles = new List<ConstructInterfaceInfoRltMarkFile>();
            foreach (var markFiles in input.MarkFiles)
            {
                constructInterfaceInfo.MarkFiles.Add(
                    new ConstructInterfaceInfoRltMarkFile(_guidGenerator.Create())
                    {
                        MarkFileId = markFiles.MarkFileId,
                        Type = InterfaceFlagType.InterfaceFlag,
                    });
            }

            await _constructInterfaceInfoResposotory.UpdateAsync(constructInterfaceInfo);
            return true;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        [HttpGet]
        public async Task<Stream> Export(ConstructInterfaceInfoExportDto input)
        {
            //1、获取需要导出的数据
            var constructInterface = _constructInterfaceResposotory.WithDetails()
                .Where(x => x.Id == input.ConstructInterfaceId).FirstOrDefault();
            if (constructInterface == null) throw new UserFriendlyException("当前数据不存在");
            /*获取关联表的信息*/
            var constructInterfaceInfo = _constructInterfaceInfoResposotory.WithDetails().Where(x => x.ConstructInterfaceId == input.ConstructInterfaceId).ToList();
            constructInterface.ConstructInterfaceInfos = new List<ConstructInterfaceInfo>();
            constructInterface.ConstructInterfaceInfos = constructInterfaceInfo;
            // 获取当前用户所在的组织机构
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var workOrganization = "";
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var count = constructInterfaceInfo.Count;
            if (organization != null)
            {
                workOrganization = organization.Name;
            }
            var tableName = $"{constructInterface.Name}报告记录表";
            return NpoiWordExportService.SaveConstructInterfaceWordFile(constructInterface, count, currentDate, tableName, workOrganization);
        }
    }

}
