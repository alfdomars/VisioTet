using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using VisioTet.Models;

namespace VisioTet.Controllers
{
    public class ReverseController : Controller
    {
        private readonly ILogger<ReverseController> _logger;

        public ReverseController(ILogger<ReverseController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                ViewData["ErrorMessage"] = "Please enter a valid string.";
                return View();
            }

            string reversedString = ReverseString(input);
            ViewData["ReversedString"] = reversedString;

            return View();
        }

        private string ReverseString(string input)
        {
            char[] charArray = input.ToCharArray();
            int length = charArray.Length;

            string left = input.Substring(0, length / 2);
            string right = input.Substring((length + 1) / 2);

            // Reverse both parts
            char[] leftArray = left.ToCharArray();
            Array.Reverse(leftArray);
            char[] rightArray = right.ToCharArray();
            Array.Reverse(rightArray);

            // Combine the reversed parts
            string reversedString = new string(leftArray) + new string(rightArray);

            if (length % 2 == 1)
            {
                reversedString = reversedString.Insert(reversedString.Length / 2, input[length / 2].ToString());
            }

            return reversedString;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}