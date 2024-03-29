﻿// Ishan Pranav's REBUS: Map.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rebus.Server
{
    internal sealed class Map
    {
        private readonly int _size;
        private readonly Dictionary<HexPoint, int[]> _zones = new Dictionary<HexPoint, int[]>();

        public JuliaSet JuliaSet { get; }
        public HexPoint Origin { get; }
        public int Radius { get; }
        public int Depth { get; }
        public double Zoom { get; }
        public IReadOnlyList<Biome> Biomes { get; }

        public Map(JuliaSet juliaSet, HexPoint origin, int radius, int depth, double zoom, IReadOnlyList<Biome> biomes)
        {
            JuliaSet = juliaSet;
            Origin = origin;
            _size = (radius * 2) + 1;
            Radius = radius;
            Depth = depth;
            Zoom = zoom;
            Biomes = biomes;

            int[] buffer = new int[depth];

            foreach (HexPoint location in origin.Range(radius))
            {
                int index = (int)JuliaDepth(location);

                if (index > 0)
                {
                    _zones.Add(location, new int[depth - index + 1]);
                }
            }

            Array.Fill(buffer, Depths.FirstValue);

            foreach (HexPoint location in origin.Spiral(radius))
            {
                if (recurse(location))
                {
                    move(Depths.Constellation);
                }
            }

            bool recurse(HexPoint location)
            {
                if (_zones.TryGetValue(location, out int[]? layers) && layers.Length > Depths.Constellation && Depths.Layer(layers, Depths.Constellation) == Depths.EmptyValue)
                {
                    for (int i = 0; i < layers.Length; i++)
                    {
                        layers[i] = buffer[i];
                    }

                    move(layers.Length);

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
                buffer[depth - 1]++;

                Array.Fill(buffer, value: 1, depth, buffer.Length - depth);
            }
        }

        public bool Contains(HexPoint location)
        {
            return HexPoint.Distance(location, Origin) <= Radius;
        }

        private double JuliaDepth(HexPoint location)
        {
            return JuliaSet.Julia(location.Q, location.R, _size, _size, Zoom) * Depth;
        }

        public bool TryGetLayers(HexPoint location, [MaybeNullWhen(false)] out IReadOnlyList<int> layers)
        {
            if (_zones.TryGetValue(location, out int[]? results))
            {
                layers = results;

                return true;
            }
            else if (Contains(location))
            {
                layers = Array.Empty<int>();

                return true;
            }
            else
            {
                layers = null;

                return false;
            }
        }

        public Biome GetBiome(HexPoint location, IReadOnlyList<int> layers)
        {
            return Biomes[getIndex()];

            int getIndex()
            {
                switch (layers.Count)
                {
                    case Depths.Star:
                        return 1;

                    case Depths.Planet:
                        return (int)((2 - JuliaDepth(location)) * (Biomes.Count - 3)) + 2;

                    default:
                        return 0;
                }
            }
        }
    }
}
