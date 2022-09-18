using Volo.Abp.Settings;

namespace SiahaVoyages.Settings;

public class SiahaVoyagesSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SiahaVoyagesSettings.MySetting1));
    }
}
