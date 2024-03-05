using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using VisioTet.Data;
using VisioTet.Models;

namespace VisioTet.Controllers
{
    public class DiscountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(ApplicationDbContext context, ILogger<DiscountController> logger)
        {
            _context = context;
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
                decimal diskon = CalculateDiscountAndSave(model.CustomerType, model.PointReward, model.TotalBelanja);
                decimal totalBayar = model.TotalBelanja - diskon;

                ViewBag.Diskon = diskon;
                ViewBag.TotalBayar = totalBayar;
            }

            return View("Index", model);
        }

        private decimal CalculateDiscountAndSave(string customerType, int pointReward, decimal totalBelanja)
        {
            decimal diskon = CalculateDiscount(customerType, pointReward, totalBelanja);

            // Save to database
            string transactionId = GenerateTransactionId();
            var transaction = new Discount
            {
                TransactionId = transactionId,
                CustomerType = customerType,
                PointReward = pointReward,
                TotalBelanja = totalBelanja,
                Discounts = diskon,
                TransactionDate = DateTime.Now
            };

            _context.Discounts.Add(transaction);
            _context.SaveChanges();

            return diskon;
        }

        private string GenerateTransactionId()
        {
            string dateString = DateTime.Now.ToString("yyyyMMdd");

            var lastTransaction = _context.Discounts
                .Where(t => t.TransactionId.StartsWith(dateString))
                .OrderByDescending(t => t.TransactionId)
                .FirstOrDefault();

            int runningNumber = (lastTransaction != null) ? int.Parse(lastTransaction.TransactionId.Substring(9)) + 1 : 1;

            string formattedRunningNumber = runningNumber.ToString("00000");

            string transactionId = dateString + "_" + formattedRunningNumber;

            return transactionId;
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