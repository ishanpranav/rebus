// Ishan Pranav's REBUS: HexPoint.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

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
    public readonly struct HexPoint : IEquatable<HexPoint>, IFormattable, IWritable
    {
        public static readonly HexPoint Zero;

        private static readonly HexPoint[] s_directions = new HexPoint[]
        {
            new HexPoint(1, 0),
            new HexPoint(1, -1),
            new HexPoint(0, -1),
            new HexPoint(-1, 0),
            new HexPoint(-1, 1),
            new HexPoint(0, 1)
        };

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

        public IEnumerable<HexPoint> Neighbors()
        {
            for (int i = 0; i < s_directions.Length; i++)
            {
                yield return this + s_directions[i];
            }
        }

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

        public IEnumerable<HexPoint> Ring(int radius)
        {
            HexPoint point = this + (s_directions[4] * radius);

            foreach (HexPoint direction in s_directions)
            {
                for (int i = 0; i < radius; i++)
                {
                    yield return point;

                    point += direction;
                }
            }
        }

        public IEnumerable<HexPoint> Spiral(int radius)
        {
            yield return this;

            for (int i = 1; i <= radius; i++)
            {
                foreach (HexPoint point in Ring(i))
                {
                    yield return point;
                }
            }
        }

        public static int Distance(HexPoint value1, HexPoint value2)
        {
            HexPoint difference = value1 - value2;

            return (Math.Abs(difference.Q) + Math.Abs(difference.R) + Math.Abs(difference.S)) / 2;
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

        public static bool TryParse(string value, out HexPoint result)
        {
            try
            {
                int state = 0;
                int qSign = 1;
                int rSign = 1;
                int qAbs = 0;
                int rAbs = 0;
                int qFactor = 1;
                int rFactor = 1;

                for (int i = value.Length - 1; i >= 0; i--)
                {
                    char current = char.ToUpper(value[i]);

                    switch (state)
                    {
                        case 0:
                            if (current >= 'A' && current <= 'C')
                            {
                                rSign = -1;
                            }

                            switch (current)
                            {
                                case 'A':
                                case 'D':
                                case 'G':
                                    qSign = -1;
                                    break;
                            }

                            if (i > 0 && char.IsDigit(value[i - 1]))
                            {
                                state++;

                                break;
                            }
                            else
                            {
                                result = Zero;

                                return false;
                            }

                        case 1:
                            if (char.IsDigit(current) && i > 0)
                            {
                                rAbs += (current - '0') * rFactor;
                                rFactor *= 10;

                                if (char.IsLetter(value[i - 1]))
                                {
                                    state++;
                                }

                                break;
                            }
                            else
                            {
                                result = Zero;

                                return false;
                            }

                        case 2:
                            if (char.IsLetter(current))
                            {
                                if (current != 'Z')
                                {
                                    qAbs += (current - 'A' + 1) * qFactor;
                                }

                                if (i == 0)
                                {
                                    result = new HexPoint(qSign * qAbs, rSign * rAbs);

                                    return true;
                                }
                                else
                                {
                                    qFactor *= 26;
                                }
                            }
                            else
                            {
                                result = Zero;

                                return false;
                            }
                            break;
                    }
                }
            }
            catch (ArithmeticException) { }

            result = Zero;

            return false;
        }

        public void Write(ExpressionWriter writer)
        {
            StringBuilder result = new StringBuilder();
            int q = Math.Abs(Q);
            int ones;

            do
            {
                ones = q % 26;

                if (ones == 0)
                {
                    result.Insert(index: 0, 'Z');
                }
                else
                {
                    result.Insert(index: 0, (char)(ones + 'A' - 1));
                }

                q = (q - ones) / 26;
            }
            while (q > 0);

            writer.Write(result
                .Append('-')
                .Append(Math.Abs(R))
                .Append((char)('a' + 4 + Math.Sign(Q) + (3 * Math.Sign(R)))));
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
