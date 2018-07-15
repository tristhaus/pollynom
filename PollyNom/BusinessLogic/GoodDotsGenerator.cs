using System;
using System.Collections.Generic;

using PollyNom.BusinessLogic.Dots;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Creates good dots on a grid in a random manner.
    /// </summary>
    public class GoodDotsGenerator
    {
        private static Random rng = new Random();

        private int targetNumberOfDots;

        /// <summary>
        /// Creates a new instance of the <see cref="GoodDotsGenerator"/> class
        /// that will generate <paramref name="targetNumberOfDots"/> dots.
        /// </summary>
        /// <param name="targetNumberOfDots">Number of dots to be generated.</param>
        public GoodDotsGenerator(int targetNumberOfDots = 10)
        {
            this.targetNumberOfDots = targetNumberOfDots;
        }

        /// <summary>
        /// Generates a list of good dots 
        /// </summary>
        /// <returns></returns>
        public List<IDot> Generate()
        {
            List<CandidateDot> candidates = new List<CandidateDot>(400);
            for(int x = -10; x < 10; x++)
            {
                for(int y = -10; y < 10; y++)
                {
                    candidates.Add(new CandidateDot { x = x + 0.5, y = y + 0.5, picked = false });
                }
            }

            int candidateCount = candidates.Count;
            List<IDot> list = new List<IDot>();
            while(list.Count < targetNumberOfDots)
            {
                CandidateDot candidate = candidates[rng.Next(candidateCount)];
                if(!candidate.picked)
                {
                    list.Add(new GoodDot(candidate.x, candidate.y));
                    candidate.picked = true;
                }
            }

            return list;
        }

        /// <summary>
        /// Struct for candidate dots.
        /// </summary>
        private class CandidateDot
        {
            public double x;
            public double y;
            public bool picked;
        }
    }
}
