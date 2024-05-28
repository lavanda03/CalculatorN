using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace CalculatorWebApp.Services
{
	public class CalculatorServicesMVC
	{

		static bool divMinus = false;
		static bool otherNum = false;
		static bool addDot = false;
		static int count = 0;

		public string AddToExpression(string expressionInput, string value)
		{


			if (string.IsNullOrEmpty(expressionInput))
			{
				expressionInput = value;
			}
			else if (count == 1 && otherNum == false && value == ".")
			{
				return expressionInput;
			}
			else if (count == 0 && otherNum == true && value == ".")
			{
				expressionInput += value;
				count = 0;
				otherNum = false;
				count++;
			}
			else
			{
				string lastChar = expressionInput.Substring(expressionInput.Length - 1);
				string lastOperator = Regex.IsMatch(lastChar, @"[()+\-*\/\.]") ? lastChar : "";

				/*if (Regex.IsMatch(lastOperator, @"[+\-*\/]") && !Regex.IsMatch(value, @"[+\-*\/\.]"))
				{
					otherNum = true;
				}*/

				if (Regex.IsMatch(value, @"[()+\-*\/\.]") && divMinus == false)
				{
					if (otherNum == true && value == ".")
					{
						expressionInput += value;
						return expressionInput;

					}
					if (Regex.IsMatch(lastOperator, @"[()+\-*\/\.]"))
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
						if (value == ".")
						{
							addDot = true;
							count++;
						
						}
						if (value == "-" || value == "*" ||  value == "/" || value == "+")
						{
							otherNum = true;
							count = 0;
						}
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
				else if (!Regex.IsMatch(value, @"[+\-*\/\.]"))
				{
					expressionInput += value;
					divMinus = false;
				}
			}

			return expressionInput;
		}




		public string RemoveLastCharacter(string expression)
		{

			return expression.Remove(expression.Length - 1);

		}

		public string DeleteString(string expression)
		{
			return expression = " ";
		}




	}
}