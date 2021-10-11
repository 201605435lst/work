using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.PermissionManagement.EntityFrameworkCore
{
    [ConnectionStringName(SnAbpPermissionManagementDbProperties.ConnectionStringName)]
    public interface IPermissionManagementDbContext : IEfCoreDbContext
    {
        DbSet<PermissionGrant> PermissionGrants { get; set; }
    }
}