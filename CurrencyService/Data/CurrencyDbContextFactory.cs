using CurrencyService.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Data
{
    public class CurrencyDbContextFactory : IDesignTimeDbContextFactory<CurrencyContext>
    {
        public IConfiguration Configuration { get; }

        public CurrencyDbContextFactory()
        {

        }

        public CurrencyContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                                         .Build();

            var builder = new DbContextOptionsBuilder<CurrencyContext>()
                                .UseSqlServer(configuration.GetConnectionString("CurrencyConnectionString"))
                                .EnableSensitiveDataLogging(true);

            return new CurrencyContext(builder.Options);
        }
    }
}
