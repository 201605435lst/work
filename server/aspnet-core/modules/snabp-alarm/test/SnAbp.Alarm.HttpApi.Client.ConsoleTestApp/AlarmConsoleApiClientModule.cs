using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Alarm
{
    [DependsOn(
        typeof(AlarmHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class AlarmConsoleApiClientModule : AbpModule
    {
        
    }
}
