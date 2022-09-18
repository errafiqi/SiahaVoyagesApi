using SiahaVoyages.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SiahaVoyages.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SiahaVoyagesController : AbpControllerBase
{
    protected SiahaVoyagesController()
    {
        LocalizationResource = typeof(SiahaVoyagesResource);
    }
}
