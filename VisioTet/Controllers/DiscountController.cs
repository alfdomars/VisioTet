using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using VisioTet.Models;

namespace VisioTet.Controllers
{
    public class DiscountController : Controller
    {
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(ILogger<DiscountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculateDiscount(DiscountModel model)
        {
            if (ModelState.IsValid)
            {
                decimal diskon = CalculateDiscount(model.CustomerType, model.PointReward, model.TotalBelanja);
                decimal totalBayar = model.TotalBelanja - diskon;

                ViewBag.Diskon = diskon;
                ViewBag.TotalBayar = totalBayar;
            }

            return View("Index", model);
        }

        private decimal CalculateDiscount(string customerType, int pointReward, decimal totalBelanja)
        {
            decimal diskon = 0;

            switch (customerType.ToLower())
            {
                case "platinum":
                    if (pointReward >= 100 && pointReward <= 300)
                        diskon = totalBelanja * (decimal)0.5 + 35;
                    else if (pointReward >= 301 && pointReward <= 500)
                        diskon = totalBelanja * (decimal)0.5 + 50;
                    else if (pointReward > 500)
                        diskon = totalBelanja * (decimal)0.5 + 68;
                    break;
                case "gold":
                    if (pointReward >= 100 && pointReward <= 300)
                        diskon = totalBelanja * (decimal)0.25 + 25;
                    else if (pointReward >= 301 && pointReward <= 500)
                        diskon = totalBelanja * (decimal)0.25 + 34;
                    else if (pointReward > 500)
                        diskon = totalBelanja * (decimal)0.25 + 52;
                    break;
                case "silver":
                    if (pointReward >= 100 && pointReward <= 300)
                        diskon = totalBelanja * (decimal)0.1 + 12;
                    else if (pointReward >= 301 && pointReward <= 500)
                        diskon = totalBelanja * (decimal)0.1 + 27;
                    else if (pointReward > 500)
                        diskon = totalBelanja * (decimal)0.1 + 39;
                    break;
                default:
                    break;
            }

            return diskon;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}