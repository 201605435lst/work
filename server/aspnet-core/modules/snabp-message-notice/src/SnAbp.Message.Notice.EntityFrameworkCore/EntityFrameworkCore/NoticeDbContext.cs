using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.Notice.EntityFrameworkCore
{
    [ConnectionStringName(NoticeDbProperties.ConnectionStringName)]
    public class NoticeDbContext : AbpDbContext<NoticeDbContext>, INoticeDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public NoticeDbContext(DbContextOptions<NoticeDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureNotice();
        }

        public DbSet<Entities.Notice> Notice { get; set; }
    }
}