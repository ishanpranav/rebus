﻿// Ishan Pranav's REBUS: Namer.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Humanizer;

namespace Rebus.Server
{
    internal sealed class Namer
    {
        private readonly int _starCount;
        private readonly int _planetCount;
        private readonly Queue<string> _constellations;
        private readonly Queue<string> _stars;
        private readonly Queue<string> _planets;
        private readonly Dictionary<int, string> _existingConstellations = new Dictionary<int, string>();
        private readonly Dictionary<(int, int), string> _existingStars = new Dictionary<(int, int), string>();
        private readonly Dictionary<(int, int, int), string> _existingPlanets = new Dictionary<(int, int, int), string>();
        private readonly FisherYatesShuffle _shuffle;

        private int _starIndex;
        private int _planetIndex;

        public Namer(FisherYatesShuffle shuffle, IList<string> constellations, IList<string> stars, IList<string> planets)
        {
            shuffle.Shuffle(constellations);
            shuffle.Shuffle(stars);
            shuffle.Shuffle(planets);

            _shuffle = shuffle;
            _constellations = new Queue<string>(constellations);
            _stars = new Queue<string>(stars);
            _planets = new Queue<string>(planets);
            _starCount = _stars.Count;
            _planetCount = _planets.Count;
        }

        public string? Name(IReadOnlyList<int> layers)
        {
            int depth = layers.Count;

            if (depth > Depths.Constellation && depth <= Depths.Planet)
            {
                int constellation = Depths.Layer(layers, Depths.Constellation);
                int star = Depths.Layer(layers, Depths.Star);

                if (depth > Depths.Star)
                {
                    int planet = Depths.Layer(layers, Depths.Planet);

                    if (_existingPlanets.TryGetValue((constellation, star, planet), out string? planetName))
                    {
                        return planetName;
                    }
                    else
                    {
                        const int maxRoman = 4000;

                        if (_shuffle.Random.NextDouble() < ((double)_planets.Count / _planetCount) && _planets.TryDequeue(out planetName)) { }
                        else if (tryNameStar(out string? starName))
                        {
                            int ordinal = planet + 1;
                            string numeral;

                            if (ordinal < maxRoman)
                            {
                                numeral = ordinal.ToRoman();
                            }
                            else
                            {
                                numeral = ordinal.ToString();
                            }

                            planetName = $"{starName} {numeral}";
                        }
                        else if (_planets.TryDequeue(out planetName)) { }
                        else
                        {
                            int ordinal = planet + 1;

                            if (ordinal < maxRoman)
                            {
                                planetName = $"{nameStar()} {ordinal.ToRoman()}";
                            }
                            else
                            {
                                _planetIndex++;

                                planetName = $"P-{_planetIndex}";
                            }
                        }

                        _existingPlanets.Add((constellation, star, planet), planetName);

                        return planetName;
                    }
                }
                else if (tryNameStar(out string? starName))
                {
                    return starName;
                }
                else
                {
                    return nameStar();
                }

                string nameStar()
                {
                    string result;

                    if (tryNameConstellation(out string? constellationName))
                    {
                        result = $"{star} {constellationName}";
                    }
                    else
                    {
                        _starIndex++;

                        result = $"S-{_starIndex}";
                    }

                    _existingStars.Add((constellation, star), result);

                    return result;
                }

                bool tryNameStar([MaybeNullWhen(false)] out string starName)
                {
                    if (_existingStars.TryGetValue((constellation, star), out starName))
                    {
                        return true;
                    }
                    else if (_shuffle.Random.NextDouble() < ((double)_stars.Count / _starCount) && _stars.TryDequeue(out starName)) { }
                    else if (tryNameConstellation(out string? constellationName) && tryNumberStar(star, out string? starNumber))
                    {
                        starName = $"{starNumber} {constellationName}";
                    }
                    else if (_stars.TryDequeue(out starName)) { }
                    else
                    {
                        return false;
                    }

                    _existingStars.Add((constellation, star), starName);

                    return true;

                    bool tryNumberStar(int value, [MaybeNullWhen(false)] out string result)
                    {
                        const char greekMin = '\u03b1';
                        const char greekMax = '\u03c9';
                        const int greekCount = greekMax - greekMin + 1;

                        if (value < greekCount)
                        {
                            result = ((char)(greekMin + value)).ToString();
                        }
                        else
                        {
                            value -= greekCount;

                            const char latinMin = 'A';

                            if (value <= 'Z' - latinMin)
                            {
                                result = ((char)(latinMin + value)).ToString();
                            }
                            else
                            {
                                result = null;

                                return false;
                            }
                        }

                        return true;
                    }
                }

                bool tryNameConstellation([MaybeNullWhen(false)] out string constellationName)
                {
                    if (_existingConstellations.TryGetValue(constellation, out constellationName)) { }
                    else if (_constellations.TryDequeue(out constellationName))
                    {
                        _existingConstellations.Add(constellation, constellationName);
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
