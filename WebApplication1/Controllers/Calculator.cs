using CalculatorService;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApplication1.Controllers
{
    public class Calculator : Controller
    {
        private readonly CalculationsService _calculatorService;
        
        public Calculator()
        {
            _calculatorService = new CalculationsService();
        }
        public IActionResult Index(string expression)
        { 
            return View();
           
        }

        public IActionResult Execute (string expression)
        {
            var result = _calculatorService.Execute(expression);
           // ViewData["Result"] = result;
            ViewBag.Result = result;
            return View("Index");

        }
    }
}
