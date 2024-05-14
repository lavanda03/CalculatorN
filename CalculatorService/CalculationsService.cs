﻿using CalculatorService.Exceptions;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace CalculatorService;

public class CalculationsService
{
    public  double Execute(string expression)
    {

        expression = expression.Trim().Replace(" ", "");

        double result = 0;
       try
       {
            if (!IsSingleNumberAndSimbols(expression))
            {
               throw new Exception ("Introduceti doar cifrele 1-9 si semnele respective : /,+,* -; ");
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

    public static double Calculate(string expression)
    {
        if (IsSingleNumber(expression, out int num))
            return num;

        Stack<char> operators = new Stack<char>();
        StringBuilder currentNumber = new StringBuilder();

        double? firstNumber = null;
        double secondNumber = 0;
        bool foundFirstNumber = false;

        foreach (var c in expression)
        {
            if (char.IsDigit(c) || c == '.')
            {
                currentNumber.Append(c);

                if (firstNumber.HasValue)
                    secondNumber = double.Parse(currentNumber.ToString());
            }
            else
            {
                if (currentNumber.Length > 0)
                {
                    if (!firstNumber.HasValue)
                    {
                        firstNumber = double.Parse(currentNumber.ToString());
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


    public static bool WithoutParenthesis(string expression, out string executedExpressions)
    {
        if (!expression.Any(x => x is '(' or ')'))
        {
            executedExpressions = expression;
            return false;
        }

        int lastOpenParenthesisIndex = -1;
        int firstClosedParenthesisIndex = -1;
        bool foundFirstPair = false;

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
            else if (expression[i] == ')' && lastOpenParenthesisIndex == -1)
            {
                throw new Exception("Lipseste paranteza deschisa '(' ");
            }
        }


        if (!foundFirstPair)
        {
            throw new Exception("Lipseste paranteza inchisa ')' ");
        }

        string parenthesisExpression = expression
            .Substring(lastOpenParenthesisIndex, firstClosedParenthesisIndex - lastOpenParenthesisIndex)
            .Replace("(", "").Replace(")", "");


        string parenthesisExpressionResult = Calculate(parenthesisExpression).ToString();

        executedExpressions = expression.Remove(lastOpenParenthesisIndex,
                firstClosedParenthesisIndex - lastOpenParenthesisIndex + 1)
            .Insert(lastOpenParenthesisIndex, parenthesisExpressionResult);


        return true;
    }


    public static double Operate(Stack<char> operators, double number1, double number2)
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

            if ((prevOperator == '/' || prevOperator == '*'))
            {
                var ex = operators.Count(x => x == '-') % 2 == 0 ? negativeNumber = false : negativeNumber = true;
              //   negativeNumber = true;
                operators.Pop();
                op = prevOperator;
            }
            else
            {
                op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
            }
        }

        if (operators.Peek() == '*')
        {
            op = operators.Pop();
        }

        else if (operators.Peek() == '/')
        {
            op = operators.Pop();
        }
        
       else if (operators.Peek() == '+')
       {
            op = operators.Peek();
       }

        foreach (var c in operators)
        
            if (c == '-' && c == '+' )
            {
                op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
            }
        

        operators.Clear();

        double result = 0;
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

        if (double.IsPositiveInfinity(result))
            throw new DivideByZeroException();

        if (negativeNumber)
            result *= -1;


        return result;
    }
    
    public static bool IsSingleNumber(string expression, out int num)
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

    public static bool HasInvalidCombination(Stack<char> stack)
    {
        string[] invalidCombination = { "**", "//", "*/", "/*", "+*", "*+", "/+", "+/", "/-", "*-" };

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


    public static bool IsSingleNumberAndSimbols(string expression)
    {
        return !Regex.IsMatch(expression, @"[^0-9*/+\-()\.]");
    }
}