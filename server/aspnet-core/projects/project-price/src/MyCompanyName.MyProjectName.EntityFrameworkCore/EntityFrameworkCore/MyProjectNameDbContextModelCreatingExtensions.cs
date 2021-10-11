using Microsoft.EntityFrameworkCore;
using MyCompanyName.MyProjectName.Entities;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public static class MyProjectNameDbContextModelCreatingExtensions
    {
        public static void ConfigureMyProjectName(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Module>(b =>
            {
                b.ToTable(MyProjectNameConsts.DbTablePrefix + nameof(Module), MyProjectNameConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Project>(b =>
            {
                b.ToTable(MyProjectNameConsts.DbTablePrefix + nameof(Project), MyProjectNameConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<ProjectRltModule>(b =>
            {
                b.ToTable(MyProjectNameConsts.DbTablePrefix + nameof(ProjectRltModule), MyProjectNameConsts.DbSchema);
                b.ConfigureByConvention();
            });
        }
    }
}