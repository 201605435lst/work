using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.SettingManagement.MongoDB
{
    [ConnectionStringName(AbpSettingManagementDbProperties.ConnectionStringName)]
    public interface ISettingManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Setting> Settings { get; }
    }
}