namespace TestProject
{
    using CalculatorService;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Reflection;

    [TestClass]
    public class CalculationServiceTest
    {
        [TestMethod]
        public void IsSingleNumberAndSymbols_Test()
        {
            string expression = "3+4-10";

            var actual = CalculationsService.IsSingleNumberAndSimbols(expression);

            Assert.IsTrue(actual);

        }

        [TestMethod]
        public void IsNotOnlySingleNumberAndSymbols_ExceptionTest()
        {
            string expression = "3+!-A*10";

            var result = CalculationsService.IsSingleNumberAndSimbols(expression);

            try
            {
                Assert.Fail("Expected exception was not thrown");
            }

            catch(InvalidOperationException ex)
            {
                Assert.AreEqual("Introduceti doar cifrele 1-9 si semnele respective : /,+,* -;", ex.Message);
            }
        }


        [TestMethod]
        public void IsSingleNumber_PositiveNumberTest()
        {
            string expression = "33";
            int expected;

            bool actual = CalculationsService.IsSingleNumber(expression , out expected);

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


        [TestMethod]
        public void HasInvalidCombination_Test()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('+');
            stack.Push('*');
            stack.Push('*');
           
         
           var actual = CalculationsService.HasInvalidCombination(stack);
            
           Assert.IsTrue(actual);  //trebuie si exception
           
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




        [TestMethod]
        public void Operate_Addition_ReturnsCorrectResult()
        {
            Stack<char>operators= new Stack<char>();
            operators.Push('+');
            int number1 = 5;
            int number2 = 4;

            int result= CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(9, result);
        }

        [TestMethod]    
        public void Operate_Subtraction_ReturnCorrectResult()
        {

            Stack<char> operators = new Stack<char>();
            operators.Push('-');
            int number1 = 5;
            int number2 = 4;

            int result = CalculationsService.Operate(operators,number1,number2); 

            Assert.AreEqual(1, result);
        }

        
    }
}