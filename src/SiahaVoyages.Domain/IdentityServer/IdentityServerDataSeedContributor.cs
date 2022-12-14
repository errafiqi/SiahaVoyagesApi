using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using SiahaVoyages.App;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace SiahaVoyages.IdentityServer;

public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IApiResourceRepository _apiResourceRepository;
    private readonly IApiScopeRepository _apiScopeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTenant _currentTenant;
    private readonly IRepository<IdentityRole, Guid> _roleRepository;
    private readonly IdentityRoleManager RoleManager;

    public IdentityServerDataSeedContributor(
        IClientRepository clientRepository,
        IApiResourceRepository apiResourceRepository,
        IApiScopeRepository apiScopeRepository,
        IIdentityResourceDataSeeder identityResourceDataSeeder,
        IGuidGenerator guidGenerator,
        IPermissionDataSeeder permissionDataSeeder,
        IConfiguration configuration,
        ICurrentTenant currentTenant,
        IRepository<IdentityRole, Guid> roleRepository,
        IdentityRoleManager roleManager)
    {
        _clientRepository = clientRepository;
        _apiResourceRepository = apiResourceRepository;
        _apiScopeRepository = apiScopeRepository;
        _identityResourceDataSeeder = identityResourceDataSeeder;
        _guidGenerator = guidGenerator;
        _permissionDataSeeder = permissionDataSeeder;
        _configuration = configuration;
        _currentTenant = currentTenant;
        _roleRepository = roleRepository;
        RoleManager = roleManager;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await CreateApiResourcesAsync();
            await CreateApiScopesAsync();
            await CreateClientsAsync();
            await CreateRoles();
        }
    }

    private async Task CreateRoles()
    {
        if (await _roleRepository.CountAsync(r => r.Name.Equals("driver")) == 0)
        {
            var role = new IdentityRole(_guidGenerator.Create(), "driver", null)
            {
                IsDefault = false,
                IsPublic = false
            };
            await RoleManager.CreateAsync(role);
        }
        if (await _roleRepository.CountAsync(r => r.Name.Equals("office")) == 0)
        {
            var role = new IdentityRole(_guidGenerator.Create(), "office", null)
            {
                IsDefault = false,
                IsPublic = false
            };
            await RoleManager.CreateAsync(role);
        }
        if (await _roleRepository.CountAsync(r => r.Name.Equals("client")) == 0)
        {
            var role = new IdentityRole(_guidGenerator.Create(), "client", null)
            {
                IsDefault = false,
                IsPublic = false
            };
            await RoleManager.CreateAsync(role);
        }
    }

    private async Task CreateApiScopesAsync()
    {
        await CreateApiScopeAsync("SiahaVoyages");
    }

    private async Task CreateApiResourcesAsync()
    {
        var commonApiUserClaims = new[]
        {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };

        await CreateApiResourceAsync("SiahaVoyages", commonApiUserClaims);
    }

    private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
    {
        var apiResource = await _apiResourceRepository.FindByNameAsync(name);
        if (apiResource == null)
        {
            apiResource = await _apiResourceRepository.InsertAsync(
                new ApiResource(
                    _guidGenerator.Create(),
                    name,
                    name + " API"
                ),
                autoSave: true
            );
        }

        foreach (var claim in claims)
        {
            if (apiResource.FindClaim(claim) == null)
            {
                apiResource.AddUserClaim(claim);
            }
        }

        return await _apiResourceRepository.UpdateAsync(apiResource);
    }

    private async Task<ApiScope> CreateApiScopeAsync(string name)
    {
        var apiScope = await _apiScopeRepository.FindByNameAsync(name);
        if (apiScope == null)
        {
            apiScope = await _apiScopeRepository.InsertAsync(
                new ApiScope(
                    _guidGenerator.Create(),
                    name,
                    name + " API"
                ),
                autoSave: true
            );
        }

        return apiScope;
    }

    private async Task CreateClientsAsync()
    {
        var commonScopes = new[]
        {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address",
                "SiahaVoyages"
            };

        var configurationSection = _configuration.GetSection("IdentityServer:Clients");


        //Console Test / Angular Client
        var consoleAndAngularClientId = configurationSection["SiahaVoyages_App:ClientId"];
        if (!consoleAndAngularClientId.IsNullOrWhiteSpace())
        {
            var webClientRootUrl = configurationSection["SiahaVoyages_App:RootUrl"]?.TrimEnd('/');

            await CreateClientAsync(
                name: consoleAndAngularClientId,
                clientUri: webClientRootUrl,
                scopes: commonScopes,
                grantTypes: new[] { "password", "client_credentials", "authorization_code" },
                secret: (configurationSection["SiahaVoyages_App:ClientSecret"] ?? "1q2w3e*").Sha256(),
                requireClientSecret: false,
                redirectUri: webClientRootUrl,
                postLogoutRedirectUri: webClientRootUrl,
                corsOrigins: new[] { webClientRootUrl.RemovePostFix("/") }
            );
        }



        // Swagger Client
        var swaggerClientId = configurationSection["SiahaVoyages_Swagger:ClientId"];
        if (!swaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection["SiahaVoyages_Swagger:RootUrl"].TrimEnd('/');

            await CreateClientAsync(
                name: swaggerClientId,
                clientUri: swaggerRootUrl,
                scopes: commonScopes,
                grantTypes: new[] { "authorization_code" },
                secret: configurationSection["SiahaVoyages_Swagger:ClientSecret"]?.Sha256(),
                requireClientSecret: false,
                redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                corsOrigins: new[] { swaggerRootUrl.RemovePostFix("/") }
            );
        }
    }

    private async Task<Client> CreateClientAsync(
        string name,
        IEnumerable<string> scopes,
        IEnumerable<string> grantTypes,
        string clientUri = null,
        string secret = null,
        string redirectUri = null,
        string postLogoutRedirectUri = null,
        string frontChannelLogoutUri = null,
        bool requireClientSecret = true,
        bool requirePkce = false,
        IEnumerable<string> permissions = null,
        IEnumerable<string> corsOrigins = null)
    {
        var client = await _clientRepository.FindByClientIdAsync(name);
        if (client == null)
        {
            client = await _clientRepository.InsertAsync(
                new Client(
                    _guidGenerator.Create(),
                    name
                )
                {
                    ClientName = name,
                    ClientUri = clientUri,
                    ProtocolType = "oidc",
                    Description = name,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AbsoluteRefreshTokenLifetime = 31536000, //365 days
                    AccessTokenLifetime = 31536000, //365 days
                    AuthorizationCodeLifetime = 300,
                    IdentityTokenLifetime = 300,
                    RequireConsent = false,
                    FrontChannelLogoutUri = frontChannelLogoutUri,
                    RequireClientSecret = requireClientSecret,
                    RequirePkce = requirePkce
                },
                autoSave: true
            );
        }

        if (client.ClientUri != clientUri)
        {
            client.ClientUri = clientUri;
        }

        foreach (var scope in scopes)
        {
            if (client.FindScope(scope) == null)
            {
                client.AddScope(scope);
            }
        }

        foreach (var grantType in grantTypes)
        {
            if (client.FindGrantType(grantType) == null)
            {
                client.AddGrantType(grantType);
            }
        }

        if (!secret.IsNullOrEmpty())
        {
            if (client.FindSecret(secret) == null)
            {
                client.AddSecret(secret);
            }
        }

        if (redirectUri != null)
        {
            if (client.FindRedirectUri(redirectUri) == null)
            {
                client.AddRedirectUri(redirectUri);
            }
        }

        if (postLogoutRedirectUri != null)
        {
            if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
            {
                client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
            }
        }

        if (permissions != null)
        {
            await _permissionDataSeeder.SeedAsync(
                ClientPermissionValueProvider.ProviderName,
                name,
                permissions,
                null
            );
        }

        if (corsOrigins != null)
        {
            foreach (var origin in corsOrigins)
            {
                if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                {
                    client.AddCorsOrigin(origin);
                }
            }
        }

        return await _clientRepository.UpdateAsync(client);
    }
}
