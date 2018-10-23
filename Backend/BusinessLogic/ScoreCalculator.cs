using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// A class implementing the calculation of the score based on lists of hit counts.
    /// </summary>
    public static class ScoreCalculator
    {
        /// <summary>
        /// Calculates the score according to the series of numbers of hits.
        /// </summary>
        /// <param name="numbersOfGoodHits">List of good hit series.</param>
        /// <param name="numbersOfBadHits">List of bad hit series.</param>
        /// <returns>The calculated score.</returns>
        public static int CalculateScore(List<int> numbersOfGoodHits, List<int> numbersOfBadHits)
        {
            if (numbersOfBadHits.Any(x => x > 0))
            {
                return -1;
            }

            return numbersOfGoodHits.Where(x => (x > 0)).Select(x => ((int)Math.Pow(2, x) - 1)).Sum();
        }
    }
}
