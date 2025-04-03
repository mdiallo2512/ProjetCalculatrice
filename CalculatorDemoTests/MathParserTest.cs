

using CalculatorDemo;

namespace CalculatorDemoTests
{
    [TestClass]
    public class MathParserTest
    {
        [TestMethod]
        public void InvalidInput_TestCase()
        {
            IMathParser parser = new MathParser();

            Assert.ThrowsException<ArgumentException>(() => parser.Normalize(null));
        }
    }
}
