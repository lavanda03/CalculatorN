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
            try
            {
                var result = _calculatorService.Execute(expression);
                ViewBag.Result = result;
               
            }
            catch(Exception ex) 
            {
                ViewBag.ErrorMesage=ex.Message; 
                
            }
			return View("Index");
		}
    }
}
