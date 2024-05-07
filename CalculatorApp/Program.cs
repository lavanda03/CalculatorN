Console.WriteLine("Introduceti expresia :");

var expression = Console.ReadLine();

if(string.IsNullOrEmpty(expression))
    Console.WriteLine("Expresie invalida");
else
{
    var result = CalculatorService.CalculationsService.Execute(expression);
    Console.WriteLine(result);
}

