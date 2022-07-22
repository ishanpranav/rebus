// Ishan Pranav's REBUS: Map.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Rebus.Server
{
    public class Map : IEnumerable<HexPoint>
    {
        private readonly HexPoint _origin;
        private readonly Dictionary<HexPoint, int[]> _zones = new Dictionary<HexPoint, int[]>();

        public int Radius { get; }

        public Map(JuliaSet juliaSet, HexPoint origin, int radius, double zoom)
        {
            _origin = origin;
            Radius = radius;

            int depth = Enum.GetValues<Depth>().Length;

            juliaSet.MaxIterations = depth * 100;

            int size = radius * 2 + 1;

            foreach (HexPoint location in origin.Spiral(radius))
            {
                int index = (int)(juliaSet.Julia(location.Q, location.R, size, size, zoom) * depth);
                int layers;

                if (index == 0)
                {
                    layers = 0;
                }
                else
                {
                    layers = depth - index + 1;
                }

                _zones[location] = new int[layers];
            }

            int[] buffer = new int[depth];

            Array.Fill(buffer, value: 1);

            foreach (HexPoint location in HexPoint.Empty.Spiral(radius))
            {
                if (recurse(location))
                {
                    move(depth: 0);
                }
            }

            bool recurse(HexPoint location)
            {
                if (_zones.TryGetValue(location, out int[]? current) && current.Length > 1 && current[0] == 0)
                {
                    for (int i = 0; i < current.Length; i++)
                    {
                        current[i] = buffer[i];
                    }

                    move(current.Length - 1);

                    foreach (HexPoint neighbor in location.Neighbors())
                    {
                        recurse(neighbor);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            void move(int depth)
            {
                buffer[depth]++;

                Array.Fill(buffer, value: 1, depth + 1, buffer.Length - depth - 1);
            }
        }

        public Depth GetDepth(HexPoint location)
        {
            if (_zones.TryGetValue(location, out int[]? layers))
            {
                return (Depth)layers.Length;
            }
            else
            {
                return Depth.Unknown;
            }
        }

        public void Name(HexPoint location, Namer namer, out string? name, out Depth depth)
        {
            if (_zones.TryGetValue(location, out int[]? layers))
            {
                depth = (Depth)layers.Length;
                name = namer.Name(layers);
            }
            else
            {
                depth = Depth.Unknown;
                name = null;
            }
        }

        public IEnumerator<HexPoint> GetEnumerator()
        {
            return _origin
                .Spiral(Radius)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
