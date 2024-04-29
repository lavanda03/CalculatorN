Console.WriteLine("Introduceti expresia :");
string expresion = "-21--/+7";

var result = CalculatorService.CalculationsService.Execute(expresion);
Console.WriteLine(result);

