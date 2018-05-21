﻿using PollyNom.BusinessLogic.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Implements parsing of a human-readable string to an <see cref="IExpression"/>.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// The parsing operation from string to <see cref="IExpression"/>.
        /// </summary>
        /// <param name="S">The string to be parsed.</param>
        /// <returns>The expression, which can be <see cref="InvalidExpression"/>.</returns>
        public IExpression Parse(string S)
        {
            S = this.PrepareString(S);
            if (!ValidateInput(S))
            {
                return new InvalidExpression();
            }

            return this.InternalParse(S);
        }

        /// <summary>
        /// Internal parsing (post validation). Supports recursion when given prepared and validated strings.
        /// </summary>
        /// <param name="S">The prepared and validated string to be parsed.</param>
        /// <returns>The expression, which can be <see cref="InvalidExpression"/>.</returns>
        private IExpression InternalParse(string S)
        {
            // if fully enclosed in braces, we remove them
            if (S[0] == '(' && S.Length - 1 == this.FindMatchingBrace(S, 0))
            {
                return this.InternalParse(S.Substring(1, S.Length - 2));
            }

            // deal with a simple case
            if (S == "X" || S == "x")
            {
                return new BaseX();
            }

            // deal with a simple case
            if (Regex.IsMatch(S, @"^[+-]?[0-9]+.?[0-9]*$", RegexOptions.Compiled))
            {
                return ParseToConstant(S);
            }

            // Now, tokenize
            string token = string.Empty;
            List<string> tokens = new List<string>();
            List<string> ops = new List<string>();

            for (int index = 0; index < S.Length; index++)
            {
                char c = S[index];

                if (c == '(')
                {
                    int EndIndex = this.FindMatchingBrace(S, index);
                    token += S.Substring(index, EndIndex - index + 1);
                    index = EndIndex;
                    continue;
                }

                if (isOperatorChar(c) && token.Length > 0)
                {
                    tokens.Add(token);
                    token = string.Empty;
                    ops.Add(c.ToString());
                    continue;
                }

                token += c;
            }

            if(token.Length > 0)
            {
                tokens.Add(token);
                token = string.Empty;
            }

            // deal with a signed single token
            if (Regex.IsMatch(S, @"^[+-]", RegexOptions.Compiled) && tokens.Count == 1)
            {
                string subToken = S.Substring(1, S.Length - 1);
                Add.AddExpression.Signs sign = S[0] == '+' ? Add.AddExpression.Signs.Plus : Add.AddExpression.Signs.Minus;
                IExpression bracketedExpression = this.InternalParse(subToken);
                if (bracketedExpression.Equals(new InvalidExpression()))
                {
                    return new InvalidExpression();
                }

                return new Add(new Add.AddExpression(sign, bracketedExpression));
            }

            // reassemble for recursive parsing, three cases. First case: plus and minus
            if (ops.Contains("+") || ops.Contains("-"))
            {
                List<Add.AddExpression> targetList = new List<Add.AddExpression>();

                Add.AddExpression.Signs sign = Add.AddExpression.Signs.Plus;

                token = tokens[0];
                tokens.RemoveAt(0);
                foreach(var op in ops)
                {
                    if(op == "+" || op == "-")
                    {
                        var expression = this.InternalParse(token);
                        if (expression.Equals(new InvalidExpression()))
                        {
                            return new InvalidExpression();
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

                if(tokens.Count == 1)
                {
                    token = tokens[0];
                }

                if (token != string.Empty)
                {
                    var expression = this.InternalParse(token);
                    if (expression.Equals(new InvalidExpression()))
                    {
                        return new InvalidExpression();
                    }
                    targetList.Add(new Add.AddExpression(sign, expression));
                    token = string.Empty;
                }

                return new Add(targetList);
            }

            // Second case: multiply and divide
            if (ops.Contains("*") || ops.Contains("/"))
            {
                List<Multiply.MultiplyExpression> targetList = new List<Multiply.MultiplyExpression>();

                Multiply.MultiplyExpression.Signs sign = Multiply.MultiplyExpression.Signs.Multiply;

                token = tokens[0];
                tokens.RemoveAt(0);
                foreach (var op in ops)
                {
                    if (op == "*" || op == "/")
                    {
                        var expression = this.InternalParse(token);
                        if (expression.Equals(new InvalidExpression()))
                        {
                            return new InvalidExpression();
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
                    if(expression.Equals(new InvalidExpression()))
                    {
                        return new InvalidExpression();
                    }
                    targetList.Add(new Multiply.MultiplyExpression(sign, expression));
                    token = string.Empty;
                }

                return new Multiply(targetList);
            }

            // Third case: power expressions
            if (ops.Contains("^"))
            {
                if (ops.Contains("*") || ops.Contains("/") || ops.Contains("+") || ops.Contains("-"))
                {
                    return new InvalidExpression();
                }

                string baseToken = tokens[0];
                tokens.RemoveAt(0);
                IExpression baseExpression = this.InternalParse(baseToken);
                if (baseExpression.Equals(new InvalidExpression()))
                {
                    return new InvalidExpression();
                }

                string exponentToken = tokens[0];
                tokens.RemoveAt(0);
                for(int index = 1; index < ops.Count; index++)
                {
                    exponentToken += ops[index] + tokens[0];
                    tokens.RemoveAt(0);
                }

                IExpression exponentExpression = this.InternalParse(exponentToken);
                if (exponentExpression.Equals(new InvalidExpression()))
                {
                    return new InvalidExpression();
                }

                return new Power(baseExpression, exponentExpression);
            }

            return new InvalidExpression();
        }

        /// <summary>
        /// Checks the character for being a mathemical operator.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns><c>true</c> if character is an operator.</returns>
        private bool isOperatorChar(char c)
        {
            return c == '-' || c == '+' || c == '*' || c == '/' || c == '^';
        }

        /// <summary>
        /// Parses a <see cref="Constant"/> expression from the input string.
        /// </summary>
        /// <param name="S">The input string.</param>
        /// <returns>The expression, which can be <see cref="InvalidExpression"/>.</returns>
        private IExpression ParseToConstant(string S)
        {
            double result;
            if (double.TryParse(S, NumberStyles.Any, new CultureInfo("en-US"), out result))
            {
                return new Constant(result);
            }

            return new InvalidExpression();
        }

        /// <summary>
        /// Finds the parenthesis in the string matching the one at the given index.
        /// </summary>
        /// <param name="S">The string to be worked on.</param>
        /// <param name="pos">The index of the parenthesis that needs to be matched.</param>
        /// <returns>The index of the matching parenthesis.</returns>
        private int FindMatchingBrace(string S, int pos)
        {
            if (pos < 0 || pos > S.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pos) + "must be within" + nameof(S));
            }

            bool lookForClosing;
            if (S[pos] == '(')
            {
                lookForClosing = true;
            }
            else if (S[pos] == ')')
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
                for (int index = pos; index < S.Length; index++)
                {
                    char c = S[index];
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
                    char c = S[index];
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

        /// <summary>
        /// Check if the input string is valid to be parsed per business criteria.
        /// </summary>
        /// <param name="S">The string to be checked.</param>
        /// <returns><c>true</c> if valid.</returns>
        private bool ValidateInput(string S)
        {
            // check unsupported characters 
            {
                Regex regex = new Regex("^[-0-9.+/*^()xX]+$", RegexOptions.Compiled);
                if (!regex.IsMatch(S))
                {
                    return false;
                }
            }

            // check for "^-", "^+", which is hard to parse
            {
                if(S.Contains("^-") || S.Contains("^+"))
                {
                    return false;
                }
            }

            // check balanced parentheses
            {
                int count = 0;
                foreach (char c in S)
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
        /// Prepare the string for parsing.
        /// </summary>
        /// <param name="S">The string to be prepared.</param>
        /// <returns>The prepared string.</returns>
        private string PrepareString(string S)
        {
            return S.Replace(" ", string.Empty);
        }
    }
}
