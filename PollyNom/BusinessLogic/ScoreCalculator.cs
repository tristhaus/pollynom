using System;
using System.Collections.Generic;
using System.Linq;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// A class implementing the calculation of the score based on lists of hit counts.
    /// </summary>
    public static class ScoreCalculator
    {
        /// <summary>
        /// Calculates the score according to the series of numbers of hits.
        /// </summary>
        /// <param name="numbersOfHits">List of hit series.</param>
        /// <returns>The calculated score.</returns>
        public static int CalculateScore(List<int> numbersOfHits)
        {
            return numbersOfHits.Where(x => (x > 0)).Select(x => ((int)Math.Pow(2, x) - 1)).Sum();
        }
    }
}
