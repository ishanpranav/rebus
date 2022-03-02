// Ishan Pranav's REBUS: HexPoint.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus
{
    /// <summary>
    /// Represents a cubic coordinate used to identify tiles in a grid of hexagons.
    /// </summary>
    /// <remarks>
    /// The implementation of this structure was inspired by and based on <see href="https://www.redblobgames.com/grids/hexagons/">this</see> article by <see href="http://www-cs-students.stanford.edu/~amitp/">Amit Patel</see>.
    /// </remarks>
    /// <seealso href="https://www.redblobgames.com/grids/hexagons/">Red Blob Games - Hexagonal Grids</seealso>
    /// <seealso href="http://www-cs-students.stanford.edu/~amitp/">Amit Patel's Home Page</seealso>
    public readonly struct HexPoint : IEquatable<HexPoint>, IFormattable
    {
        public static readonly HexPoint One = new HexPoint(1, 0);
        public static readonly HexPoint Two = new HexPoint(1, -1);
        public static readonly HexPoint Three = new HexPoint(0, -1);
        public static readonly HexPoint Four = new HexPoint(-1, 0);
        public static readonly HexPoint Five = new HexPoint(-1, 1);
        public static readonly HexPoint Six = new HexPoint(0, 1);

        public int Q { get; }
        public int R { get; }

        public int S
        {
            get
            {
                return -Q - R;
            }
        }

        public HexPoint(int q, int r)
        {
            Q = q;
            R = r;
        }

        public override string ToString()
        {
            return ToString(format: null, formatProvider: null);
        }

        public string ToString(string? format)
        {
            return ToString(format, formatProvider: null);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            switch (format)
            {
                case "a":
                case "A":
                    return string.Format(formatProvider, "({0}, {1})", Q, R);

                default:
                    return string.Format(formatProvider, "({0}, {1}, {2})", Q, R, S);
            }
        }

        public HexPoint Add(HexPoint other)
        {
            return new HexPoint(Q + other.Q, R + other.R);
        }

        public HexPoint Subtract(HexPoint other)
        {
            return new HexPoint(Q - other.Q, R - other.R);
        }

        public HexPoint Multiply(int factor)
        {
            return new HexPoint(Q * factor, R * factor);
        }

        public int Length()
        {
            return (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2;
        }

        public IEnumerable<HexPoint> Neighbors()
        {
            yield return this + One;
            yield return this + Two;
            yield return this + Three;
            yield return this + Four;
            yield return this + Five;
            yield return this + Six;
        }

        public static int Distance(HexPoint value1, HexPoint value2)
        {
            return (value1 - value2).Length();
        }

        public override bool Equals(object? obj)
        {
            return obj is HexPoint other && Equals(other);
        }

        public bool Equals(HexPoint other)
        {
            return other.Q == Q && other.R == R;
        }

        public override int GetHashCode()
        {
            HashCode result = new HashCode();

            result.Add(Q);
            result.Add(R);

            return result.ToHashCode();
        }

        public static bool operator ==(HexPoint left, HexPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HexPoint left, HexPoint right)
        {
            return !(left == right);
        }

        public static HexPoint operator +(HexPoint left, HexPoint right)
        {
            return left.Add(right);
        }

        public static HexPoint operator -(HexPoint left, HexPoint right)
        {
            return left.Subtract(right);
        }

        public static HexPoint operator *(HexPoint left, int right)
        {
            return left.Multiply(right);
        }
    }
}
