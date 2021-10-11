using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace SnAbp.ConstructionBase
{
	[DependsOn(
		typeof(ConstructionBaseDomainModule),
		typeof(ConstructionBaseApplicationContractsModule),
		typeof(AbpDddApplicationModule),
		// typeof(AbpFluentValidationModule), // 添加 fluent Validation 验证 
		typeof(AbpAutoMapperModule)
		
	)]
	public class ConstructionBaseApplicationModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			context.Services.AddAutoMapperObjectMapper<ConstructionBaseApplicationModule>();
			Configure<AbpAutoMapperOptions>(options =>
			{
				//  验证 entity ,dto ,createDto ,updateDto 这些字段 是否 一致 ，不一致就报错了(给他关了)
				options.AddMaps<ConstructionBaseApplicationModule>(false);
			});
		}
	}
}