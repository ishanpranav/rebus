// Ishan Pranav's REBUS: JuliaSet.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Numerics;

namespace Rebus.Server
{
    /// <summary>
    /// Performs the Julia set function.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Julia_set">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Julia_set">Julia set - Wikipedia</seealso>
    public class JuliaSet
    {
        /// <summary>
        /// Gets or sets the maximum depth of iteration.
        /// </summary>
        /// <value>The maximum depth of iteration. The default is <see cref="int.MaxValue"/>.</value>
        public int MaxIterations { get; set; } = int.MaxValue;

        // R = escape radius  # choose R > 0 such that R**2 - R >= sqrt(cx**2 + cy**2)

        /// <summary>
        /// Gets the complex parameter <em>c</em>.
        /// </summary>
        /// <value>The complex parameter <em>c</em>.</value>
        public Complex C { get; }

        /// <summary>
        /// Gets the escape radius <em>R</em>.
        /// </summary>
        /// <value>The escape radius <em>R</em>.</value>
        public double R { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JuliaSet"/> class.
        /// </summary>
        /// <param name="c">The complex parameer <em>c</em>.</param>
        /// <param name="r">The escape radius <em>R</em>. The value of <em>R</em> must be greater than zero, while <em>R</em>-squared minus <em>R</em> must be greater than or equal to the magnitude of the complex parameter <em>c</em>.</param>
        public JuliaSet(Complex c, double r)
        {
            C = c;
            R = r;

            if (r < 0)
            {
                throw new ArgumentException("The escape radius (R) must be greater than zero.", nameof(r));
            }
            else if (r * r - r < C.Magnitude)
            {
                throw new ArgumentException("The escape radius squared (R-squared) must be greater than or equal to the magnitude of the complex parameter (c).", nameof(c));
            }
        }

        /// <summary>
        /// Performs the Julia set function.
        /// </summary>
        /// <param name="x">The <em>x</em> coordinate of the point.</param>
        /// <param name="y">The <em>y</em> coordinate of the point.</param>
        /// <param name="width">The width of the fractal map.</param>
        /// <param name="height">The height of the fractal map.</param>
        /// <returns>The intensity of the point.</returns>
        public double Julia(int x, int y, int width, int height)
        {
            return Julia(x, y, width, height, zoom: 1);
        }

        /// <inheritdoc cref="Julia(int, int, int, int)"/>
        /// <param name="zoom">The zoom factor of the fractal map.</param>
        public double Julia(int x, int y, int width, int height, double zoom)
        {
            // zx = scaled x coordinate of pixel # (scale to be between -R and R)
            // zx represents the real part of z.
            // zy = scaled y coordinate of pixel # (scale to be between -R and R)
            // zy represents the imaginary part of z.

            double halfWidth = width / 2d;
            double halfHeight = height / 2d;

            Complex z = new Complex(1.5 * (x - halfWidth) / (halfWidth * zoom), (y - halfHeight) / (halfHeight * zoom));

            // iteration = 0
            // max_iteration = 1000

            // while (zx * zx + zy * zy < R**2  AND  iteration < max_iteration)
            //   xtemp = zx * zx - zy * zy
            //   zy = 2 * zx * zy  + cy 
            //   zx = xtemp + cx
            //   iteration = iteration + 1

            int i = 0;

            while (z.Magnitude < R && i < MaxIterations)
            {
                z = z * z + C;

                i++;
            }

            if (i == MaxIterations)
            {
                return 0;
            }
            else
            {
                return (double)i / MaxIterations;
            }
        }
    }
}
