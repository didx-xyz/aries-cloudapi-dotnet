using AriesCloudAPI.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AriesCloudAPI.EntityFramework.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PersistedStateDbContext>
    {
        public PersistedStateDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PersistedStateDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new PersistedStateDbContext(builder.Options, new Configuration.DependencyInjection.Options.OperationalStoreOptions { });
        }
    }
}
