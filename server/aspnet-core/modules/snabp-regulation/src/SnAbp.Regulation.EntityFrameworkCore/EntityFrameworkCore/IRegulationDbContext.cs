using Microsoft.EntityFrameworkCore;
using SnAbp.Regulation.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;



namespace SnAbp.Regulation.EntityFrameworkCore
{
    [ConnectionStringName(RegulationDbProperties.ConnectionStringName)]
    public interface IRegulationDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */


        DbSet<Institution> Institution { get; }
        DbSet<Label> Label { get; }
        DbSet<InstitutionRltAuthority> InstitutionRltAuthority { get;}
        DbSet<InstitutionRltEdition> InstitutionRltEdition { get;}
        DbSet<InstitutionRltLabel> InstitutionRltLabel { get;}
        DbSet<InstitutionRltFile> InstitutionRltFile { get; }
        DbSet<InstitutionRltFlow> InstitutionRltFlow { get; }

    }
}