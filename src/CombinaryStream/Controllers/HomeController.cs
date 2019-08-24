using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CombinaryStream.Models;
using CombinaryStream.Services;

namespace CombinaryStream.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemRepository _mergeService;
        public HomeController(CachedMergeService mergeService) {
            _mergeService = mergeService;
        }

        [HttpGet("/items.json")]
        public async Task<IActionResult> ItemsAsync() {
            return Json(await _mergeService.GetItemsAsync());
        }
        
        public IActionResult Index() {
            return RedirectToAction(nameof(ItemsAsync));
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
