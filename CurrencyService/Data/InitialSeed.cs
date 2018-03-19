using CurrencyService.Contexts;
using CurrencyService.Model;
using CurrencyService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyService.Data
{
    public static class InitialSeed
    {
        public static void Seed(this CurrencyContext context, IXmlReaderRepository reader)
        {
            context.Database.EnsureCreated();

            if (context.Currencies.Any())
            {
                return;
            }

            try
            {
                var paths = new Path[]
                {
                    new Path(){ Name = "InitializeData", Value = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml" },
                    new Path(){ Name = "WorkingUrl", Value = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml" },
                };

                context.Paths.AddRange(paths);

                var currencies = reader.ReadCurrencyFromUrl("http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml");
                var dates = currencies.Select(x => x.CurrencyDate).GroupBy(y=>y.Date).Select(z=>z.First()).ToList();

                context.CurrencyDates.AddRange(dates);
                context.SaveChanges();

                var dbDates = context.CurrencyDates.ToList();


                foreach (var x in currencies)
                {
                    x.CurrencyDateID = dbDates.FirstOrDefault(y => y.Date == x.CurrencyDate.Date).ID;
                    x.CurrencyDate = null;
                }

                context.Currencies.AddRange(currencies);
                context.SaveChanges();
                Serilog.Log.Information(string.Format("DB Migration was made, Date : {0}", DateTime.Now));

            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
        }
        public static void ComplementarySeed(this CurrencyContext context, IXmlReaderRepository reader)
        {
            context.Database.EnsureCreated();
            var addedDates = new List<CurrencyDate>();
            try
            {
                var paths = new Path[]
                {
                    new Path(){ Name = "InitializeData", Value = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml" },
                    new Path(){ Name = "WorkingUrl", Value = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml" },
                };

                foreach (var path in paths)
                {
                    if (!context.Paths.Any(x => x.Name == path.Name))
                    {
                        context.Paths.Add(path);
                    }

                }

                var currencies = reader.ReadCurrencyFromUrl("http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml");
                var dates = currencies.Select(x => x.CurrencyDate).GroupBy(y => y.Date).Select(z => z.First()).ToList();


                foreach (var date in dates)
                {
                    if (!context.CurrencyDates.Any(x => x.Date == date.Date))
                    {
                        context.CurrencyDates.Add(date);
                        addedDates.Add(date);
                    }
                }

                context.SaveChanges();

                var dbDates = context.CurrencyDates.ToList();


                foreach (var x in currencies)
                {
                    x.CurrencyDateID = dbDates.FirstOrDefault(y => y.Date == x.CurrencyDate.Date).ID;
                    x.CurrencyDate = null;
                }

                foreach (var x in currencies)
                {
                    if (addedDates.Exists(y=>y.ID == x.CurrencyDateID))
                    {
                        context.Currencies.Add(x);
                    }
                }

                context.SaveChanges();
                Serilog.Log.Information(string.Format("Complementary DB fullfillment was made, Date : {0}", DateTime.Now));

            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
        }
    }
}
