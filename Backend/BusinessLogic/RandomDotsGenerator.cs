using System;
using System.Collections.Generic;

using Persistence.Models;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Initializes good dots on a grid in a random manner.
    /// </summary>
    public class RandomDotsGenerator
    {
        private static readonly Random rng = new Random();

        private readonly int targetNumberOfGoodDots;
        private readonly int targetNumberOfBadDots;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDotsGenerator"/> class
        /// that will generate <paramref name="targetNumberOfGoodDots"/>+<paramref name="targetNumberOfBadDots"/> dots.
        /// </summary>
        /// <param name="targetNumberOfGoodDots">Number of good dots to be generated.</param>
        /// <param name="targetNumberOfBadDots">Number of bad dots to be generated.</param>
        public RandomDotsGenerator(int targetNumberOfGoodDots = 10, int targetNumberOfBadDots = 2)
        {
            this.targetNumberOfGoodDots = targetNumberOfGoodDots;
            this.targetNumberOfBadDots = targetNumberOfBadDots;
        }

        /// <summary>
        /// Generates a list of good dots.
        /// </summary>
        /// <returns>A list of dots in terms of logical units.</returns>
        public List<IDot> Generate()
        {
            List<CandidateDot> candidates = new List<CandidateDot>(400);
            for (int x = -10; x < 10; x++)
            {
                for (int y = -10; y < 10; y++)
                {
                    candidates.Add(new CandidateDot { X = x + 0.5, Y = y + 0.5, Picked = false });
                }
            }

            int candidateCount = candidates.Count;
            List<IDot> list = new List<IDot>();
            while (list.Count < this.targetNumberOfGoodDots)
            {
                CandidateDot candidate = candidates[rng.Next(candidateCount)];
                if (!candidate.Picked)
                {
                    list.Add(new Dot(DotKind.Good, candidate.X, candidate.Y));
                    candidate.Picked = true;
                }
            }

            while (list.Count < this.targetNumberOfBadDots + this.targetNumberOfGoodDots)
            {
                CandidateDot candidate = candidates[rng.Next(candidateCount)];
                if (!candidate.Picked)
                {
                    list.Add(new Dot(DotKind.Bad, candidate.X, candidate.Y));
                    candidate.Picked = true;
                }
            }

            return list;
        }

        /// <summary>
        /// Struct for candidate dots.
        /// </summary>
        private class CandidateDot
        {
            /// <summary>
            /// Gets or sets the x-coordinate of the dot.
            /// </summary>
            public double X { get; set; }

            /// <summary>
            /// Gets or sets the y-coordinate of the dot.
            /// </summary>
            public double Y { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this dot has been picked up,
            /// should be returned and cannot be picked again.
            /// </summary>
            public bool Picked { get; set; }
        }
    }
}
