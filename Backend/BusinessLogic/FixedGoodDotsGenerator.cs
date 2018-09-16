using System.Collections.Generic;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Initializes good dots on a grid in a random manner.
    /// </summary>
    public class FixedGoodDotsGenerator
    {
        /// <summary>
        /// Generates a list of good dots
        /// </summary>
        /// <returns>A list of dots in terms of logical units.</returns>
        public List<IDot> Generate()
        {
            List<IDot> list = new List<IDot>();
            /*
            list.Add(new Dots.GoodDot(0.0, 0.0));
            list.Add(new Dots.GoodDot(4.0, 0.0));
            list.Add(new Dots.GoodDot(1.0, 1.25));
            */

            list.Add(new Dots.GoodDot(-3.0, -3.0));
            /*
            list.Add(new Dots.GoodDot(-3.0, 3.0));
            list.Add(new Dots.GoodDot(3.0, -3.0));

            list.Add(new Dots.GoodDot(1.0, 0.0));
            list.Add(new Dots.GoodDot(0.0, -1.0));
            */

            return list;
        }
    }
}
