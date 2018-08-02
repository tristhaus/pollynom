using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Represents a sorted list of <see cref="PointLogical"/>. Is kept sorted on insertion.
    /// </summary>
    public class ListPointLogical
    {
        private List<PointLogical> list;

        /// <summary>
        /// Creates a new instance of the <see cref="ListPointLogical"/> class.
        /// </summary>
        public ListPointLogical()
        {
            this.list = new List<PointLogical>();
        }

        /// <summary>
        /// Gets the list of points contained in this instance.
        /// </summary>
        public IReadOnlyList<PointLogical> Points
        {
            get
            {
                return list.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the count of points contained in this instance.
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Adds the given point to the list, keeping the list sorted as needed.
        /// </summary>
        /// <param name="point">The point to be added.</param>
        public void Add(PointLogical point)
        {
            if (list.Count == 0 || list[list.Count - 1].X < point.X)
            {
                list.Add(point);
                return;
            }
            else
            {
                list.Add(point);
                list.Sort();
            }
        }
    }
}
