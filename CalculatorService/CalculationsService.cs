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
        
        return Calculate(expression);
    }

    private static int Calculate(string expression)
    {
        //List<char> operators = new List<char>();

       // Stack<double> values = new Stack<double>();
        Stack<char> operators = new Stack<char>();
        StringBuilder currentNumber = new StringBuilder();

        int? firstNumber = null;
        int secondNumber = 0;

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
                        firstNumber = int.Parse(currentNumber.ToString());
                    else
                    {
                        firstNumber = Operate(operators, firstNumber.Value, secondNumber);
                    }

                    currentNumber = new StringBuilder();
                }

                operators.Push(c);
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
        char op;
        bool negativeNumber = false;
        
        if (operators.Any(x => x is '*' or '/'))
        {

            /*
              var multipleOperatorIndex = operators.LastIndex('*');
              var divideOperatorIndex = operators.LastIndexOf('/');

              op = multipleOperatorIndex > divideOperatorIndex ? '*' : '/';

              negativeNumber = operators.Count(x => x == '-') % 2 != 0;*/

           op = operators.Peek() == '*' ? '*' :
           operators.Peek() == '/' ? '/' :
           operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
        }
        else
        {
            op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
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
}