using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace CalculatorWebApp.Services
{
	public class CalculatorServicesMVC
	{

		bool divMinus = false; 

		public string AddToExpression(string expressionInput, string value)
		{
			if (string.IsNullOrEmpty(expressionInput))
			{
				expressionInput = value;
			}
			else
			{
				string lastChar = expressionInput.Substring(expressionInput.Length - 1);
				string lastOperator = Regex.IsMatch(lastChar, @"[+\-*\/]") ? lastChar : "";
				

				if (Regex.IsMatch(value, @"[+\-*\/]") && divMinus == false)
				{
					if (Regex.IsMatch(lastOperator, @"[+\-*\/]"))
					{
						if ((Regex.IsMatch(lastOperator, @"[*\/]") && Regex.IsMatch(value, @"[-]")))
						{
							expressionInput += value;
							divMinus = true;
						}
						else if (value == "-" && Regex.IsMatch(lastChar, @"\($"))
						{
							expressionInput += value;
						}
						else
						{
							expressionInput = expressionInput.Substring(0, expressionInput.Length - 1) + value;
						}
					}
					else
					{
						expressionInput += value;
					}
				}
				else if (value == "(")
				{
					if (string.IsNullOrEmpty(lastChar) || Regex.IsMatch(lastChar, @"[*+\-\/(]"))
					{
						expressionInput += value;
					}
				}
				else if (!Regex.IsMatch(value, @"[+\-*\/]"))
				{
					expressionInput += value;
				}
			}

			return expressionInput;
		}




		public string RemoveLastCharacter(string expression)
		{

			return expression.Remove(expression.Length-1);

		}
		
		public string DeleteString(string expression)
		{
			return expression = " ";
		}

		


	}
}
