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
            if (Regex.IsMatch(S, @"^[0-9]*.?[0-9]*$", RegexOptions.Compiled))
            {
                return ParseToConstant(S);
            }

            List<int> addList = new List<int>();
            List<int> multiplyList = new List<int>();
            List<int> powerList = new List<int>();

            for (int index = 0; index < S.Length; index++)
            {
                char c = S[index];
                switch(c)
                {
                    case '(':
                        index = this.FindMatchingBrace(S, index);
                        continue;

                    case '^':
                        powerList.Add(index);
                        break;

                    case '*':
                    case '/':
                        multiplyList.Add(index);
                        break;

                    case '+':
                    case '-':
                        addList.Add(index);
                        break;

                    default:
                        break;
                }
            }

            if(addList.Count > 0)
            {
                // deal with simple constants
                if(addList.Count == 1 && addList[0] == 0)
                {
                    return ParseToConstant(S);
                }

                List<int> cleanedUpList = new List<int>(addList.Count);
                foreach(int index in addList)
                {
                    if(index == 0 || (index > 0 && S[index - 1] != '*' && S[index - 1] != '/' && S[index - 1] != '^'))
                    {
                        cleanedUpList.Add(index);
                    }
                }

                if(cleanedUpList.Count > 0)
                {
                    cleanedUpList.Add(S.Length);
                    List<Add.AddExpression> finalList = new List<Add.AddExpression>();

                    int firstIndex = -1;
                    int secondIndex;
                    foreach(int index in cleanedUpList)
                    {
                        secondIndex = index;

                        if(secondIndex == 0 && firstIndex == -1)
                        {
                            firstIndex = secondIndex;
                            continue;
                        }

                        Add.AddExpression.Signs sign = firstIndex == -1 || S[firstIndex] == '+' 
                            ? Add.AddExpression.Signs.Plus 
                            : Add.AddExpression.Signs.Minus;
                        ++firstIndex;
                        finalList.Add(new Add.AddExpression(sign, this.InternalParse(S.Substring(firstIndex, secondIndex - firstIndex))));
                        firstIndex = secondIndex;
                    }
                    return new Add(finalList);
                }
            }

            if (multiplyList.Count > 0)
            {
                multiplyList.Add(S.Length);
                List<Multiply.MultiplyExpression> finalList = new List<Multiply.MultiplyExpression>();

                int firstIndex = -1;
                int secondIndex;
                foreach (int index in multiplyList)
                {
                    secondIndex = index;

                    Multiply.MultiplyExpression.Signs sign = firstIndex == -1 || S[firstIndex] == '*'
                        ? Multiply.MultiplyExpression.Signs.Multiply
                        : Multiply.MultiplyExpression.Signs.Divide;
                    ++firstIndex;
                    finalList.Add(new Multiply.MultiplyExpression(sign, this.InternalParse(S.Substring(firstIndex, secondIndex - firstIndex))));
                    firstIndex = secondIndex;
                }
                return new Multiply(finalList);
            }

            return new InvalidExpression();
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
