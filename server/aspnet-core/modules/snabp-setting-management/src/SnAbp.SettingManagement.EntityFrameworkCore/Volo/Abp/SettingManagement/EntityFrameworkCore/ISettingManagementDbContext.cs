using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.SettingManagement.EntityFrameworkCore
{
    [ConnectionStringName(AbpSettingManagementDbProperties.ConnectionStringName)]
    public interface ISettingManagementDbContext : IEfCoreDbContext
    {
        DbSet<Setting> Settings { get; set; }
    }
}