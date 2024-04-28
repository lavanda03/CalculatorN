Console.WriteLine("Introduceti expresia :");
string expresion = "2++*(3//-//*---5)--7++(3*6)-7";

var result = CalculatorService.CalculationsService.Execute(expresion);
Console.WriteLine(result);

