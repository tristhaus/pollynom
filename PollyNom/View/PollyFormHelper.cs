using System;
using System.Collections.Generic;
using System.Drawing;

using PollyNom.BusinessLogic;

namespace PollyNom.View
{
    public static class PollyFormHelper
    {
        /// <summary>
        /// Convert a list of points to be ready for display,
        /// that respect the scaling factors provided.
        /// </summary>
        /// <param name="pointLogicalLists">Input points.</param>
        /// <param name="scaleX">Horizontal scaling factor.</param>
        /// <param name="scaleY">Vertical scaling factor.</param>
        /// <returns>A list of lists of points.</returns>
        public static List<List<PointF>> ConvertToScaledPoints(List<ListPointLogical> pointLogicalLists, float scaleX = 1.0f, float scaleY = 1.0f)
        {
            List<List<PointF>> pointLists = new List<List<PointF>>(pointLogicalLists.Count);

            foreach (var pointLogicalList in pointLogicalLists)
            {
                List<PointF> pointList = new List<PointF>(pointLogicalList.Count);
                foreach (var logicalPoint in pointLogicalList.Points)
                {
                    try
                    {
                        PointF point = new PointF(scaleX * Convert.ToSingle(logicalPoint.X), scaleY * Convert.ToSingle(logicalPoint.Y));
                        pointList.Add(point);
                    }
                    catch (OverflowException)
                    {
                    }
                }

                pointLists.Add(pointList);
            }

            return pointLists;
        }
    }
}
