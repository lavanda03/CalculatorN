﻿using CalculatorService;
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Execute (string expression)
        {
            var result = _calculatorService.Execute(expression);
            return View(result);

        }
    }
}