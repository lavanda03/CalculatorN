using System.ComponentModel.Design;
using System.Security;
using System.Text;

namespace CalculatorService;

public static class CalculationsService
{
    public static int Execute(string expression)
    {
        expression = expression.Trim().Replace(" ", "");

        while (WithoutParenthesis(expression, out expression))
        {
        }

        if (IsSingleNumber(expression))
        {
            return int.Parse(expression);
        }

        else

        { return Calculate(expression); }
    }

    private static int Calculate(string expression)
    {
       
        Stack<char> operators = new Stack<char>();
        StringBuilder currentNumber = new StringBuilder();

        int? firstNumber =null;
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

        var ex1 = expression.SkipWhile(x => x != '(').TakeWhile(x => x != ')').Append(')').ToList();
        
        var parenthesisExpression = new string(ex1.ToArray());

        var parenthesisExpressionResult = Calculate(parenthesisExpression.Replace("(", "").Replace(")", ""));

        executedExpressions = expression.Replace(parenthesisExpression, parenthesisExpressionResult.ToString());

        return true;
    }

    private static int Operate(Stack<char> operators, int number1, int number2)
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
                op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';

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
}