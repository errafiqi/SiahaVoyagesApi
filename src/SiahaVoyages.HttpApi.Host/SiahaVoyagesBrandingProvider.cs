using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SiahaVoyages;

[Dependency(ReplaceServices = true)]
public class SiahaVoyagesBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SiahaVoyages";
}
