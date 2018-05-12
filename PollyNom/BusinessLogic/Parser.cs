using PollyNom.BusinessLogic.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PollyNom.BusinessLogic
{
    public class Parser
    {
        public IExpression Parse(string S)
        {
            S = this.PrepareString(S);
            if (!ValidateInput(S))
            {
                return new InvalidExpression();
            }

            return this.InternalParse(S);
        }

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
            if (Regex.IsMatch(S, @"^[+-]?[0-9]*.?[0-9]*$", RegexOptions.Compiled))
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

            // reassemble for recursive parsing
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
                        targetList.Add(new Add.AddExpression(sign, this.InternalParse(token)));
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
                    targetList.Add(new Add.AddExpression(sign, this.InternalParse(token)));
                    token = string.Empty;
                }

                return new Add(targetList);
            }

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
                        targetList.Add(new Multiply.MultiplyExpression(sign, this.InternalParse(token)));
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
                    targetList.Add(new Multiply.MultiplyExpression(sign, this.InternalParse(token)));
                    token = string.Empty;
                }

                return new Multiply(targetList);
            }

            return new InvalidExpression();
        }

        private bool isOperatorChar(char c)
        {
            return c == '-' || c == '+' || c == '*' || c == '/' || c == '^';
        }

        private IExpression ParseToConstant(string S)
        {
            double result;
            if (double.TryParse(S, NumberStyles.Any, new CultureInfo("en-US"), out result))
            {
                return new Constant(result);
            }

            return new InvalidExpression();
        }

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

        private string PrepareString(string S)
        {
            return S.Replace(" ", string.Empty);
        }
    }
}
