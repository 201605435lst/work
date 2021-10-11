using Microsoft.EntityFrameworkCore;
using SnAbp.Common.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Common.EntityFrameworkCore
{
    [ConnectionStringName(CommonDbProperties.ConnectionStringName)]
    public interface ICommonDbContext : IEfCoreDbContext
    {
        DbSet<Area> Area { get; set; }
        DbSet<QRCode> QRCode { get; set; }
    }
}