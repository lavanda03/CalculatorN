Console.WriteLine("Introduceti expresia :");
string expresion = "--(-(-2+3))";

var result = CalculatorService.CalculationsService.Execute(expresion);
Console.WriteLine(result);

