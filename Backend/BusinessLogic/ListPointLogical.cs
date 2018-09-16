using System.Collections.Generic;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Represents a sorted list of <see cref="PointLogical"/>. Is kept sorted on insertion.
    /// </summary>
    public class ListPointLogical
    {
        private List<PointLogical> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPointLogical"/> class.
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
                return this.list.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the count of points contained in this instance.
        /// </summary>
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        /// <summary>
        /// Adds the given point to the list, keeping the list sorted as needed.
        /// </summary>
        /// <param name="point">The point to be added.</param>
        public void Add(PointLogical point)
        {
            if (this.list.Count == 0 || this.list[this.list.Count - 1].X < point.X)
            {
                this.list.Add(point);
                return;
            }
            else
            {
                this.list.Add(point);
                this.list.Sort();
            }
        }
    }
}
