// Ishan Pranav's REBUS: MarkovGenerator.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Markov;

namespace Rebus.Generators.Markov
{
    public class MarkovGenerator : IGenerator
    {
        private readonly Random _random;
        private readonly MarkovChain<char> _chain = new MarkovChain<char>(2);
        private readonly HashSet<string> _previousResults = new HashSet<string>();

        public MarkovGenerator(Random random)
        {
            _random = random;
        }

        public void Add(string value)
        {
            if (_previousResults.Add(value))
            {
                _chain.Add(value);
            }
        }

        public string Generate()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string result;
            int iterations = 0;

            do
            {
                foreach (char value in _chain.Chain(_random))
                {
                    stringBuilder.Append(value);
                }

                result = stringBuilder.ToString();

                stringBuilder.Clear();

                iterations++;
            }
            while (_previousResults.Contains(result) && iterations < 1024);

            if (iterations == 256)
            {
                throw new Exception("Cannot create a unique string.");
            }
            else
            {
                _previousResults.Add(result);

                return result;
            }
        }
    }
}
