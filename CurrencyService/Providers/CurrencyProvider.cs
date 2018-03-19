using CurrencyService.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CurrencyService.Repositories.Interfaces;
using CurrencyService.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CurrencyService.Providers
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private IXmlReaderRepository repository;
        private IConfiguration configuration;
        public CurrencyProvider(IXmlReaderRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }
        public async Task UpdateCurrency(CancellationToken cToken)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CurrencyContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("CurrencyConnectionString"));
                using (var context = new CurrencyContext(optionsBuilder.Options))
                {
                    if (!context.CurrencyDates.Any(x => x.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString())))
                    {
                        bool isUrl = context.Paths.Where(x => x.Name == "WorkingUrl").Any();
                        if (isUrl)
                        {
                            string url = context.Paths.Where(x => x.Name == "WorkingUrl").FirstOrDefault().Value;
                            var returned = repository.ReadCurrencyFromUrl(url);
                            await context.Currencies.AddRangeAsync(returned);
                            await context.CurrencyDates.AddRangeAsync(returned.Select(x => x.CurrencyDate).ToList());
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        Serilog.Log.Information(string.Format("Currencies from date {0} was found. No data was added", DateTime.Now.ToShortDateString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
        }
    }
}
