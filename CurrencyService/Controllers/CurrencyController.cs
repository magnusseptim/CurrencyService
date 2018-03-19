using System;
using Microsoft.AspNetCore.Mvc;
using CurrencyService.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using CurrencyService.Providers.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyService.Controllers
{
    public class CurrencyController : Controller
    {
        private ICurrencyRepository repository;
        private ICurrencyProvider currProvider;

        public CurrencyController(ICurrencyRepository repository, ICurrencyProvider currProvider)
        {
            this.repository = repository;
            this.currProvider = currProvider;
        }

        [HttpGet("api/[controller]"),Authorize]
        public JsonResult GetAll()
        {
            currProvider.UpdateCurrency(new System.Threading.CancellationToken());
            return Json(repository.GetAll());
        }

        [HttpGet("api/[controller]/{id}"),Authorize]
        public JsonResult GetByID(long id)
        {
            return Json(repository.GetByID(id));
        }

        [HttpPost("api/[controller]"),Authorize]
        public JsonResult GetByDate(DateTime dateTime)
        {
            return Json(repository.GetByDate(dateTime));
        }

        [HttpPost("api/[controller]/{name}"), Authorize]
        public JsonResult GetByName(string name)
        {
            return Json(repository.GetByName(name));
        }
    }
}
