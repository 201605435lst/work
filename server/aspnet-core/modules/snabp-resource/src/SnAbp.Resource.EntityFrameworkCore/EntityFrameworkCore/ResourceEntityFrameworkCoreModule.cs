using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Resource.Entities;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Resource.EntityFrameworkCore
{
    [DependsOn(
        typeof(ResourceDomainModule),
        typeof(SnAbp.Identity.EntityFrameworkCore.SnAbpIdentityEntityFrameworkCoreModule),
        typeof(SnAbp.Basic.EntityFrameworkCore.BasicEntityFrameworkCoreModule)
    )]
    public class ResourceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ResourceDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */

                options.AddDefaultRepositories<IResourceDbContext>(true);

                options.Entity<StoreHouse>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.Children)
                   .Include(x => x.Area).ThenInclude(s => s.Parent).ThenInclude(v => v.Parent).ThenInclude(b => b.Parent)
                );

                options.Entity<StoreEquipmentTransfer>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.StoreHouse).ThenInclude(c => c.Organization)
                   .Include(x => x.User)
                   .Include(x => x.StoreEquipmentTransferRltEquipments).ThenInclude(s => s.StoreEquipment).ThenInclude(w => w.ProductCategory).ThenInclude(h => h.Parent).ThenInclude(r => r.Parent).ThenInclude(t => t.Parent).ThenInclude(y => y.Parent).ThenInclude(u => u.Parent)
                   .Include(x => x.StoreEquipmentTransferRltEquipments).ThenInclude(s => s.StoreEquipment).ThenInclude(w => w.Manufacturer)
                );

                options.Entity<Equipment>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.Children)
                   .Include(x => x.EquipmentRltOrganizations).ThenInclude(r => r.Organization)
                   .Include(x => x.ProductCategory)
                   .Include(x => x.ComponentCategory).ThenInclude(s => s.Parent)
                   .Include(x => x.Manufacturer)
                   .Include(x => x.Organization)
                  .Include(x => x.Group)
                   .Include(x => x.ComponentRltQRCodes)
                   .Include(x => x.InstallationSite)
                   .Include(x => x.EquipmentProperties)
                   .Include(x => x.EquipmentRltFiles).ThenInclude(s => s.File).ThenInclude(m => m.Tags).ThenInclude(n => n.Tag)
                );

                options.Entity<EquipmentRltFile>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.File).ThenInclude(m => m.Tags).ThenInclude(n => n.Tag)
                );

                options.Entity<StoreEquipment>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.ProductCategory).ThenInclude(s => s.Parent).ThenInclude(s => s.Parent).ThenInclude(s => s.Parent).ThenInclude(s => s.Parent).ThenInclude(s => s.Parent).ThenInclude(s => s.Parent).ThenInclude(s => s.Parent)
                   .Include(x => x.ComponentCategory).ThenInclude(s => s.Parent)
                   .Include(x => x.Manufacturer)
                   .Include(x => x.StoreHouse)
                   //.Include(x => x.StoreEquipmentTransferRltEquipments).ThenInclude(s => s.StoreEquipmentTransfer).ThenInclude(s => s.User)
                   //.Include(x => x.StoreEquipmentTransferRltEquipments).ThenInclude(s => s.StoreEquipmentTransfer).ThenInclude(s => s.StoreHouse)
                   .Include(x => x.EquipmentServiceRecords)
                   .Include(x => x.Creator)
                //.Include(x => x.StoreEquipmentTestRltEquipments).ThenInclude(s => s.StoreEquipmentTest).ThenInclude(s => s.Tester)
                );

                options.Entity<StoreEquipmentTest>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.Organization)
                   .Include(x => x.StoreEquipmentTestRltEquipments).ThenInclude(r => r.StoreEquipment).ThenInclude(st => st.ProductCategory)
                );

                options.Entity<Terminal>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(t => t.TerminalLinkAs).ThenInclude(links => links.TerminalB)
                    .Include(t => t.TerminalLinkAs).ThenInclude(links => links.CableCore.Cable.Group)
                    .Include(t => t.TerminalLinkBs).ThenInclude(links => links.TerminalA)
                    .Include(t => t.TerminalLinkBs).ThenInclude(links => links.CableCore.Cable.Group)
                );

                options.Entity<TerminalLink>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(rlt => rlt.TerminalA).ThenInclude(terminal => terminal.Equipment).ThenInclude(equip => equip.Terminals).ThenInclude(m => m.TerminalLinkAs)
                   .Include(rlt => rlt.TerminalA).ThenInclude(terminal => terminal.Equipment).ThenInclude(equip => equip.Terminals).ThenInclude(m => m.TerminalLinkBs)
                   .Include(rlt => rlt.TerminalB).ThenInclude(terminal => terminal.Equipment).ThenInclude(equip => equip.Terminals).ThenInclude(m => m.TerminalLinkAs)
                   .Include(rlt => rlt.TerminalB).ThenInclude(terminal => terminal.Equipment).ThenInclude(equip => equip.Terminals).ThenInclude(m => m.TerminalLinkBs)
                );


                options.Entity<EquipmentGroup>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.Organization)
                   .Include(x => x.Children)
                   .Include(x => x.Parent)
                );
                options.Entity<ComponentRltQRCode>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.GenerateEquipment)
                    .Include(x => x.InstallationEquipment)
                    .Include(y => y.GenerateEquipment).ThenInclude(x => x.ComponentCategory).ThenInclude(s => s.Parent)
                );
                options.Entity<ComponentTrackRecord>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.ComponentRltQRCode)
                    .Include(x => x.User)
                );
                options.Entity<TerminalBusinessPath>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.Nodes).ThenInclude(x => x.CableCore.Cable.Group)
                   .Include(x => x.Nodes).ThenInclude(x => x.Terminal)
                );
            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();
        }
    }
}