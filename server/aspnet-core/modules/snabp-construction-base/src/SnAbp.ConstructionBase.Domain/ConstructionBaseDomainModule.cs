using Volo.Abp.Modularity;

namespace SnAbp.ConstructionBase
{
	[DependsOn(
		typeof(ConstructionBaseDomainSharedModule)
	)]
	public class ConstructionBaseDomainModule : AbpModule
	{
	}
}