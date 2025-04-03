using System.Text.RegularExpressions;

namespace CalculatorDemo
{
    public class MathParser : IMathParser
    {
        public List<string> Normalize(string expression)
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
    }
}
