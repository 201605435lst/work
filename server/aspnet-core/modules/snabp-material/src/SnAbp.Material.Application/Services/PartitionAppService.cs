/**********************************************************************
*******命名空间： SnAbp.Material.Services
*******类 名 称： PartitionAppService
*******类 说 明： 分区管理服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/1 14:23:40
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Utils.TreeHelper;

using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Material.Services
{
    /// <summary>
    /// $$
    /// </summary>
    public class PartitionAppService : MaterialAppService, IApplicationService
    {
        IRepository<Partition, Guid> _partitions;
        IGuidGenerator _generator;
        public PartitionAppService(
            IRepository<Partition, Guid> partitions,
            IGuidGenerator generator
            )
        {
            _partitions = partitions;
            _generator = generator;
        }

        public async Task Create(PartitionCreateDto input)
        {
            // 先提交其他模块未提交的事务信息
            var model = ObjectMapper.Map<PartitionCreateDto, Partition>(input);
            model.SetId(_generator.Create());
            model.Type = input.ParentId == null ? PartitionType.Map : PartitionType.Drawing;
            await _partitions.InsertAsync(model);
        }

        public Task<PartitionDto> Get(Guid id)
        {
            var partition = _partitions.WithDetails(a => a.File, a => a.Parent).FirstOrDefault(x => x.Id == id);
            if (partition == null) throw new UserFriendlyException("更新分区不存在");
            return Task.FromResult(ObjectMapper.Map<Partition, PartitionDto>(partition));
        }

        public async Task<List<PartitionDto>> GetTreeList()
        {
            var list = _partitions.WithDetails(a => a.File, a => a.Parent).ToList();
            var dtos = ObjectMapper.Map<List<Partition>, List<PartitionDto>>(list);
            var result = GuidKeyTreeHelper<PartitionDto>.GetTree(dtos);
            return result;
        }

        public async Task<bool> Update(PartitionCreateDto input)
        {
            var model = await _partitions.GetAsync(input.Id);
            model.Description = input.Description;
            model.FileId = input.FileId;
            model.Name = input.Name;
            model.Remark = input.Remark;
            model.X = input.X;
            model.Y = input.Y;
            await _partitions.UpdateAsync(model);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (_partitions.Any(a => a.ParentId == id))
            {
                throw new UserFriendlyException("存在子分区，无法删除");
            }
            await _partitions.DeleteAsync(id);
            return true;
        }

    }


}
