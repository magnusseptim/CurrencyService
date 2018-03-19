using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.GlobalHelpers
{
    public static class DbContextExtension
    {
        public static bool AllMigrationApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                                 .GetAppliedMigrations()
                                 .Select(x => x.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                               .Migrations
                               .Select(x => x.Key);

            return !total.Except(applied).Any();
        }
    }
}
