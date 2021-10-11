using Microsoft.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Regulation.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Regulation.EntityFrameworkCore
{
    [ConnectionStringName(RegulationDbProperties.ConnectionStringName)]
    public class RegulationDbContext : AbpDbContext<RegulationDbContext>, IRegulationDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<Institution> Institution { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<InstitutionRltAuthority> InstitutionRltAuthority { get; set; }
        public DbSet<InstitutionRltEdition> InstitutionRltEdition { get; set; }
        public DbSet<InstitutionRltLabel> InstitutionRltLabel { get; set; }
        public DbSet<InstitutionRltFile> InstitutionRltFile { get; set; }
        public DbSet<InstitutionRltFlow> InstitutionRltFlow { get; set; }

        public RegulationDbContext(DbContextOptions<RegulationDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureRegulation();

            builder.ConfigureFile();
        }
    }
}