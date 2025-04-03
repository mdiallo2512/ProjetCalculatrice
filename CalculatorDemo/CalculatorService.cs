using System.Globalization;

namespace CalculatorDemo
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IMathParser _mathParser;

        private readonly Dictionary<string, int> _lstOrder = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 },
            { "sqrt", 4 }
        };

        public CalculatorService(IMathParser mathParser)
        {
            _mathParser = mathParser;
        }

        public double EvaluateExpression(string expression)
        {
            var lstInput = _mathParser.Normalize(expression);

            var lstRealExpression = ExtractRealExpression(lstInput);

            return EvaluateRealExpression(lstRealExpression);
        }

        // Conversion des inputs en enlevant toute parenthèse et space
        private List<string> ExtractRealExpression(List<string> lstInputs)
        {
            var operators = new Stack<string>();
            var output = new List<string>();

            foreach (string input in lstInputs)
            {
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                {
                    output.Add(input);
                }
                else if (input == "sqrt")
                {
                    operators.Push(input);
                }
                else if (input == "(")
                {
                    operators.Push(input);
                }
                else if (input == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Pop();
                }
                else
                {
                    while (operators.Count > 0 && _lstOrder.ContainsKey(operators.Peek()) &&
                           _lstOrder[operators.Peek()] >= _lstOrder[input])
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(input);
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }

        // Évalue l'expression réelle
        private static double EvaluateRealExpression(List<string> lstRealExpression)
        {
            Stack<double> valueStack = new Stack<double>();

            foreach (string realInput in lstRealExpression)
            {
                if (double.TryParse(realInput, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
                {
                    valueStack.Push(number);
                }
                else
                {
                    double result = 0;
                    switch (realInput)
                    {
                        case "+":
                            result = valueStack.Pop() + valueStack.Pop();
                            break;
                        case "-":
                            double subtrahend = valueStack.Pop();
                            result = valueStack.Pop() - subtrahend;
                            break;
                        case "*":
                            result = valueStack.Pop() * valueStack.Pop();
                            break;
                        case "/":
                            double divider = valueStack.Pop();
                            if (divider == 0)
                                throw new DivideByZeroException("Erreur, division par 0.");
                            result = valueStack.Pop() / divider;
                            break;
                        case "^":
                            double exponent = valueStack.Pop();
                            result = Math.Pow(valueStack.Pop(), exponent);
                            break;
                        case "sqrt":
                            result = Math.Sqrt(valueStack.Pop());
                            break;
                        default:
                            throw new InvalidOperationException($"Opérateur inconnu : {realInput}");
                    }
                    valueStack.Push(result);
                }
            }

            return valueStack.Pop();
        }
    }
}
