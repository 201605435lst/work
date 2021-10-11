using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.FeatureManagement.EntityFrameworkCore
{
    [ConnectionStringName(FeatureManagementDbProperties.ConnectionStringName)]
    public interface IFeatureManagementDbContext : IEfCoreDbContext
    {
        DbSet<FeatureValue> FeatureValues { get; set; }
    }
}