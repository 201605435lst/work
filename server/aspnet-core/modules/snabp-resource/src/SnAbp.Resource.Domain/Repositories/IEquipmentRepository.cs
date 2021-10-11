/**********************************************************************
*******命名空间： SnAbp.Resource.Repositories
*******接口名称： IEquipmentRepository
*******接口说明： 设备管理仓储
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/14 19:17:33
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SnAbp.Resource
{
    public interface IEquipmentRepository: IDomainService
    {
        Task<bool> HasEquipment(string name,string installationName);

        Task<Guid?> GetComponentCodeIdByEquipmentName(string parentName, string installationName);

        Task<Equipment> PerfectEquipment(Equipment equipment, string parentName);

        Task<Guid?> GetEquipmentGroupId(string groupName);

        Task<bool> HasCableWiring(CableWiringModel model);

        Task<Guid?> GetEquipmentGuid(string equipmentName, string installName);
    }
}
