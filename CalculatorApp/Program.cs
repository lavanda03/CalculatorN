Console.WriteLine("Introduceti expresia :");
string expresion = "(11+18) * 20-2";

var result = CalculatorService.CalculationsService.Execute(expresion);
Console.WriteLine(result);

