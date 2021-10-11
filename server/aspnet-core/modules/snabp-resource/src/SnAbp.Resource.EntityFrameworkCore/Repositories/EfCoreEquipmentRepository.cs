/**********************************************************************
*******命名空间： SnAbp.Resource.Repositories
*******类 名 称： EfCoreEquipmentRepository
*******类 说 明： 设备管理仓储自定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/14 19:19:06
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.StdBasic.Entities;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Terminal = SnAbp.Resource.Entities.Terminal;

namespace SnAbp.Resource.Repositories
{
    public class EfCoreEquipmentRepository : EfCoreRepository<IResourceDbContext, Equipment, Guid>, IEquipmentRepository
    {
        public EfCoreEquipmentRepository(IDbContextProvider<IResourceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 通过父级设备名称获取构建编码
        /// </summary>
        /// <param name="parentName">父级设备名称</param>
        /// <param name="installationName">安装位置名称</param>
        /// <returns></returns>
        public async Task<Guid?> GetComponentCodeIdByEquipmentName(string parentName, string installationName)
        {
            var installation = await DbContext.Set<InstallationSite>()
                .FirstOrDefaultAsync(a => a.Name == installationName);
            if (installation == null) return null;
            var equipment = await
                DbSet.FirstOrDefaultAsync(a => a.Name == parentName && a.InstallationSiteId == installation.Id);
            return equipment?.ComponentCategoryId;
        }

        /// <summary>
        /// 根据分组名称获取设备的分组id
        /// </summary>
        /// <param name="groupName">设备分组名称</param>
        /// <returns></returns>
        public async Task<Guid?> GetEquipmentGroupId(string groupName)
        {
            if (groupName.IsNullOrEmpty()) return null;
            return (await DbContext.Set<EquipmentGroup>()
                .FirstOrDefaultAsync(a => a.Name == groupName))?.Id;
        }

        /// <summary>
        /// 补全设备信息
        /// </summary>
        /// <param name="name">设备的名称</param>
        /// <param name="installationName">安装位置</param>
        /// <returns></returns>
        public async Task<Equipment> PerfectEquipment(Equipment equipment, string parentName)
        {
            var install = await DbContext.Set<InstallationSite>()
                .FirstOrDefaultAsync(a => a.Id == equipment.InstallationSiteId);
            if (install != null)
            {
                equipment.OrganizationId = install.OrganizationId;
            }
            var pid = (await DbSet.FirstOrDefaultAsync(a => a.Name == parentName && a.InstallationSiteId == install.Id))?.Id;
            equipment.ParentId = pid;
           
            return equipment;
        }

        /// <summary>
        /// 判断电缆配信数据是否存在
        /// </summary>
        /// <returns></returns>
        public async Task<bool> HasCableWiring(CableWiringModel model)
        {
            // 设备地址+设备名称+设备端子名称确定一个端子
            // A设备
            var equipmentAGroup = await DbContext.Set<EquipmentGroup>()
               .FirstOrDefaultAsync(a => a.Name == model.EquipmentAGroupName);
            if (equipmentAGroup == null) return false;
            var equipmentA = await DbSet.FirstOrDefaultAsync(a =>
                a.Name == model.EquipmentAName && a.GroupId == equipmentAGroup.Id);
            if (equipmentA == null) return false;
            var equipmentATerminal = await DbContext.Set<Terminal>()
                .FirstOrDefaultAsync(a => a.Name == model.EquipmentATerminalName && a.EquipmentId == equipmentA.Id);
            if (equipmentATerminal == null) return false;

            // B设备
            var equipmentBGroup = await DbContext.Set<EquipmentGroup>()
               .FirstOrDefaultAsync(a => a.Name == model.EquipmentBGroupName);
            if (equipmentBGroup == null) return false;

            var equipmentB = await DbSet.FirstOrDefaultAsync(a =>
                a.Name == model.EquipmentBName && a.GroupId == equipmentBGroup.Id);
            if (equipmentB == null) return false;
            var equipmentBTerminal = await DbContext.Set<Terminal>()
                .FirstOrDefaultAsync(a => a.Name == model.EquipmentBTerminalName && a.EquipmentId == equipmentB.Id);
            if (equipmentBTerminal == null) return false;

            var terminalLink = await DbContext.Set<TerminalLink>()
                .FirstOrDefaultAsync(a =>
                    (a.TerminalAId == equipmentATerminal.Id && a.TerminalBId == equipmentBTerminal.Id) ||
                    (a.TerminalAId == equipmentBTerminal.Id && a.TerminalBId == equipmentATerminal.Id));
            return terminalLink != null;
        }

        /// <summary>
        /// 根据条件判断是否存在设备信息
        /// </summary>
        /// <param name="name">设备名称</param>
        /// <param name="installationName">安装位置名称</param>
        /// <returns></returns>
        public async Task<bool> HasEquipment(string name, string installationName)
        {
            // 安装位置是否存在
            var installation = await DbContext.Set<InstallationSite>()
                .FirstOrDefaultAsync(a => a.Name == installationName);
            if (installation == null)
            {
                return false;
            }
            return await DbSet.AnyAsync(a => a.Name == name && a.InstallationSiteId == installation.Id);
        }

        /// <summary>
        /// 根据设备分组名称进行判断
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="installName"></param>
        /// <returns></returns>
        public async Task<Guid?> GetEquipmentGuid(string equipmentName, string groupName)
        {
            if (equipmentName.IsNullOrEmpty() || groupName.IsNullOrEmpty())
                return null;
            var equipmentGroup = await DbContext.Set<EquipmentGroup>()
                .FirstOrDefaultAsync(a => a.Name == groupName);
            if (equipmentGroup == null) return null;
            var equipment = await DbSet.FirstOrDefaultAsync(a => a.Name == equipmentName && a.GroupId == equipmentGroup.Id);
            return equipment?.Id;
        }
    }
}
