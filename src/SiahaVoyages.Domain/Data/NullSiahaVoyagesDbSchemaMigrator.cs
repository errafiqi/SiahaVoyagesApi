using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SiahaVoyages.Data;

/* This is used if database provider does't define
 * ISiahaVoyagesDbSchemaMigrator implementation.
 */
public class NullSiahaVoyagesDbSchemaMigrator : ISiahaVoyagesDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
