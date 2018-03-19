using CurrencyService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyService.Contexts;
using CurrencyService.Model;

namespace CurrencyService.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private CurrencyContext context;
        public CurrencyRepository(CurrencyContext context)
        {
            this.context = context;
        }
        public IEnumerable<Currency> GetAll()
        {
            List<Currency> currencies = new List<Currency>();
            try
            {
                if (context.Currencies.Any())
                {
                    currencies = GetAllCurrencies(context).ToList();
                                        
                    Serilog.Log.Information(string.Format("All data request, date of request {0}", DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
            return currencies;
        }

        public IEnumerable<Currency> GetByDate(DateTime dateTime)
        {
            List<Currency> currencies = new List<Currency>();
            try
            {
                if (context.Currencies.Any())
                {
                    currencies = GetAllCurrencies(context).Where(x => x.CurrencyDate.Date == dateTime).ToList();
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
            Serilog.Log.Information(string.Format("Request based on date was made, request param {0}, date of request {1}", dateTime.ToString(), DateTime.Now.ToString()));
            return currencies;
        }

        public IEnumerable<Currency> GetByName(string name)
        {
            List<Currency> currencies = new List<Currency>();
            try
            {
                if (context.Currencies.Any())
                {
                    currencies = GetAllCurrencies(context).Where(x => x.Name == name).ToList();
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
            Serilog.Log.Information(string.Format("Request based on name was made, request param {0}, date of request {1}", name, DateTime.Now.ToString()));
            return currencies;
        }

        public Currency GetByID(long id)
        {
            Currency currency = new Currency();
            try
            {
                if (context.Currencies.Any())
                {
                    currency = GetAllCurrencies(context).Where(x => x.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
            Serilog.Log.Information(string.Format("Request based on id was made, request param {0}, date of request {1}", id.ToString(), DateTime.Now.ToString()));
            return currency;
        }

        private IQueryable<Currency> GetAllCurrencies(CurrencyContext context)
        {
            return context.Currencies
                          .Join(context.CurrencyDates,
                          currency => currency.CurrencyDateID,
                          currDate => currDate.ID,
                          (currency, currDate) => new Currency()
                          {
                              ID = currency.ID,
                              Name = currency.Name,
                              Rate = currency.Rate,
                              CurrencyDateID = currency.CurrencyDateID,
                              CurrencyDate = currDate
                          });
        }
    }
}
