using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SiahaVoyages.Data;
using Volo.Abp.DependencyInjection;

namespace SiahaVoyages.EntityFrameworkCore;

public class EntityFrameworkCoreSiahaVoyagesDbSchemaMigrator
    : ISiahaVoyagesDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSiahaVoyagesDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SiahaVoyagesDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SiahaVoyagesDbContext>()
            .Database
            .MigrateAsync();
    }
}
