namespace TestProject
{
    using CalculatorService;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
   
    [TestClass]
    public class CalculatorTest
    {
      

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