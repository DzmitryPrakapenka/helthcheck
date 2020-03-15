using System.Linq;
using System.Threading.Tasks;
using HelthCheck.Data.Entities;
using HelthCheck.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var hosts = await _applicationContext.Checks
                .Select(t => new CheckListViewModel()
                {
                    Id = t.Id,
                    Host = t.TargetHost.IP,
                    Url = t.HelthCheckUrl
                }).ToArrayAsync();

            return View(hosts);
        }
    }
}
