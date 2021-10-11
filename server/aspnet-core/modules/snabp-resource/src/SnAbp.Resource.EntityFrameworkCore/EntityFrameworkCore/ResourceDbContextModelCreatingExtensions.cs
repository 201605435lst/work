using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Settings;
using SnAbp.Common.Entities;
using SnAbp.Resource.Entities;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Settings;
using SnAbp.Common.Settings;
using SnAbp.Identity;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;
using Terminal = SnAbp.Resource.Entities.Terminal;
using SnAbp.File.Settings;

namespace SnAbp.Resource.EntityFrameworkCore
{
    public static class ResourceDbContextModelCreatingExtensions
    {
        public static void ConfigureResource(
            this ModelBuilder builder,
            Action<ResourceModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ResourceModelBuilderConfigurationOptions(
                ResourceDbProperties.DbTablePrefix,
                ResourceDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);


            #region 依赖模块
            builder.Entity<Area>(b =>
            {
                b.ToTable(CommonSettings.DbTablePrefix + nameof(Area), CommonSettings.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), SystemSettings.DbSchema);
            });

            builder.Entity<InstallationSite>(b =>
            {
                b.ToTable(BasicSettings.DbTablePrefix + nameof(InstallationSite), BasicSettings.DbSchema);
            });

            builder.Entity<Railway>(b =>
            {
                b.ToTable(BasicSettings.DbTablePrefix + nameof(Railway), BasicSettings.DbSchema);
            });

            builder.Entity<Station>(b =>
            {
                b.ToTable(BasicSettings.DbTablePrefix + nameof(Station), BasicSettings.DbSchema);
            });

            builder.Entity<ProductCategory>(b =>
            {
                b.ToTable(StdBasicSettings.DbTablePrefix + nameof(ProductCategory), StdBasicSettings.DbSchema);
            });

            builder.Entity<ComponentCategory>(b =>
            {
                b.ToTable(StdBasicSettings.DbTablePrefix + nameof(ComponentCategory), StdBasicSettings.DbSchema);
            });

            builder.Entity<MVDProperty>(b =>
            {
                b.ToTable(StdBasicSettings.DbTablePrefix + nameof(MVDProperty), StdBasicSettings.DbSchema);
            });

            builder.Entity<MVDCategory>(b =>
            {
                b.ToTable(StdBasicSettings.DbTablePrefix + nameof(MVDCategory), StdBasicSettings.DbSchema);
            });

            builder.Entity<Manufacturer>(b =>
            {
                b.ToTable(StdBasicSettings.DbTablePrefix + nameof(Manufacturer), StdBasicSettings.DbSchema);
            });

            builder.Entity<SnAbp.File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(SnAbp.File.Entities.File), FileSettings.DbSchema);
            });

            builder.Entity<SnAbp.File.Entities.FileRltTag>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(SnAbp.File.Entities.FileRltTag), FileSettings.DbSchema);
            });

            builder.Entity<SnAbp.File.Entities.Tag>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(SnAbp.File.Entities.Tag), FileSettings.DbSchema);
            });

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "Users", SnAbpIdentityDbProperties.DbSchema);
            });
            #endregion


            #region 当前模块
            builder.Entity<CableCore>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(CableCore), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<CableExtend>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(CableExtend), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<Equipment>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Equipment), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<EquipmentGroup>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentGroup), options.Schema);
                b.HasIndex(x => x.Name).IsUnique();
                b.ConfigureByConvention();
            });

            builder.Entity<EquipmentProperty>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentProperty), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<EquipmentRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentRltFile), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<EquipmentRltOrganization>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentRltOrganization), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<EquipmentServiceRecord>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentServiceRecord), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<StoreEquipment>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StoreEquipment), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<StoreEquipmentTest>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StoreEquipmentTest), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<StoreEquipmentTestRltEquipment>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StoreEquipmentTestRltEquipment), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<StoreEquipmentTransfer>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StoreEquipmentTransfer), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });

            builder.Entity<StoreEquipmentTransferRltEquipment>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StoreEquipmentTransferRltEquipment), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<StoreHouse>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StoreHouse), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<Terminal>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Terminal), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<TerminalLink>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(TerminalLink), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<TerminalBusinessPath>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(TerminalBusinessPath), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<TerminalBusinessPathNode>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(TerminalBusinessPathNode), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<CableLocation>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(CableLocation), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ComponentRltQRCode>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ComponentRltQRCode), options.Schema);
            });
            builder.Entity<ComponentTrackRecord>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ComponentTrackRecord), options.Schema);
            });
            builder.Entity<OrganizationRltLayer>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(OrganizationRltLayer), options.Schema);
            });
            #endregion
        }
    }
}