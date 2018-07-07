namespace PollyNom.Controller
{
    /// <summary>
    /// DTO concerning the coordinate system
    /// </summary>
    public class CoordinateSystemInfo
    {
        /// <summary>
        /// Gets or sets the left end of the x-axis in business logical units.
        /// </summary>
        public float StartX { get; set; }

        /// <summary>
        /// Gets or sets the right end of the x-axis in business logical units.
        /// </summary>
        public float EndX { get; set; }

        /// <summary>
        /// Gets or sets the bottom end of the x-axis in business logical units.
        /// </summary>
        public float StartY { get; set; }

        /// <summary>
        /// Gets or sets the top end of the x-axis in business logical units.
        /// </summary>
        public float EndY { get; set; }

        /// <summary>
        /// Gets or sets the distance between ticks.
        /// </summary>
        public float TickInterval { get; set; }
    }
}
