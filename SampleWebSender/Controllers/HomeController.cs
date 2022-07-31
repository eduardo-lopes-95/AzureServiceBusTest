using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleShared;
using SampleWebSender.Models;
using SampleWebSender.Services;

namespace SampleWebSender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IAzureServiceBus Busservice { get; }

        public HomeController(ILogger<HomeController> logger, IAzureServiceBus iAzureBusservice)
        {
            _logger = logger;
            this.Busservice = iAzureBusservice;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Person person)
        {
            await Busservice.SendMessageAsync(person, "personqueue");
            return RedirectToAction("Privacy");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
