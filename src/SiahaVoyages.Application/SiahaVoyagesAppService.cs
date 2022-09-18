using System;
using System.Collections.Generic;
using System.Text;
using SiahaVoyages.Localization;
using Volo.Abp.Application.Services;

namespace SiahaVoyages;

/* Inherit your application services from this class.
 */
public abstract class SiahaVoyagesAppService : ApplicationService
{
    protected SiahaVoyagesAppService()
    {
        LocalizationResource = typeof(SiahaVoyagesResource);
    }
}
