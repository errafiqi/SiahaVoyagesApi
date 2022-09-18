using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SiahaVoyages.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class SiahaVoyagesDbContextFactory : IDesignTimeDbContextFactory<SiahaVoyagesDbContext>
{
    public SiahaVoyagesDbContext CreateDbContext(string[] args)
    {
        SiahaVoyagesEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<SiahaVoyagesDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new SiahaVoyagesDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SiahaVoyages.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
