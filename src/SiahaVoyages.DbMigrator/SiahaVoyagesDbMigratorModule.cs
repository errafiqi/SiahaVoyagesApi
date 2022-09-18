using SiahaVoyages.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace SiahaVoyages.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SiahaVoyagesEntityFrameworkCoreModule),
    typeof(SiahaVoyagesApplicationContractsModule)
    )]
public class SiahaVoyagesDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
