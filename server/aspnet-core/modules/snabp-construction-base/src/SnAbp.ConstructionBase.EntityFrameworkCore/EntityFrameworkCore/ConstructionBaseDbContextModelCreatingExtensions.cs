using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
	public static class ConstructionBaseDbContextModelCreatingExtensions
	{
		public static void ConfigureConstructionBase(
			this ModelBuilder builder,
			Action<ConstructionBaseModelBuilderConfigurationOptions> optionsAction = null)
		{
			Check.NotNull(builder, nameof(builder));

			var options = new ConstructionBaseModelBuilderConfigurationOptions(
				ConstructionBaseDbProperties.DbTablePrefix,
				ConstructionBaseDbProperties.DbSchema
			);

			optionsAction?.Invoke(options);
			// 获取Domain 层的所有数据库实体
			// IEnumerable<Type> entities = EntityBuilderHandler.GetCurrentDomainDbEntities(typeof(ConstructionBaseDbProperties));
			// foreach (var item in entities)
			// {
			//     builder.Entity(item, b =>
			//     {
			//         b.ToTable(options.TablePrefix + nameof(item), options.Schema);
			//         b.ConfigureByConvention();
			//     });
			// }


			builder.Entity<Worker>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(Worker), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<EquipmentTeam>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(EquipmentTeam), options.Schema);
				b.ConfigureByConvention();
				b.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.TypeId);
			});
			builder.Entity<ConstructionBaseMaterial>(b =>
			{
				b.ToTable(options.TablePrefix + "Material", options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<Procedure>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(Procedure), options.Schema);
				b.ConfigureByConvention();
				// 一对一 数据字典表
				b.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.TypeId);
			});
			builder.Entity<ProcedureMaterial>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(ProcedureMaterial), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<ProcedureWorker>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(ProcedureWorker), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<ProcedureEquipmentTeam>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(ProcedureEquipmentTeam), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<ProcedureRltFile>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(ProcedureRltFile), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<SubItem>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(SubItem), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<SubItemContent>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(SubItemContent), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
				
				b.HasOne(p => p.SubItem)
					.WithOne(b => b.SubItemContent)
					.HasForeignKey<SubItemContent>(p => p.SubItemId) // 一对一要这样写 HasForeignKey<T>(p=>p.xxId) 不写泛型代码提示报错,百度下ef core 一对一
					.HasConstraintName("ForeignKey_SubItem_SubItemContent"); // 这里是 给 外键约束 取名 因为外键约束名太长,多个外键 名 重复了 ,ef core 迁移会报错…… 
			});
			builder.Entity<SubItemContentRltProcedure>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(SubItemContentRltProcedure), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<RltProcedureRltFile>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(RltProcedureRltFile), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<RltProcedureRltMaterial>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(RltProcedureRltMaterial), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<RltProcedureRltEquipmentTeam>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(RltProcedureRltEquipmentTeam), options.Schema);
				b.ConfigureByConvention();
			});

			builder.Entity<Standard>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(Standard), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<Section>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(Section), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});

			builder.Entity<RltProcedureRltWorker>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(RltProcedureRltWorker), options.Schema);
				b.ConfigureByConvention();
			});
		}
	}
}
