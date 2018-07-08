using System;
using System.Collections.Generic;
using System.Linq;

namespace PollyNom.BusinessLogic
{
    public static class ScoreCalculator
    {
        public static int CalculateScore(List <int> numbersOfHits)
        {
            return numbersOfHits.Where(x => (x > 0)).Select(x => ((int)Math.Pow(2, x) - 1)).Sum();
        }
    }
}
