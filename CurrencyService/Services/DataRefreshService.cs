using CurrencyService.Providers.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public class DataRefreshService : HostedService
    {
        public readonly ICurrencyProvider provider;

        public DataRefreshService(ICurrencyProvider provider)
        {
            this.provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                await provider.UpdateCurrency(cToken);
                await Task.Delay(TimeSpan.FromHours(24), cToken);
            }
        }
    }
}
