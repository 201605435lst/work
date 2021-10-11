using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel.Resolution;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.Resource.IServices;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceTerminalAppService : ResourceAppService, IResourceTerminalAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Terminal, Guid> _terminalRepository;
        private readonly IRepository<Equipment, Guid> _equipmentRepository;
        private readonly IRepository<TerminalBusinessPath, Guid> _terminalBusinessRepository;
        private readonly IRepository<TerminalBusinessPathNode, Guid> _terminalBusinessPathNodeRepository;

        public ResourceTerminalAppService(
            IGuidGenerator guidGenerator,
            IRepository<Terminal, Guid> terminalRepository,
            IRepository<Equipment, Guid> equipmentRepository,
            IRepository<TerminalBusinessPath, Guid> terminalBusinessRepository,
            IRepository<TerminalBusinessPathNode, Guid> terminalBusinessPathNodeRepository
            )
        {
            _guidGenerator = guidGenerator;
            _terminalRepository = terminalRepository;
            _equipmentRepository = equipmentRepository;
            _terminalBusinessRepository = terminalBusinessRepository;
            _terminalBusinessPathNodeRepository = terminalBusinessPathNodeRepository;
        }


        /// <summary>
        /// 根据设备 Id 获取端子列表
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public Task<List<TerminalDto>> GetListByEquipmentId(Guid equipmentId)
        {
            var terminals = _terminalRepository
                .WithDetails()
                .Where(x => x.EquipmentId == equipmentId)
                .OrderBy(x => x.Name)
                .ToList();

            terminals = terminals.OrderBy(x =>
            {
                string str = x.Name;
                List<string> segs = str.Split(new char[] { 'N', 'W', '-' }).ToList();
                segs.RemoveAll(x => string.IsNullOrEmpty(x));
                string result = "";
                segs.ForEach(x =>
                {
                    result += x.PadLeft(3, '0');
                });
                return result;
            }).ToList();

            // 端子所在设备id
            var equipmentIds = new List<Guid>();
            equipmentIds.AddRange(terminals.SelectMany(t => t.TerminalLinkAs.Select(link => link.TerminalB.EquipmentId)).ToList());
            equipmentIds.AddRange(terminals.SelectMany(t => t.TerminalLinkBs.Select(link => link.TerminalA.EquipmentId)).ToList());
            equipmentIds = equipmentIds.Distinct().ToList();

            var terminalDtos = ObjectMapper.Map<List<Terminal>, List<TerminalDto>>(terminals);
            var equipments = _equipmentRepository
                .WithDetails(x => x.Group)
                .Where(x => equipmentIds.Contains(x.Id))
                .ToList();
            var equipmentDtos = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(equipments);



            var cableCoreIds = new List<Guid>();
            var terminalIds = terminalDtos.Select(x => x.Id).ToList();

            foreach (var dto in terminalDtos)
            {

                foreach (var linkDto in dto.TerminalLinkAs)
                {
                    linkDto.TerminalB.Equipment = equipmentDtos.Find(x => x.Id == linkDto.TerminalB.EquipmentId);
                    linkDto.TerminalB.Equipment.Terminals = null;
                }

                foreach (var linkDto in dto.TerminalLinkBs)
                {
                    linkDto.TerminalA.Equipment = equipmentDtos.Find(x => x.Id == linkDto.TerminalA.EquipmentId);
                    linkDto.TerminalA.Equipment.Terminals = null;
                }

                dto.TerminalLinks = new List<TerminalLinkDto>();
                dto.TerminalLinks.AddRange(dto.TerminalLinkAs);
                dto.TerminalLinks.AddRange(dto.TerminalLinkBs);

                dto.TerminalLinks.ForEach(item =>
                {
                    if (item.TerminalAId != null && item.TerminalAId == dto.Id && item.TerminalB != null)
                    {
                        item.TargetTerminal = item.TerminalB;
                    }
                    else if (item.TerminalBId != null && item.TerminalBId == dto.Id && item.TerminalA != null)
                    {
                        item.TargetTerminal = item.TerminalA;
                    }
                });

                dto.TerminalLinks = dto.TerminalLinks
                    .OrderBy(x => x.TargetTerminal.Equipment.Name)
                    .ThenBy(x =>
                        {
                            string str = x.TargetTerminal.Name;
                            List<string> segs = str.Split(new char[] { 'N', 'W', '-' }).ToList();
                            segs.RemoveAll(x => string.IsNullOrEmpty(x));
                            string result = "";
                            segs.ForEach(x =>
                            {
                                result += x.PadLeft(3, '0');
                            });
                            return result;
                        })
                    .ToList();

                dto.TerminalLinkAs = null;
                dto.TerminalLinkBs = null;
                dto.TerminalLinks.ForEach(link =>
                {
                    if (link.CableCoreId != null)
                    {
                        cableCoreIds.Add(link.CableCoreId.Value);
                    }
                });
            }

            var businessNodes = _terminalBusinessPathNodeRepository
                .WithDetails(x => x.TerminalBusinessPath)
                .Where(x => cableCoreIds.Contains(x.CableCoreId.Value) || terminalIds.Contains(x.TerminalId))
                .ToList();
            var businessNodeDtos = ObjectMapper.Map<List<TerminalBusinessPathNode>, List<TerminalBusinessPathNodeDto>>(businessNodes);


            // 业务数据
            foreach (var dto in terminalDtos)
            {
                dto.TerminalLinks.ForEach(item =>
                {
                    var paths = businessNodeDtos
                    .Where(x => x.TerminalId == dto.Id || (x.CableCoreId != null && x.CableCoreId == item.CableCoreId))
                    .Select(x => x.TerminalBusinessPath)
                    .ToList();

                    item.TerminalBusinessPaths = new List<TerminalBusinessPathDto>();
                    paths.ForEach(path =>
                    {
                        if (item.TerminalBusinessPaths != null && item.TerminalBusinessPaths.Find(x => x.Id == path.Id) == null)
                        {
                            item.TerminalBusinessPaths.Add(new TerminalBusinessPathDto()
                            {
                                Id = path.Id,
                                Name = path.Name
                            });
                        }
                    });
                });
            }

            return Task.FromResult(terminalDtos);
        }
    }
}
