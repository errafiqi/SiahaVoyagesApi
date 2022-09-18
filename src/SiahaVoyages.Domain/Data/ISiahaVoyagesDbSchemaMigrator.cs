using System.Threading.Tasks;

namespace SiahaVoyages.Data;

public interface ISiahaVoyagesDbSchemaMigrator
{
    Task MigrateAsync();
}
