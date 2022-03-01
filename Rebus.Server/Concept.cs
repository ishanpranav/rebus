// Ishan Pranav's REBUS: Concept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Rebus.Server
{
    [Table(nameof(Concept))]
    internal sealed class Concept : Writable, IConcept
    {
        public int Id { get; set; }

        public ICollection<ConceptSignature> Signatures { get; set; } = new HashSet<ConceptSignature>();

        public Player? Player { get; set; }
        public Spacecraft? Spacecraft { get; set; }

        private IEnumerable<object?> GetAll()
        {
            yield return Player;
            yield return Spacecraft;
        }

        public bool Is<T>() where T : class
        {
            foreach (object? item in GetAll())
            {
                if (item is T)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Is<T>(out T? result) where T : class
        {
            foreach (object? item in GetAll())
            {
                if (item is T t)
                {
                    result = t;

                    return true;
                }
            }

            result = null;

            return false;
        }

        public override void Write(ExpressionWriter writer)
        {
            Signatures
                .OrderByDescending(x => x.Substantive.Value.Length)
                .ThenByDescending(x => x.Adjectives.Count)
                .FirstOrDefault()?
                .Write(writer);
        }
    }
}
