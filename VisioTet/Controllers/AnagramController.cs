using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using VisioTet.Models;

namespace VisioTet.Controllers
{
    public class AnagramController : Controller
    {
        private readonly ILogger<AnagramController> _logger;

        public AnagramController(ILogger<AnagramController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string firstWords, string secondWords)
        {
            if (string.IsNullOrEmpty(firstWords) || string.IsNullOrEmpty(secondWords))
            {
                ViewData["AnagramResults"] = "Please provide both first and second words.";
                return View("Index");
            }

            var firstWordArray = firstWords.Split(',');
            var secondWordArray = secondWords.Split(',');

            if (firstWordArray.Length != secondWordArray.Length)
            {
                ViewData["AnagramResults"] = "Number of first words doesn't match number of second words.";
                return View("Index");
            }

            var results = new List<int>();
            for (int i = 0; i < firstWordArray.Length; i++)
            {
                var isAnagram = AreAnagrams(firstWordArray[i], secondWordArray[i]) ? 1 : 0;
                results.Add(isAnagram);
            }

            ViewData["AnagramResults"] = string.Join("", results);
            return View("Index");
        }

        private bool AreAnagrams(string word1, string word2)
        {
            if (word1.Length != word2.Length)
                return false;

            var charArray1 = word1.ToCharArray();
            var charArray2 = word2.ToCharArray();
            Array.Sort(charArray1);
            Array.Sort(charArray2);

            for (int i = 0; i < charArray1.Length; i++)
            {
                if (charArray1[i] != charArray2[i])
                    return false;
            }

            return true;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}