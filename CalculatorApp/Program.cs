Console.WriteLine("Introduceti expresia :");
string expresion = "-5-/-5";

var result = CalculatorService.CalculationsService.Execute(expresion);
Console.WriteLine(result);

