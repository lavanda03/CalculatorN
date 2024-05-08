namespace TestProject
{
    using CalculatorService;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Linq.Expressions;
    using System.Reflection;

    [TestClass]
    public class CalculationServiceTest
    {

        //IsSingleNumberAndSymbols
        [TestMethod]
        public void IsSingleNumberAndSymbols_Test()
        {
            string expression = "3+4-10";

            var actual = CalculationsService.IsSingleNumberAndSimbols(expression);

            Assert.IsTrue(actual);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void IsNotOnlySingleNumberAndSymbols_ExceptionTest()
        {
            string expression = "3+!-A*10";

            var result = CalculationsService.IsSingleNumberAndSimbols(expression);

            try
            {
                Assert.Fail("Expected exception was not thrown");
            }

            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Introduceti doar cifrele 1-9 si semnele respective : /,+,* -;", ex.Message);
            }
        }


        //IsSingleNumber
        [TestMethod]
        public void IsSingleNumber_PositiveNumberTest()
        {
            string expression = "33";
            int expected;

            bool actual = CalculationsService.IsSingleNumber(expression, out expected);

            Assert.IsTrue(actual);
            Assert.AreEqual(33, expected);
        }


        [TestMethod]
        public void IsSingleNumber_NegativeNumberTest()
        {
            string expression = "-9";
            int expected;

            var actual = CalculationsService.IsSingleNumber(expression, out expected);

            Assert.IsTrue(actual);
            Assert.AreEqual(-9, expected);
        }


        [TestMethod]
        public void IsNotSingleNumber_Test()
        {
            string expression = "2 + 4 ";
            int expected;

            var actual = CalculationsService.IsSingleNumber(expression, out expected);

            Assert.IsFalse(actual);
            Assert.AreNotEqual(3, expected);
        }


        //HasInvalidCombination
        [TestMethod]
        public void HasInvalidCombination_Test()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('-');
            stack.Push('+');
            stack.Push('*');


            var actual = CalculationsService.HasInvalidCombination(stack);

            Assert.IsTrue(actual);  //trebuie si exception throw

        }
        [TestMethod]
        public void HasValidCombination_Test()
        {
            var stack = new Stack<char>();
            stack.Push('+');
            stack.Push('-');
            stack.Push('-');

            var actual = CalculationsService.HasInvalidCombination(stack);
            Assert.IsFalse(actual);

        }



        //Operate

        [TestMethod]
        public void Operate_ReturnPositiveNumberTest()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('+');
            stack.Push('-');
            stack.Push('-');

            int num1 = 4;
            int num2 = 2;


            int result = CalculationsService.Operate(stack, num1, num2);

            Assert.AreEqual(6, result);

        }

        [TestMethod]
        public void Operate_ReturnNegativeNumberTest()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('/');
            stack.Push('-');

            int num1 = 9;
            int num2 = 3;

            int result = CalculationsService.Operate(stack, num1, num2);

            Assert.AreEqual(-3, result);
        }


        [TestMethod]
        public void Operate_Addition_ReturnsCorrectResultTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('+');
            int number1 = 5;
            int number2 = 4;

            int result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void Operate_Subtraction_ReturnCorrectResultTest()
        {

            Stack<char> operators = new Stack<char>();
            operators.Push('-');
            int number1 = 5;
            int number2 = 4;

            int result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Operate_DivTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('/');
            int number1 = 20;
            int number2 = 4;

            int result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Operate_DivTo0Test()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('/');
            int number1 = 20;
            int number2 = 0;

            int result = CalculationsService.Operate(operators, number1, number2);

            //Assert.ThrowsException<DivideByZeroException>(() => result); 

        }


        [TestMethod]
        public void Operate_MultipleTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('*');
            int number1 = 6;
            int number2 = 4;

            int result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(24, result);
        }

        //operate impartirea cu virgula


        //WithoutParanthesis
        [TestMethod]
        public void WithoutParanthesis_WithoutParanthesisTest()
        {
            string expression = "3+4--5";
            string executedExpression;

            var result = CalculationsService.WithoutParenthesis(expression, out executedExpression);

            Assert.IsFalse(result);
            Assert.AreEqual("3+4--5", executedExpression);
        }

        [TestMethod]
        public void WithoutParanthesis_WithParanthesisTest()
        {
            string expression = "(3+4-10)";
            string executedExpression;

            var result = CalculationsService.WithoutParenthesis(expression, out executedExpression);

            Assert.IsTrue(result);
            Assert.AreEqual("-3", executedExpression);
        }

        [TestMethod]
        public void WithoutParanthesis_OpenPharantesis()
        {
            string expression = "(-(-6+5))";
            int lastOpenParenthesisIndex = -1;

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    lastOpenParenthesisIndex = i;
                }

            }
            Assert.AreEqual(2, lastOpenParenthesisIndex);

        }

        [TestMethod]
        public void WithoutParanthesis_ClosePharantesis()
        {
            string expression = "(-(-6+5))";
            int firstClosedParenthesisIndex = -1;
            int lastOpenParenthesisIndex = 2;


            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ')' && lastOpenParenthesisIndex != -1)
                {
                    firstClosedParenthesisIndex = i;
                }

            }
            Assert.AreEqual(8, firstClosedParenthesisIndex);

        }
         //trebuie ambele exceptii pentru ()

        //calculate
        [TestMethod]
        public void  Calculate_Test ()
        {
            string expression = "3+6";
           
            var result = CalculationsService.Calculate(expression);

            Assert.AreEqual(9, result);
        }
    }
    
}