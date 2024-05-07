using System.Text;
using System.Text.RegularExpressions;

namespace CalculatorService;

public static class CalculationsService
{
    public static int Execute(string expression)
    {
        expression = expression.Trim().Replace(" ", "");

        int result = 0;
        try
        {
            if (!IsSingleNumberAndSimbols(expression))
            {
                throw new Exception("Introduceti doar cifrele 1-9 si semnele respective : /,+,* -; ");
            }

            while (WithoutParenthesis(expression, out expression))
            {
            }

            result = IsSingleNumber(expression, out var num) ? num : Calculate(expression);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }

        return result;
    }

    private static int Calculate(string expression)
    {
        if (IsSingleNumber(expression, out int num))
            return num;

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

        int lastOpenParenthesisIndex = -1;
        int firstClosedParenthesisIndex = -1;
        bool foundFirstPair = false;

        // Căutăm prima pereche de paranteze deschise și închise care corespund
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == '(')
            {
                lastOpenParenthesisIndex = i;
            }
            else if (expression[i] == ')' && lastOpenParenthesisIndex != -1)
            {
                firstClosedParenthesisIndex = i;
                foundFirstPair = true;
                break;
            }
        }

        if (!foundFirstPair)
        {
            // Dacă nu găsim o pereche de paranteze deschise și închise, returnăm expresia inițială
            expression += ")";
            firstClosedParenthesisIndex = expression.Length - 1;
        }

        // Extragem subexpresia dintre prima pereche de paranteze
        string parenthesisExpression = expression
            .Substring(lastOpenParenthesisIndex, firstClosedParenthesisIndex - lastOpenParenthesisIndex)
            .Replace("(", "").Replace(")", "");

        // Calculăm rezultatul pentru subexpresia dintre paranteze
        string parenthesisExpressionResult = Calculate(parenthesisExpression).ToString();
        executedExpressions = expression.Remove(lastOpenParenthesisIndex,
                firstClosedParenthesisIndex - lastOpenParenthesisIndex + 1)
            .Insert(lastOpenParenthesisIndex, parenthesisExpressionResult);


        return true;
    }


    public static int Operate(Stack<char> operators, int number1, int number2)
    {
        char op = ' ';
        bool negativeNumber = false;

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

    private static bool IsSingleNumber(string expression, out int num)
    {
        int startIndex = 0;
        var operators = new List<char>();

        while (startIndex < expression.Length && (expression[startIndex] == '-' || expression[startIndex] == '+'))
        {
            operators.Add(expression[startIndex]);
            startIndex++;
        }

        if (int.TryParse(expression.Substring(startIndex), out num))
        {
            num = operators.Count(x => x == '-') % 2 == 0 ? num : num * -1;

            return true;
        }

        return false;
    }

    private static bool HasInvalidCombination(Stack<char> stack)
    {
        string[] invalidCombination = { "**", "//", "*/", "/*", "+*", "*+", "/+", "-/" };

        string stackString = new string(stack.ToArray());

        foreach (string c in invalidCombination)
        {
            if (stackString.Contains(c))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSingleNumberAndSimbols(string expression)
    {
        return !Regex.IsMatch(expression, @"[^0-9*/+\-()]");
    }
}