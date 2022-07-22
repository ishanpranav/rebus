// Ishan Pranav's REBUS: Namer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Rebus.Server.NumeralSystems;

namespace Rebus.Server
{
    public class Namer
    {
        private static readonly RomanNumeralSystem s_romanNumeralSystem = new RomanNumeralSystem();
        private static readonly BayerNumeralSystem s_bayerNumeralSystem = new BayerNumeralSystem();

        private readonly IList<string> _constellations;
        private readonly Queue<string> _stars = new Queue<string>();
        private readonly Queue<string> _planets = new Queue<string>();
        private readonly Dictionary<(int, int), string> _existingStars = new Dictionary<(int, int), string>();
        private readonly Dictionary<(int, int, int), string> _existingPlanets = new Dictionary<(int, int, int), string>();

        private int _starIndex;

        public Namer(FisherYatesShuffler shuffler, IList<string> constellations, IList<string> stars, IList<string> planets)
        {
            shuffler.Shuffle(constellations);
            shuffler.Shuffle(stars);
            shuffler.Shuffle(planets);

            _constellations = constellations;
            _stars = new Queue<string>(stars);
            _planets = new Queue<string>(planets);
        }

        public string Name(IReadOnlyList<int> layers)
        {
            Depth depth = (Depth)layers.Count;

            int getLayer(Depth depth)
            {
                return layers[(int)depth - 1] - 1;
            }

            if (depth > Depth.Constellation && depth <= Depth.Planet)
            {
                int constellation = getLayer(Depth.Constellation);
                int star = getLayer(Depth.Star);

                if (layers.Count > 2)
                {
                    int planet = getLayer(Depth.Planet);

                    if (_existingPlanets.TryGetValue((constellation, star, planet), out string? planetName))
                    {
                        return planetName;
                    }
                    else if (tryNameStar(out string? starName))
                    {
                        return $"{starName} {namePlanet()}";
                    }
                    else if (_planets.TryDequeue(out planetName))
                    {
                        _existingPlanets[(constellation, star, planet)] = planetName;

                        return planetName;
                    }
                    else
                    {
                        return $"{nameStar()} {namePlanet()}";
                    }

                    string namePlanet()
                    {
                        int ordinal = planet + 1;

                        if (!s_romanNumeralSystem.TryGetNumeral(ordinal, out string? value))
                        {
                            value = ordinal.ToString();
                        }

                        return value;
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
                    _starIndex++;

                    string result = $"SS-{_starIndex}";

                    _existingStars[(constellation, star)] = result;

                    return result;
                };

                bool tryNameStar([NotNullWhen(true)] out string? name)
                {
                    if (_existingStars.TryGetValue((constellation, star), out name)) { }
                    else if (tryBayer(out name)) { }
                    else if (_stars.TryDequeue(out name))
                    {
                        _existingStars[(constellation, star)] = name;
                    }
                    else
                    {
                        return false;
                    }

                    return true;

                    bool tryBayer([NotNullWhen(true)] out string? starName)
                    {
                        if (constellation < _constellations.Count && s_bayerNumeralSystem.TryGetNumeral(star, out string? result))
                        {
                            starName = $"{result} {_constellations[constellation]}";

                            return true;
                        }
                        else
                        {

                            starName = null;

                            return false;
                        }
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
