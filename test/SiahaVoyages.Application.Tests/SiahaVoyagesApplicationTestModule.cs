using Volo.Abp.Modularity;

namespace SiahaVoyages;

[DependsOn(
    typeof(SiahaVoyagesApplicationModule),
    typeof(SiahaVoyagesDomainTestModule)
    )]
public class SiahaVoyagesApplicationTestModule : AbpModule
{

}
