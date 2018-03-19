using System.Threading;
using System.Threading.Tasks;

namespace CurrencyService.Providers.Interfaces
{
    public interface ICurrencyProvider
    {
        Task UpdateCurrency(CancellationToken cToken);
    }
}
