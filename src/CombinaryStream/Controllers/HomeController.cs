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
        private readonly CachedMergeService _mergeService;
        public HomeController(CachedMergeService mergeService) {
            _mergeService = mergeService;
        }

        [HttpGet("/items.json")]
        public async Task<IActionResult> JsonItemsAsync() {
            var (items,cacheHit) = await _mergeService.GetItemsExAsync();

            Response.Headers.Add("X-StreamCache", cacheHit ? "Hit" : "Miss");

            return Json(items);
        }
        
        [HttpGet("/")]
        public async Task<IActionResult> Index() {
            var (items,cacheHit) = await _mergeService.GetItemsExAsync();

            Response.Headers.Add("X-StreamCache", cacheHit ? "Hit" : "Miss");
            return View(items.ToList());
        }

        [HttpGet("/items.html")]
        public async Task<IActionResult> HtmlItems() {
            var (items,cacheHit) = await _mergeService.GetItemsExAsync();

            Response.Headers.Add("X-StreamCache", cacheHit ? "Hit" : "Miss");
            return View(items.ToList());
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
