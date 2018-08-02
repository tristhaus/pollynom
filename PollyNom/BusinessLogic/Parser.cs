using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Implements parsing of a human-readable string to an <see cref="IExpression"/>.
    /// </summary>
    public class Parser
    {
        private readonly InvalidExpression invalidExpressionSample = new InvalidExpression();

        private Dictionary<string, Type> functions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        public Parser()
        {
            this.functions = new Dictionary<string, Type>();
            this.functions.Add(NaturalLogarithm.Symbol, typeof(NaturalLogarithm));
            this.functions.Add(Exponential.Symbol, typeof(Exponential));
            this.functions.Add(Sine.Symbol, typeof(Sine));
            this.functions.Add(Cosine.Symbol, typeof(Cosine));
            this.functions.Add(Tangent.Symbol, typeof(Tangent));
            this.functions.Add(AbsoluteValue.Symbol, typeof(AbsoluteValue));
        }

        /// <summary>
        /// Tests the expression for parseability.
        /// </summary>
        /// <param name="s">The string to be tested.</param>
        /// <returns><c>true</c> if string is parseable.</returns>
        public bool IsParseable(string s)
        {
            return !this.Parse(s).Equals(this.invalidExpressionSample);
        }

        /// <summary>
        /// The parsing operation from string to <see cref="IExpression"/>.
        /// </summary>
        /// <param name="s">The string to be parsed.</param>
        /// <returns>The expression, which can be <see cref="InvalidExpression"/>.</returns>
        public IExpression Parse(string s)
        {
            try
            {
                s = this.PrepareString(s);
                if (!this.ValidateInput(s))
                {
                    return this.invalidExpressionSample;
                }

                return this.InternalParse(s);
            }
            catch (Exception)
            {
                return this.invalidExpressionSample;
            }
        }

        /// <summary>
        /// Prepare the string for parsing.
        /// </summary>
        /// <param name="s">The string to be prepared.</param>
        /// <returns>The prepared string.</returns>
        private string PrepareString(string s)
        {
            return s.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Check if the input string is valid to be parsed per business criteria.
        /// </summary>
        /// <param name="s">The string to be checked.</param>
        /// <returns><c>true</c> if valid.</returns>
        private bool ValidateInput(string s)
        {
            // check unsupported characters
            {
                Regex regex = new Regex("^[-0-9.+/*^()abceilnopstxX]+$", RegexOptions.Compiled);
                if (!regex.IsMatch(s))
                {
                    return false;
                }
            }

            // check for "^-", "^+", which is hard to parse
            {
                if (s.Contains("^-") || s.Contains("^+"))
                {
                    return false;
                }
            }

            // check for dangling operators at the end of string
            {
                char lastChar = s[s.Length - 1];
                if (lastChar == '+' || lastChar == '-' || lastChar == '*' || lastChar == '\\' || lastChar == '^' || lastChar == '(')
                {
                    return false;
                }
            }

            // check balanced parentheses
            {
                int count = 0;
                foreach (char c in s)
                {
                    if (c == '(')
                    {
                        count++;
                    }
                    else if (c == ')')
                    {
                        count--;
                    }

                    if (count < 0)
                    {
                        return false;
                    }
                }

                if (count != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Internal parsing (post validation). Supports recursion when given prepared and validated strings.
        /// </summary>
        /// <param name="s">The prepared and validated string to be parsed.</param>
        /// <returns>The expression, which can be <see cref="InvalidExpression"/>.</returns>
        private IExpression InternalParse(string s)
        {
            if (!this.ValidateInput(s))
            {
                return this.invalidExpressionSample;
            }

            // if fully enclosed in braces, we remove them
            if (s[0] == '(' && s.Length - 1 == this.FindMatchingBrace(s, 0))
            {
                return this.InternalParse(s.Substring(1, s.Length - 2));
            }

            // deal with a simple case: plain x
            if (s == "X" || s == "x")
            {
                return new BaseX();
            }

            // deal with a simple case: a numerical constant
            if (Regex.IsMatch(s, @"^[+-]?[0-9]+.?[0-9]*$", RegexOptions.Compiled))
            {
                return this.ParseToConstant(s);
            }

            // now, tokenize
            List<string> tokens = new List<string>();
            List<string> ops = new List<string>();

            this.Tokenize(s, tokens, ops);

            // deal with a signed single token
            if (Regex.IsMatch(s, @"^[+-]", RegexOptions.Compiled) && tokens.Count == 1)
            {
                string subToken = s.Substring(1, s.Length - 1);
                Add.AddExpression.Signs sign = s[0] == '+' ? Add.AddExpression.Signs.Plus : Add.AddExpression.Signs.Minus;
                IExpression bracketedExpression = this.InternalParse(subToken);
                if (bracketedExpression.Equals(this.invalidExpressionSample))
                {
                    return this.invalidExpressionSample;
                }

                return new Add(new Add.AddExpression(sign, bracketedExpression));
            }

            // reassemble for recursive parsing
            // First case: plus and minus
            if (ops.Contains("+") || ops.Contains("-"))
            {
                return this.ParseAdd(tokens, ops);
            }

            // Second case: multiply and divide
            if (ops.Contains("*") || ops.Contains("/"))
            {
                return this.ParseMultiply(tokens, ops);
            }

            // Third case: power expressions
            if (ops.Contains("^"))
            {
                return this.ParsePower(tokens, ops);
            }

            // Fourth case: functions
            if (tokens.Count == 1)
            {
                return this.ParseFunction(tokens);
            }

            return this.invalidExpressionSample;
        }

        /// <summary>
        /// Parses a <see cref="Constant"/> expression from the input string.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <returns>The expression, which can be <see cref="InvalidExpression"/>.</returns>
        private IExpression ParseToConstant(string s)
        {
            double result;
            if (double.TryParse(s, NumberStyles.Any, new CultureInfo("en-US"), out result))
            {
                return new Constant(result);
            }

            return this.invalidExpressionSample;
        }

        /// <summary>
        /// Fill the provided lists with operands and operators.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <param name="tokens">Output slot for lists of operands.</param>
        /// <param name="ops">Output slot for lists of operators.</param>
        private void Tokenize(string s, List<string> tokens, List<string> ops)
        {
            string token = string.Empty;
            for (int index = 0; index < s.Length; index++)
            {
                char c = s[index];

                if (c == '(')
                {
                    int endIndex = this.FindMatchingBrace(s, index);
                    token += s.Substring(index, endIndex - index + 1);
                    index = endIndex;
                    continue;
                }

                if (this.IsOperatorChar(c) && token.Length > 0)
                {
                    tokens.Add(token);
                    token = string.Empty;
                    ops.Add(c.ToString());
                    continue;
                }

                token += c;
            }

            if (token.Length > 0)
            {
                tokens.Add(token);
                token = string.Empty;
            }
        }

        /// <summary>
        /// Checks the character for being a mathematical operator.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns><c>true</c> if character is an operator.</returns>
        private bool IsOperatorChar(char c)
        {
            return c == '-' || c == '+' || c == '*' || c == '/' || c == '^';
        }

        /// <summary>
        /// Parse the operators and operands into an <see cref="Add"/> expression,
        /// reassembling the rest into summands.
        /// </summary>
        /// <param name="tokens">The lists of operands.</param>
        /// <param name="ops">The lists of operators.</param>
        /// <returns>A returnable <see cref="IExpression"/>.</returns>
        private IExpression ParseAdd(List<string> tokens, List<string> ops)
        {
            List<Add.AddExpression> targetList = new List<Add.AddExpression>();

            Add.AddExpression.Signs sign = Add.AddExpression.Signs.Plus;

            string token = tokens[0];
            tokens.RemoveAt(0);
            foreach (var op in ops)
            {
                if (op == "+" || op == "-")
                {
                    var expression = this.InternalParse(token);
                    if (expression.Equals(this.invalidExpressionSample))
                    {
                        return this.invalidExpressionSample;
                    }

                    targetList.Add(new Add.AddExpression(sign, expression));
                    token = tokens[0];
                    tokens.RemoveAt(0);
                    sign = op == "+" ? Add.AddExpression.Signs.Plus : Add.AddExpression.Signs.Minus;
                }
                else
                {
                    token += op + tokens[0];
                    tokens.RemoveAt(0);
                }
            }

            if (tokens.Count == 1)
            {
                token = tokens[0];
            }

            if (token != string.Empty)
            {
                var expression = this.InternalParse(token);
                if (expression.Equals(this.invalidExpressionSample))
                {
                    return this.invalidExpressionSample;
                }

                targetList.Add(new Add.AddExpression(sign, expression));
                token = string.Empty;
            }

            return new Add(targetList);
        }

        /// <summary>
        /// Parse the operators and operands into a <see cref="Multiply"/> expression,
        /// reassembling the rest into factors.
        /// </summary>
        /// <param name="tokens">The lists of operands.</param>
        /// <param name="ops">The lists of operators.</param>
        /// <returns>A returnable <see cref="IExpression"/>.</returns>
        private IExpression ParseMultiply(List<string> tokens, List<string> ops)
        {
            List<Multiply.MultiplyExpression> targetList = new List<Multiply.MultiplyExpression>();

            Multiply.MultiplyExpression.Signs sign = Multiply.MultiplyExpression.Signs.Multiply;

            string token = tokens[0];
            tokens.RemoveAt(0);
            foreach (var op in ops)
            {
                if (op == "*" || op == "/")
                {
                    var expression = this.InternalParse(token);
                    if (expression.Equals(this.invalidExpressionSample))
                    {
                        return this.invalidExpressionSample;
                    }

                    targetList.Add(new Multiply.MultiplyExpression(sign, expression));
                    token = tokens[0];
                    tokens.RemoveAt(0);
                    sign = op == "*" ? Multiply.MultiplyExpression.Signs.Multiply : Multiply.MultiplyExpression.Signs.Divide;
                }
                else
                {
                    token += op + tokens[0];
                    tokens.RemoveAt(0);
                }
            }

            if (tokens.Count == 1)
            {
                token = tokens[0];
            }

            if (token != string.Empty)
            {
                var expression = this.InternalParse(token);
                if (expression.Equals(this.invalidExpressionSample))
                {
                    return this.invalidExpressionSample;
                }

                targetList.Add(new Multiply.MultiplyExpression(sign, expression));
                token = string.Empty;
            }

            return new Multiply(targetList);
        }

        /// <summary>
        /// Parse the operators and operands into a <see cref="Power"/> expression,
        /// reassembling the rest into base and exponent.
        /// </summary>
        /// <param name="tokens">The lists of operands.</param>
        /// <param name="ops">The lists of operators.</param>
        /// <returns>A returnable <see cref="IExpression"/>.</returns>
        private IExpression ParsePower(List<string> tokens, List<string> ops)
        {
            if (ops.Contains("*") || ops.Contains("/") || ops.Contains("+") || ops.Contains("-"))
            {
                return this.invalidExpressionSample;
            }

            string baseToken = tokens[0];
            tokens.RemoveAt(0);
            IExpression baseExpression = this.InternalParse(baseToken);
            if (baseExpression.Equals(this.invalidExpressionSample))
            {
                return this.invalidExpressionSample;
            }

            string exponentToken = tokens[0];
            tokens.RemoveAt(0);
            for (int index = 1; index < ops.Count; index++)
            {
                exponentToken += ops[index] + tokens[0];
                tokens.RemoveAt(0);
            }

            IExpression exponentExpression = this.InternalParse(exponentToken);
            if (exponentExpression.Equals(this.invalidExpressionSample))
            {
                return this.invalidExpressionSample;
            }

            return new Power(baseExpression, exponentExpression);
        }

        /// <summary>
        /// Parse the operators and operands into a <see cref="SingleArgumentFunctionBase{T}"/>
        /// expression, reassembling the rest into the argument.
        /// </summary>
        /// <param name="tokens">The lists of operands.</param>
        /// <returns>A returnable <see cref="IExpression"/>.</returns>
        /// <remarks>Note that the operators should be non-existent when this is called.</remarks>
        private IExpression ParseFunction(List<string> tokens)
        {
            string token = tokens[0];

            Regex functionNameRegex = new Regex(@"^(?<name>[a-zA-Z0-9]+\()");
            Match match = functionNameRegex.Match(token);
            string functionName = match.Groups["name"].Value.Replace("(", string.Empty);

            if (!this.functions.ContainsKey(functionName))
            {
                return this.invalidExpressionSample;
            }

            var functionType = this.functions[functionName];

            string argumentString = token.Substring(functionName.Length);
            IExpression argumentExpression = this.InternalParse(argumentString);

            if (argumentExpression.Equals(this.invalidExpressionSample))
            {
                return this.invalidExpressionSample;
            }

            return Activator.CreateInstance(functionType, argumentExpression) as IExpression;
        }

        /// <summary>
        /// Finds the parenthesis in the string matching the one at the given index.
        /// </summary>
        /// <param name="s">The string to be worked on.</param>
        /// <param name="pos">The index of the parenthesis that needs to be matched.</param>
        /// <returns>The index of the matching parenthesis.</returns>
        private int FindMatchingBrace(string s, int pos)
        {
            if (pos < 0 || pos > s.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pos) + "must be within" + nameof(s));
            }

            bool lookForClosing;
            if (s[pos] == '(')
            {
                lookForClosing = true;
            }
            else if (s[pos] == ')')
            {
                lookForClosing = false;
            }
            else
            {
                return -1;
            }

            int count = 0;
            if (lookForClosing)
            {
                for (int index = pos; index < s.Length; index++)
                {
                    char c = s[index];
                    if (c == '(')
                    {
                        count++;
                    }
                    else if (c == ')')
                    {
                        count--;
                    }

                    if (count == 0)
                    {
                        return index;
                    }
                }
            }
            else
            {
                for (int index = pos; index >= 0; index--)
                {
                    char c = s[index];
                    if (c == ')')
                    {
                        count++;
                    }
                    else if (c == '(')
                    {
                        count--;
                    }

                    if (count == 0)
                    {
                        return index;
                    }
                }
            }

            return -1;
        }
    }
}
