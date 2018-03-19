using CurrencyService.Model;
using CurrencyService.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace CurrencyService.Repositories
{
    internal class XmlReaderRepository : IXmlReaderRepository
    {
        public IEnumerable<Currency> ReadCurrencyFromUrl(string url)
        {
            List<Currency> currencies = new List<Currency>();
            DateTime date = new DateTime();
            try
            {
                XmlReader xmlreader = XmlReader.Create(url);
                while (xmlreader.Read())
                {
                    if (xmlreader.Name == "Cube")
                    {
                        if (xmlreader.HasAttributes)
                        {
                            var xmlDate = xmlreader.GetAttribute("time");
                            if (xmlDate != null)
                            {
                                date = DateTime.ParseExact(xmlDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            }

                            var name = xmlreader.GetAttribute("currency");
                            var rate = xmlreader.GetAttribute("rate");

                            if (name != null && rate != null)
                            {
                                currencies.Add(new Currency()
                                {
                                    Name = name,
                                    Rate = Convert.ToDecimal(rate,CultureInfo.InvariantCulture),
                                    CurrencyDate = new CurrencyDate() { Date = date }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }

            return currencies;
        }
    }
}
