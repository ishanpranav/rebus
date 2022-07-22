// Ishan Pranav's REBUS: Layout.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using SkiaSharp;

namespace Rebus.Client
{
    /// <summary>
    /// Represents a grid of hexagons.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://www.redblobgames.com/grids/hexagons/">this</see> article by <see href="http://www-cs-students.stanford.edu/~amitp/">Amit Patel</see>.
    /// </remarks>
    /// <seealso href="https://www.redblobgames.com/grids/hexagons/">Red Blob Games - Hexagonal Grids</seealso>
    /// <seealso href="http://www-cs-students.stanford.edu/~amitp/">Amit Patel&apos;s Home Page</seealso>
    public class Layout
    {
        private const int HexagonEdges = 6;
        private const float Theta0 = MathF.PI / HexagonEdges;
        private const float ThetaI = MathF.Tau / HexagonEdges;

        private static readonly float s_sqrt3 = MathF.Sqrt(3);

        /// <summary>
        /// Gets the <em>x</em> coordinate of the origin.
        /// </summary>
        /// <value>The <em>x</em> coordinate of the origin.</value>
        public float X { get; }

        /// <summary>
        /// Gets the <em>y</em> coordinate of the origin.
        /// </summary>
        /// <value>The <em>y</em> coordinate of the origin.</value>
        public float Y { get; }

        /// <summary>
        /// Gets or sets the width factor of a hexagon.
        /// </summary>
        /// <value>The width factor of a hexagon.</value>
        public float HexagonWidth { get; set; }

        /// <summary>
        /// Gets or sets the height factor of a hexagon.
        /// </summary>
        /// <value>The height factor of a hexagon.</value>
        public float HexagonHeight { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexPoint"/> class.
        /// </summary>
        /// <param name="x">The <em>x</em> coordinate of the origin.</param>
        /// <param name="y">The <em>y</em> coordinate of the origin.</param>
        public Layout(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the point representing the center of the hexagon at the given coordinate.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <returns>The center of the hexagon.</returns>
        public SKPoint GetCenter(HexPoint value)
        {
            return new SKPoint(HexagonWidth * (s_sqrt3 * value.Q + s_sqrt3 * 0.5f * value.R) + X, HexagonHeight * (1.5f * value.R) + Y);
        }

        /// <summary>
        /// Gets the path representing the hexagon at the given coordinate.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <returns>The hexagon.</returns>
        public SKPath GetHexagon(HexPoint value)
        {
            SKPath result = new SKPath();
            SKPoint center = GetCenter(value);
            SKPoint first = getVertex(n: 0);

            result.MoveTo(first);

            for (int i = 1; i < HexagonEdges; i++)
            {
                result.LineTo(getVertex(i));
            }

            result.Close();

            return result;

            SKPoint getVertex(int n)
            {
                (float sin, float cos) = MathF.SinCos(n * ThetaI + Theta0);

                return new SKPoint((int)(center.X + HexagonWidth * cos), (int)(center.Y + HexagonHeight * sin));
            }
        }

        /// <summary>
        /// Gets the hexagonal coordinate corresponding to the Cartesian coordinate.
        /// </summary>
        /// <param name="x">The Cartesian <em>x</em> coordinate.</param>
        /// <param name="y">The Cartesian <em>y</em> coordinate.</param>
        /// <returns>The hexagonal coordinate.</returns>
        public HexPoint GetHexPoint(float x, float y)
        {
            y = (y - Y) / HexagonHeight;

            float qF = s_sqrt3 / 3 * ((x - X) / HexagonWidth) - y / 3;
            float rF = 2 * y / 3;
            float sF = -qF - rF;

            int q = (int)MathF.Round(qF);
            int r = (int)MathF.Round(rF);
            int s = (int)MathF.Round(sF);

            float qError = Math.Abs(q - qF);
            float rError = Math.Abs(r - rF);
            float sError = Math.Abs(s - sF);

            if (qError > rError && qError > sError)
            {
                q = -r - s;
            }
            else if (rError > sError)
            {
                r = -q - s;
            }

            return new HexPoint(q, r);
        }
    }
}
