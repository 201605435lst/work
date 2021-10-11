using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceTerminalLinkAppService : ResourceAppService, IResourceTerminalLinkAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<TerminalLink, Guid> _terminalLinkRepository;

        public ResourceTerminalLinkAppService(
            IGuidGenerator guidGenerator,
            IRepository<TerminalLink, Guid> terminalLinkRepository)
        {
            _guidGenerator = guidGenerator;
            _terminalLinkRepository = terminalLinkRepository;
        }


        /// <summary>
        /// 根据端子 Id 获取连接关系列表
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        public Task<List<TerminalLinkDto>> GetListByTerminalId(Guid terminalId)
        {
            var list = _terminalLinkRepository
              .WithDetails()
              .Where(x => x.TerminalAId == terminalId || x.TerminalBId == terminalId)
              .ToList();

            return Task.FromResult(ObjectMapper.Map<List<TerminalLink>, List<TerminalLinkDto>>(list));
        }


        /// <summary>
        /// 根据设备 Id 获取连接关系列表
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public Task<List<TerminalLinkDto>> GetListByEquipmentId(Guid equipmentId)
        {
            var list = _terminalLinkRepository
              .WithDetails(
                m => m.TerminalA,
                m => m.TerminalB,
                m => m.CableCore
              )
              .Where(m =>
                m.TerminalA.EquipmentId == equipmentId ||
                m.TerminalB.EquipmentId == equipmentId
              )
              .ToList();

            return Task.FromResult(ObjectMapper.Map<List<TerminalLink>, List<TerminalLinkDto>>(list));
        }
    }
}
