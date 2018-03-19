using CurrencyService.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public abstract class HostedService : IHostedService
    {
        private Task executingTask;
        private CancellationTokenSource cts;

        public Task StartAsync(CancellationToken cToken)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cToken);

            executingTask = ExecuteAsync(cts.Token);

            return executingTask.IsCompleted ? executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cToken)
        {
            if (executingTask == null)
            {
                return;
            }

            cts.Cancel();

            await Task.WhenAny(executingTask, Task.Delay(-1, cToken));

            cToken.ThrowIfCancellationRequested();
        }

        protected abstract Task ExecuteAsync(CancellationToken cToken);
    }
}
