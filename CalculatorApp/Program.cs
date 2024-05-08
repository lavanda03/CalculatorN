using CalculatorService;

Console.WriteLine("Introduceti expresia :");

var expression = Console.ReadLine();

if(string.IsNullOrEmpty(expression))
    Console.WriteLine("Expresie invalida");
else
{
    var result = CalculatorService.CalculationsService.Execute(expression);
    decimal dec = Convert.ToDecimal(result);
    
    Console.WriteLine(dec);
}



