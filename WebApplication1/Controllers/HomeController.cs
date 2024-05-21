using CalculatorService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {


		private readonly CalculationsService _calculatorService;

		public HomeController()
		{
			_calculatorService = new CalculationsService();
		}
		public IActionResult Index(string expression)
		{
			return View();

		}

		public IActionResult Execute(string expression)
		{
			try
			{
				var result = _calculatorService.Execute(expression);
				ViewBag.Result = result;
				return View("Index");

			}
			catch (Exception)
			{
				ViewBag.Result = "Error";
				return View("Index");

			}


		}





	}
}
