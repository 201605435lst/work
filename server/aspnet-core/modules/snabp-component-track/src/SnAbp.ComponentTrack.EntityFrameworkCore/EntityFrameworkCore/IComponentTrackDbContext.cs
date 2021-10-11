using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    [ConnectionStringName(ComponentTrackDbProperties.ConnectionStringName)]
    public interface IComponentTrackDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        //DbSet<TrackProcess> TrackProcess { get; set; }
        //DbSet<TrackProcessRltNode> TrackProcessRltNode { get; set; }
    }
}