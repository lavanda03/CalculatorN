using CalculatorService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using CalculatorWebApp.Services;
using Microsoft.Extensions.Primitives;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

		private readonly CalculationsService _calculatorService;
		private readonly CalculatorServicesMVC _calcServicesMVC;

		public HomeController()
		{
			_calculatorService = new CalculationsService();
			_calcServicesMVC = new CalculatorServicesMVC();
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

		public IActionResult AddToExpression(string expression, string buttonValue)
		{
			switch (buttonValue)
			{
				case"AC":
					expression = _calcServicesMVC.DeleteString(expression);
					break;

				case "C":
					expression = _calcServicesMVC.RemoveLastCharacter(expression);
					break;

				case "=":
					expression=_calculatorService.Execute(expression).ToString();	
					break;

                default:
			    expression = _calcServicesMVC.AddToExpression(expression, buttonValue);
			    break;
		    }


			ViewBag.Expression = expression;
			return View("Index");


		}

	}
}
