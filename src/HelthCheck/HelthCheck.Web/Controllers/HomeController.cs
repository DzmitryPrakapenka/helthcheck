using System.Linq;
using HelthCheck.Web.Data;
using HelthCheck.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HelthCheck.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _applicationContext;

        public HomeController(ILogger<HomeController> logger,
            ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
        }

        public IActionResult Index()
        {
            var hosts = _applicationContext.TargetHosts
                .Select(t => new TargetHostViewModel()
                {
                    Id = t.Id,
                    Host = t.IP
                }).ToArray();

            return View(hosts);
        }
    }
}
