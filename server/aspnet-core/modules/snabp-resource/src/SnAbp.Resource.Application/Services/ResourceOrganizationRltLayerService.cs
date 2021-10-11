using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
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
    public class ResourceOrganizationRltLayerService : ResourceAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<OrganizationRltLayer, Guid> _organizationRltLayerRepository;

        public ResourceOrganizationRltLayerService(
            IGuidGenerator guidGenerator,
            IRepository<OrganizationRltLayer, Guid> organizationRltLayerRepository)
        {
            _guidGenerator = guidGenerator;
            _organizationRltLayerRepository = organizationRltLayerRepository;
        }


        /// <summary>
        /// 根据组织机构id获取图层id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<List<Guid>> GetLayerIdsByOrganizationId(Guid organizationId)
        {
            var layerIds = _organizationRltLayerRepository.WhereIf(organizationId != Guid.Empty && organizationId != null, x => x.OrganizationId == organizationId)
                                                          .Select(y => y.LayerId)
                                                          .ToList();

            return Task.FromResult(layerIds);
        }


        /// <summary>
        /// 组织机构关联图层
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(OrganizationRltLayerCreateDto input)
        {
            try
            {
                await _organizationRltLayerRepository.DeleteAsync(x => x.OrganizationId == input.OrganizationId);
                if (input.LayerIds.Count > 0)
                {
                    foreach (var layerId in input.LayerIds)
                    {
                        var organizationRltLayer = new OrganizationRltLayer(_guidGenerator.Create())
                        {
                            OrganizationId = input.OrganizationId,
                            LayerId = layerId,
                        };
                        await _organizationRltLayerRepository.InsertAsync(organizationRltLayer);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }

        }
    }
}
