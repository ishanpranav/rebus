// Ishan Pranav's REBUS: HexPoint.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rebus
{
    /// <summary>
    /// Represents a cubic coordinate in three dimensions (<em>Q</em>, <em>R</em>, and <em>S</em>) used to identify tiles in a grid of hexagons.
    /// </summary>
    /// <remarks>
    /// The implementation of this struct was inspired by and based on <see href="https://www.redblobgames.com/grids/hexagons/">this</see> article by <see href="http://www-cs-students.stanford.edu/~amitp/">Amit Patel</see>.
    /// </remarks>
    /// <seealso href="https://www.redblobgames.com/grids/hexagons/">Red Blob Games - Hexagonal Grids</seealso>
    /// <seealso href="http://www-cs-students.stanford.edu/~amitp/">Amit Patel&apos;s Home Page</seealso>
    [DataContract]
    public readonly struct HexPoint : IComparable, IComparable<HexPoint>, IEquatable<HexPoint>
    {
        private static readonly HexPoint[] s_directions = new HexPoint[]
        {
            new HexPoint(1, 0),
            new HexPoint(1, -1),
            new HexPoint(0, -1),
            new HexPoint(-1, 0),
            new HexPoint(-1, 1),
            new HexPoint(0, 1)
        };

        /// <summary>
        /// Specifies the coordinate whose three axes are zero.
        /// </summary>
        /// <value>The coordinate (0, 0, 0).</value>
        public static readonly HexPoint Empty;

        /// <summary>
        /// Gets the <em>Q</em> coordinate.
        /// </summary>
        /// <value>The <em>Q</em> coordinate. The default is 0.</value>
        [DataMember(Order = 0)]
        public int Q { get; }

        /// <summary>
        /// Gets the <em>R</em> coordinate.
        /// </summary>
        /// <value>The <em>R</em> coordinate. The default is 0.</value>
        [DataMember(Order = 1)]
        public int R { get; }

        /// <summary>
        /// Gets the <em>S</em> coordinate.
        /// </summary>
        /// <value>The <em>S</em> coordinate. The default is 0.</value>
        public int S
        {
            get
            {
                return -Q - R;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexPoint"/> struct.
        /// </summary>
        /// <param name="q">The <em>Q</em> coordinate.</param>
        /// <param name="r">The <em>R</em> coordinate.</param>
        public HexPoint(int q, int r)
        {
            Q = q;
            R = r;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({Q}, {R}, {S})";
        }

        /// <summary>
        /// Returns a new <see cref="HexPoint"/> that adds the value of the specified <see cref="HexPoint"/> to the value of this instance.
        /// </summary>
        /// <param name="other">A positive or negative hexagonal coordinate.</param>
        /// <returns>An object whose value is the sum of the hexagonal coordinates represented by this instance and the <paramref name="other"/> value.</returns>
        public HexPoint Add(HexPoint other)
        {
            return new HexPoint(Q + other.Q, R + other.R);
        }

        /// <summary>
        /// Returns the value that results from subtracting the specified hexagonal coordinate from the value of this instance.
        /// </summary>
        /// <param name="other">The hexagonal coordinate to subtract.</param>
        /// <returns>A hexagonal coordinate that is equal to this instance minus the <paramref name="other"/> value.</returns>
        public HexPoint Subtract(HexPoint other)
        {
            return new HexPoint(Q - other.Q, R - other.R);
        }

        /// <summary>
        /// Returns the value that results from multiplying the specified factor by the value of this instance.
        /// </summary>
        /// <param name="factor">The factor to multiply.</param>
        /// <returns>A hexagonal coordinate that is equal to this instance times the <paramref name="factor"/>.</returns>
        public HexPoint Multiply(int factor)
        {
            return new HexPoint(Q * factor, R * factor);
        }

        /// <summary>
        /// Gets the six coordinates adjacent to this instance.
        /// </summary>
        /// <returns>The neighboring coordinates.</returns>
        public IEnumerable<HexPoint> Neighbors()
        {
            for (int i = 0; i < s_directions.Length; i++)
            {
                yield return this + s_directions[i];
            }
        }

        /// <summary>
        /// Gets the coordinates within a hexagonal range.
        /// </summary>
        /// <param name="radius">The radius of the range.</param>
        /// <returns>The coordinates within the range defined by the given <paramref name="radius"/>.</returns>
        public IEnumerable<HexPoint> Range(int radius)
        {
            for (int q = -radius; q <= radius; q++)
            {
                for (int r = Math.Max(-radius, -radius - q); r <= Math.Min(radius, radius - q); r++)
                {
                    yield return this + new HexPoint(q, r);
                }
            }
        }

        public IReadOnlyDictionary<HexPoint, int> Search(int radius, Func<HexPoint, bool> predicate)
        {
            Dictionary<HexPoint, int> results = new Dictionary<HexPoint, int>()
            {
                { this, 0 }
            };
            List<List<HexPoint>> fringes = new List<List<HexPoint>>()
            {
                new List<HexPoint>()
                {
                    this
                }
            };

            for (int k = 1; k <= radius; k++)
            {
                fringes.Add(new List<HexPoint>());

                foreach (HexPoint value in fringes[k - 1])
                {
                    foreach (HexPoint neighbor in value.Neighbors())
                    {
                        if (predicate(neighbor) && results.TryAdd(neighbor, k))
                        {
                            fringes[k].Add(neighbor);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets the coordinates within a hexagonal ring.
        /// </summary>
        /// <param name="radius">The radius of the ring.</param>
        /// <returns>The coordinates within the ring defined by the given <paramref name="radius"/>.</returns>
        public IEnumerable<HexPoint> Ring(int radius)
        {
            HexPoint value = this + (s_directions[4] * radius);

            foreach (HexPoint direction in s_directions)
            {
                for (int i = 0; i < radius; i++)
                {
                    yield return value;

                    value += direction;
                }
            }
        }

        /// <summary>
        /// Gets the coordinates within a hexagonal spiral.
        /// </summary>
        /// <param name="radius">The radius of the spiral.</param>
        /// <returns>The coordinates within the spiral defined by the given <paramref name="radius"/>.</returns>
        public IEnumerable<HexPoint> Spiral(int radius)
        {
            yield return this;

            for (int i = 1; i <= radius; i++)
            {
                foreach (HexPoint value in Ring(i))
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Gets the distance between two hexagonal coordinates.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The distance between <paramref name="value1"/> and <paramref name="value2"/>.</returns>
        public static int Distance(HexPoint value1, HexPoint value2)
        {
            HexPoint difference = value1 - value2;

            return (Math.Abs(difference.Q) + Math.Abs(difference.R) + Math.Abs(difference.S)) / 2;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is HexPoint other && Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(HexPoint other)
        {
            return other.Q == Q && other.R == R;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode result = new HashCode();

            result.Add(Q);
            result.Add(R);

            return result.ToHashCode();
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is HexPoint other)
            {
                return CompareTo(other);
            }
            else
            {
                throw new ArgumentException("Argument is not a HexPoint instance.", nameof(obj));
            }
        }

        /// <inheritdoc/>
        public int CompareTo(HexPoint other)
        {
            int result = Q.CompareTo(other.Q);

            if (result == 0)
            {
                result = R.CompareTo(R);
            }

            return result;
        }

        /// <summary>
        /// Returns a value that indicates whether two coordinates are equal.
        /// </summary>
        /// <param name="left">The first coordinate to compare.</param>
        /// <param name="right">The second coordinate to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(HexPoint left, HexPoint right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a value that indicates whether two coordinates are not equal.
        /// </summary>
        /// <param name="left">The first coordinate to compare.</param>
        /// <param name="right">The second coordinate to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(HexPoint left, HexPoint right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Adds a specified coordinate to another specified coordinate.
        /// </summary>
        /// <param name="left">The first coordinate to add.</param>
        /// <param name="right">The second coordinate to add.</param>
        /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
        public static HexPoint operator +(HexPoint left, HexPoint right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Subtracts a specified coordinate from another specified coordinate.
        /// </summary>
        /// <param name="left">The value to subtract from (the minuend).</param>
        /// <param name="right">The value to subtract (the subtrahend).</param>
        /// <returns>The result of subtracting <paramref name="right"/> from <paramref name="left"/>.</returns>
        public static HexPoint operator -(HexPoint left, HexPoint right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Multiplies a specified coordinate by a number.
        /// </summary>
        /// <param name="left">The coordinate to multiply.</param>
        /// <param name="right">The number to multiply.</param>
        /// <returns>The product of <paramref name="left"/> and <paramref name="right"/>, as a hexagonal coordinate.</returns>
        public static HexPoint operator *(HexPoint left, int right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Multiplies a number by a specified coordinate.
        /// </summary>
        /// <param name="left">The number to multiply.</param>
        /// <param name="right">The coordinate to multiply.</param>
        /// <returns>The product of <paramref name="left"/> and <paramref name="right"/>, as a hexagonal coordinate.</returns>
        public static HexPoint operator *(int left, HexPoint right)
        {
            return right.Multiply(left);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified value is less than another specified value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <(HexPoint left, HexPoint right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified value is less than or equal to another specified value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(HexPoint left, HexPoint right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified value is greater than another specified value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(HexPoint left, HexPoint right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified value is greater than or equal to another specified value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(HexPoint left, HexPoint right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
