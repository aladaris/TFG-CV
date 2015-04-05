using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sequencer {
    /// <summary>
    /// This class contains a set of methods used for manipulating
    /// Polygons and its polints.
    /// </summary>
    class PolygonHandling {

        /// <summary>
        /// Gets the, aproximate, mass center of a give Polygon.
        /// </summary>
        /// <param name="i_polygon">List of points, wich represents the polygon.</param>
        /// <returns>A point with floating point precision.</returns>
        public static PointF GetMassCenter(List<Point> i_polygon) {
            PointF center = new PointF(0f, 0f);
            foreach (Point p in i_polygon) {
                center.X += p.X;
                center.Y += p.Y;
            }
            center.X *= (1f / i_polygon.Count);
            center.Y *= (1f / i_polygon.Count);
            return center;
        }

        /// <summary>
        /// Arranges the given rectangle vertices to the formart
        /// used by OpenCV.
        /// </summary>
        /// <param name="i_corners">List with the four vertices</param>
        /// <returns></returns>
        public static PointF[] SortCorners(List<Point> i_corners) {
            PointF massCenter = GetMassCenter(i_corners);
            Point[] top = new Point[2];
            Point[] bott = new Point[2];
            int top_pos = 0;
            int bott_pos = 0;

            if (i_corners.Count != 4)
                throw new ArgumentException("Four corners expected", "i_corners");

            foreach (Point c in i_corners) {
                if (c.Y < massCenter.Y)
                    top[top_pos++] = c;
                else
                    bott[bott_pos++] = c;
            }

            PointF[] result = {
                        top[0].X > top[1].X ? top[1] : top[0],      // Top-Left
                        top[0].X > top[1].X ? top[0] : top[1],      // Top-Right
                        bott[0].X > bott[1].X ? bott[0] : bott[1],  // Bottom-Right
                        bott[0].X > bott[1].X ? bott[1] : bott[0],  // Bottom-Left
                     };

            return result;
        }

    }
}
