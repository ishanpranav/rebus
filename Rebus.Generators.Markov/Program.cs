// Ishan Pranav's REBUS: Program.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.IO;

namespace Rebus.Generators.Markov
{
    internal sealed class Program
    {
        private static void Main()
        {
            Console.Write("Random seed (or nothing for the shared random): ");

            Random random;

            if (int.TryParse(Console.ReadLine(), out int seed))
            {
                random = new Random(seed);
            }
            else
            {
                random = Random.Shared;
            }

            MarkovGenerator generator = new MarkovGenerator(random);

            Console.Write("Path to sources: ");

            using (StreamReader streamReader = File.OpenText(Console.ReadLine() ?? string.Empty))
            {
                string? line;

                while ((line = streamReader.ReadLine()) is not null)
                {
                    generator.Add(line
                        .Trim()
                        .ToLowerInvariant());
                }
            }

            while (Console.ReadLine() is not null)
            {
                try
                {
                    Console.Write(generator.Generate());
                }
                catch (Exception exception)
                {
                    Console.Write(exception.Message);
                }
            }
        }
    }
}
