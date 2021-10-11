using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Org.BouncyCastle.Math.EC.Rfc7748;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceTerminalBusinessAppService : ResourceAppService, IResourceTerminalBusinessAppService
    {
        private IGuidGenerator _guidGenerator;
        private IRepository<TerminalBusinessPath, Guid> _terminalBusinessPathRepository;
        private IRepository<TerminalBusinessPathNode, Guid> _terminalBusinessPathNodeRepository;
        private IRepository<Equipment, Guid> _equipmentRepository;

        public ResourceTerminalBusinessAppService(
            IGuidGenerator guidGenerator,
            IRepository<TerminalBusinessPath, Guid> terminalBusinessPathRepository,
            IRepository<TerminalBusinessPathNode, Guid> terminalBusinessPathNodeRepository,
            IRepository<Equipment, Guid> equipmentRepository
            )
        {
            _guidGenerator = guidGenerator;
            _terminalBusinessPathRepository = terminalBusinessPathRepository;
            _terminalBusinessPathNodeRepository = terminalBusinessPathNodeRepository;
            _equipmentRepository = equipmentRepository;
        }


        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TerminalBusinessPathDto> Get(Guid id)
        {
            var ent = _terminalBusinessPathRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            ent.Nodes = ent.Nodes.OrderBy(x => x.Order).ToList();
            var dto = ObjectMapper.Map<TerminalBusinessPath, TerminalBusinessPathDto>(ent);

            var equipmentIds = ent.Nodes.Select(x => x.Terminal.EquipmentId).ToList();
            var equipments = _equipmentRepository.WithDetails(x => x.Group).Where(x => equipmentIds.Contains(x.Id)).ToList();
            var equipmentDtos = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(equipments);
            equipmentDtos.ForEach(item =>
            {
                item.Terminals = null;
            });

            dto.Nodes.ForEach(item =>
            {
                item.Terminal.Equipment = equipmentDtos.Find(x => x.Id == item.Terminal.EquipmentId);
            });

            return Task.FromResult(dto);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TerminalBusinessPathDto> create(TerminalBusinessPathDto input)
        {
            var nodes = new List<TerminalBusinessPathNode>();

            // 增加新的关联关系
            input.Nodes.ForEach(item =>
            {
                nodes.Add(
                    new TerminalBusinessPathNode(_guidGenerator.Create())
                    {
                        TerminalId = item.TerminalId,
                        Order = item.Order,
                        CableCoreId = item.CableCoreId
                    }
                );
            });

            var ent = await _terminalBusinessPathRepository.InsertAsync(
                new TerminalBusinessPath(_guidGenerator.Create())
                {
                    Name = input.Name,
                    Nodes = nodes
                }
            );

            var dto = ObjectMapper.Map<TerminalBusinessPath, TerminalBusinessPathDto>(ent);
            return dto;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TerminalBusinessPathDto> update(TerminalBusinessPathDto input)
        {
            var ent = _terminalBusinessPathRepository.WithDetails().Where(x => x.Id == input.Id).FirstOrDefault();
            var nodes = new List<TerminalBusinessPathNode>();

            // 删除之前的节点关联关系
            await _terminalBusinessPathNodeRepository.DeleteAsync(x => x.TerminalBusinessPathId == input.Id);

            // 增加新的关联关系
            input.Nodes.ForEach(item =>
            {
                nodes.Add(
                    new TerminalBusinessPathNode(_guidGenerator.Create())
                    {
                        TerminalId = item.TerminalId,
                        Order = item.Order,
                        CableCoreId = item.CableCoreId
                    }
                );
            });

            await CurrentUnitOfWork.SaveChangesAsync();
            var dto = ObjectMapper.Map<TerminalBusinessPath, TerminalBusinessPathDto>(ent);
            return dto;
        }

        async public Task<bool> delete(Guid id)
        {
            await _terminalBusinessPathNodeRepository.DeleteAsync(x => x.TerminalBusinessPathId == id);
            await _terminalBusinessPathRepository.DeleteAsync(x => x.Id == id);
            return true;
        }
    }
}
