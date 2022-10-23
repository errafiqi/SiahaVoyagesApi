using SiahaVoyages.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SiahaVoyages.Permissions;

public class SiahaVoyagesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SiahaVoyagesPermissions.GroupName);
        //Define your own permissions here. Example:
        myGroup.AddPermission(SiahaVoyagesPermissions.IsAdmin, L("Permission:IsAdmin"));
        myGroup.AddPermission(SiahaVoyagesPermissions.IsBackOffice, L("Permission:IsBackOffice"));
        myGroup.AddPermission(SiahaVoyagesPermissions.IsClient, L("Permission:IsClient"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SiahaVoyagesResource>(name);
    }
}
