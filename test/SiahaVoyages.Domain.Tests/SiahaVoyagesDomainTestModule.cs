using SiahaVoyages.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SiahaVoyages;

[DependsOn(
    typeof(SiahaVoyagesEntityFrameworkCoreTestModule)
    )]
public class SiahaVoyagesDomainTestModule : AbpModule
{

}
