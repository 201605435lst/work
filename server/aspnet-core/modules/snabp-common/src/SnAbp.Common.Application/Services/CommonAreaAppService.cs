using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using Volo.Abp.Application.Dtos;
using SnAbp.Common.Entities;
using SnAbp.Common.IServices;
using SnAbp.Common.Dtos;

namespace SnAbp.Common.Services
{
    public class CommonAreaAppService : CommonAppService, ICommonAreaAppService
    {
        private readonly IRepository<Area> _repository;

        public CommonAreaAppService(IRepository<Area> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 根据上一级的id获取该级下属级的所有子节点|获取所有省
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<AreaDto>> GetList(AreaSearchDto input)
        {
            var area = _repository
                .Where(x => string.IsNullOrEmpty(input.KeyWord) || (input.ParentId == 0 || input.ParentId == null) ?
                (x.Deep == int.Parse(input.KeyWord)) : (x.Deep == int.Parse(input.KeyWord) && x.ParentId == input.ParentId)).ToList();

            var result = new PagedResultDto<AreaDto>();
            result.TotalCount = area.Count();
            result.Items = ObjectMapper.Map<List<Area>, List<AreaDto>>(area.Skip(input.SkipCount).Take(area.Count()).ToList());
            return Task.FromResult(result);

        }

        /// <summary>
        /// 根据ids获取父节点以及兄弟节点
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<List<AreaDto>> GetListByIds(List<string> ids)
        {
            var areas = new List<Area>();

            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    areas.AddRange(GetParents(id, new List<Area>()));
                }
            }

            var simpleArea = areas.Distinct().ToList();//去除重复城市

            simpleArea.AddRange(_repository.Where(x => x.ParentId == null).ToList());
            return Task.FromResult(ObjectMapper.Map<List<Area>, List<AreaDto>>(simpleArea.Where(x => x.Deep == 0).ToList()));
        }

        private List<Area> GetParents(string? code, List<Area> areas)
        {
            areas.AddRange(_repository.Where(x => x.ParentId.ToString() == code));

            var area = _repository.FirstOrDefault(x => x.Code == code);

            if (area != null && area.ParentId != null)
            {
                GetParents(area.ParentId.ToString(), areas);
            }

            return areas;
        }
    }
}
