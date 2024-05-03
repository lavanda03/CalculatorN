using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculatorService;

public static class CalculationsService
{
    public static int Execute(string expression)
    {
        expression = expression.Trim().Replace(" ", "");

        try
        {
            if (!IsSingleNumberAndSimbols(expression))
            {
                throw new Exception("Introduceti doar cifrele 1-9 si semnele respective : /,+,* -; ");
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }

        while (WithoutParenthesis(expression, out expression))
        {
        }

        if (IsSingleNumber(expression))
        {
            return int.Parse(expression);
        }

        else

        { return EvaulateFinalResult(expression); }
    }

    private static int Calculate(string expression)
    {

        Stack<char> operators = new Stack<char>();
        StringBuilder currentNumber = new StringBuilder();

        int? firstNumber = null;
        int secondNumber = 0;
        bool foundFirstNumber = false;

        foreach (var c in expression)
        {

            if (char.IsDigit(c))
            {
                currentNumber.Append(c);

                if (firstNumber.HasValue)
                    secondNumber = int.Parse(currentNumber.ToString());

            }
            else
            {
                if (!string.IsNullOrEmpty(currentNumber.ToString()))
                {
                    if (!firstNumber.HasValue)
                    {
                        firstNumber = int.Parse(currentNumber.ToString());
                        foundFirstNumber = true;
                    }
                    else
                    {
                        firstNumber = Operate(operators, firstNumber.Value, secondNumber);
                    }

                    currentNumber = new StringBuilder();
                }


                if (c == '-' && !foundFirstNumber)
                {
                    currentNumber.Append(c);

                }

                else
                {
                    operators.Push(c);
                }

            }

        }

        if (operators.Count > 0)
        {
            firstNumber = Operate(operators, firstNumber.Value, secondNumber);
        }

        return firstNumber.Value;
    }


    private static bool WithoutParenthesis(string expression, out string executedExpressions)
    {
        if (!expression.Any(x => x is '(' or ')'))
        {
            executedExpressions = expression;
            return false;
        }

        int firstOpenParenthesisIndex = -1;
        int lastCloseParenthesisIndex = -1;
        bool foundFirstPair = false;

        // Căutăm prima pereche de paranteze deschise și închise care corespund
        for (int i = 0; i < expression.Length; i++)
        {

            if (expression[i] == '(')
            {
                firstOpenParenthesisIndex = i;
            }
            else if (expression[i] == ')' && firstOpenParenthesisIndex != -1)
            {
                lastCloseParenthesisIndex = i;
                foundFirstPair = true;
                break;
            }
        }


        int countDigits = expression.Count(char.IsDigit);
        if (countDigits == 1 || countDigits == 2)
        {
            EvaulateFinalResult(expression);

           
        }
       

        if (!foundFirstPair)
        {
            // Dacă nu găsim o pereche de paranteze deschise și închise, returnăm expresia inițială
            executedExpressions = expression;
            return false;
        }

        // Extragem subexpresia dintre prima pereche de paranteze
        string parenthesisExpression = expression.Substring(firstOpenParenthesisIndex , lastCloseParenthesisIndex - firstOpenParenthesisIndex ).Replace("(", "").Replace(")", "");

        // Calculăm rezultatul pentru subexpresia dintre paranteze
        string parenthesisExpressionResult = Calculate(parenthesisExpression).ToString();
        executedExpressions = expression.Remove(firstOpenParenthesisIndex, lastCloseParenthesisIndex - firstOpenParenthesisIndex + 1)
                                     .Insert(firstOpenParenthesisIndex, parenthesisExpressionResult);


        return true;
    }


    public static int Operate(Stack<char> operators, int number1, int number2)
    {
        char op = ' ';
        bool negativeNumber = false;

        try
        {
            if (HasInvalidCombination(operators))
            {
                throw new Exception("Combinație invalidă de semne.");
            }

            if (operators.TryPeek(out char lasOperator) && lasOperator == '-')
            {
                char prevOperator = operators.ElementAt(operators.Count - 1);

                if (prevOperator == '/' || prevOperator == '*')
                {
                    negativeNumber = true;
                    operators.Pop();
                    op = prevOperator;
                }

                else
                {
                    op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
                }

            }

            else
            {
                op = operators.Peek() == '*' ? '*' : operators.Peek() == '/' ? '/' :
                op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
            }


        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
        
        operators.Clear();

        var result = 0;
        switch (op)
        {
            case '+':
                result = number1 + number2;
                break;
            case '-':
                result = number1 - number2;
                break;
            case '*':
                result = number1 * number2;
                break;
            case '/':
                result = number1 / number2;
                break;
        }

        if (negativeNumber)
            result *= -1;

        return result;
    }

    private static bool IsSingleNumber(string expression)
    {
        int num;
        return int.TryParse(expression, out num);
    }

    static bool HasInvalidCombination(Stack<char> stack)
    {
        string[] invalidCombination = { "**", "//", "*/", "/*", "+*", "*+", "/+", "-/"};
        
        string stackString=new string(stack.ToArray());

        foreach(string c in invalidCombination)
        {
            if (stackString.Contains(c))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsSingleNumberAndSimbols(string expression)
    {
        if (Regex.IsMatch(expression, @"[^0-9*/+\-()]"))
        {
            return false;
        }
        return true;
    }

    public static int EvaulateFinalResult(string expression)
    {
        int countMinusSign = expression.Count(x => x == '-');
        int countDigits = expression.Count(char.IsDigit);
        if (countDigits == 1 || countDigits == 2)
        {
            char op = countMinusSign % 2 == 0 ? '+' : '-';

            var numberString = expression.Replace("(", "").Replace(")", "").Replace("-", "");
            int number = int.Parse(numberString);

            return  op == '-' ? -number : number;
         

            
        }
        else
        {
           return Calculate(expression);   
        }

    }
}