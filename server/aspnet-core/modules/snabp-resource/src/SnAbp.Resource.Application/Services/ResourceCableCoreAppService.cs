using Microsoft.AspNetCore.Authorization;

using Org.BouncyCastle.Math.EC.Rfc7748;
using Org.BouncyCastle.X509.Store;

using RestSharp.Serializers;

using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices.Equipment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceCableCoreAppService : ResourceAppService, IResourceCableCoreAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<CableCore, Guid> _cableCoreRespository;
        private readonly IRepository<TerminalLink, Guid> _terminalLinkRespository;
        private readonly IRepository<TerminalBusinessPath, Guid> _terminalBusinessRepository;
        private readonly IRepository<TerminalBusinessPathNode, Guid> _terminalBusinessPathNodeRepository;

        public ResourceCableCoreAppService(
                IGuidGenerator guidGenerator,
                IRepository<CableCore, Guid> cableLocationRespository,
                IRepository<TerminalLink, Guid> terminalLinkRespository,
                IRepository<TerminalBusinessPath, Guid> terminalBusinessRepository,
                IRepository<TerminalBusinessPathNode, Guid> terminalBusinessPathNodeRepository
            )
        {
            _guidGenerator = guidGenerator;
            _cableCoreRespository = cableLocationRespository;
            _terminalLinkRespository = terminalLinkRespository;
            _terminalBusinessRepository = terminalBusinessRepository;
            _terminalBusinessPathNodeRepository = terminalBusinessPathNodeRepository;
        }


        /// <summary>
        /// 获取列表
        /// 
        /// </summary>
        /// <param name="cableId"></param>
        /// <returns></returns>
        public Task<List<CableCoreDto>> GetList(Guid cableId)
        {
            if (Guid.Empty == cableId || cableId == null)
            {
                throw new UserFriendlyException("电缆 Id 不能为空");
            }

            var list = _cableCoreRespository.Where(x => x.CableId == cableId).OrderBy(x => x.Name).ToList();
            var coreIds = list.Select(x => x.Id).ToList();

            // 查询电缆芯业务
            var businessNodes = _terminalBusinessPathNodeRepository
                .WithDetails(x => x.TerminalBusinessPath)
                .Where(x => coreIds.Contains(x.CableCoreId.Value)).ToList();
            var businessNodeDtos = ObjectMapper.Map<List<TerminalBusinessPathNode>, List<TerminalBusinessPathNodeDto>>(businessNodes);


            var dtos = ObjectMapper.Map<List<CableCore>, List<CableCoreDto>>(list);
            foreach (var item in dtos)
            {
                var terminal = _terminalLinkRespository
                    .WithDetails(
                    a => a.TerminalA,
                    a => a.TerminalB,
                    a => a.TerminalA.Equipment,
                    a => a.TerminalB.Equipment,
                     a => a.TerminalA.Equipment.Group,
                    a => a.TerminalB.Equipment.Group
                    )
                    .FirstOrDefault(a => a.CableCoreId == item.Id);
                item.TerminalLinkId = terminal.Id;
                item.BusinessFunction = terminal.BusinessFunction;
                item.TerminalAName = terminal.TerminalA.Name;
                item.TerminalBName = terminal.TerminalB.Name;
                item.EquipmentAName = terminal.TerminalA.Equipment.Name;
                item.EquipmentBName = terminal.TerminalB.Equipment.Name;
                item.EquipmentAGroupName = terminal.TerminalA.Equipment.Group.Name;
                item.EquipmentBGroupName = terminal.TerminalB.Equipment.Group.Name;
                item.TerminalBusinessPaths = businessNodeDtos.Where(x => x.CableCoreId == item.Id).Select(x => x.TerminalBusinessPath).ToList();
            }
            return Task.FromResult(dtos);
        }

        public async Task<bool> UpdateCableCore(CableCoreUpdateDto input)
        {
            var cableCore = await _cableCoreRespository.GetAsync(input.Id);
            if (cableCore != null)
            {
                cableCore.Type = input.Type;
                await _cableCoreRespository.UpdateAsync(cableCore);
            }
            return true;
        }


        /// <summary>
        /// 更新业务类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTerminalLink(TerminalLinkUpdateDto input)
        {
            var terminalLink = await _terminalLinkRespository.GetAsync(input.TerminalLinkId);
            if (terminalLink != null)
            {
                terminalLink.BusinessFunction = input.BusinessFunction;
                await _terminalLinkRespository.UpdateAsync(terminalLink);
            }
            return true;
        }
    }
}