using System.Globalization;
using System.Text.RegularExpressions;

namespace ProjetCalculatrice
{
    public class Calculator : ICalculator
    {
        private readonly Dictionary<string, int> _lstOrder = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 },
            { "sqrt", 4 }
        };

        /// <summary>
        /// Evaluer l'expréssion donnée en entrée
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public double EvaluateExpression(string expression) 
        {
            var lstInput = NormalizedExpression(expression);

            var lstRealExpression = ExtractRealExpression(lstInput);

            return EvaluateRealExpression(lstRealExpression);
         }

        // Analyse de la chaîne et séparation des éléments
        private static List<string> NormalizedExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentException("Expression invalide");
            }

            //gestion des signes doubles
            expression = expression.Replace(" ", "")
                                   .Replace("--", "+")  // -- devient +
                                   .Replace("+-", "-")
                                   .Replace("-+", "-")
                                   .Replace("++", "+");

            string pattern = @"(\d+(\.\d+)?|sqrt|\+|\-|\*|\/|\^|\(|\))";

            // Valider l'expression pour éviter les erreurs
            if (!Regex.IsMatch(expression, pattern))
                throw new ArgumentException("Caractères invalides dans la chaine.");

            MatchCollection matches = Regex.Matches(expression, pattern);

            var lstInputs = new List<string>();

            for (int i = 0; i < matches.Count; i++)
            {
                string input = matches[i].Value;

                // Gestion des signes négatifs au début ou après une parenthèse
                if (input == "-" && (i == 0 || (i > 0 && matches[i - 1].Value == "(")))
                {
                    lstInputs.Add("0"); // Ajoute un 0 pour gérer les négatifs correctement
                }

                lstInputs.Add(input);
            }

            return lstInputs;
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