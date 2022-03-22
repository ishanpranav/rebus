// Ishan Pranav's REBUS: Concept.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Rebus.Server
{
    [Table(nameof(Concept))]
    internal abstract class Concept : Writable, IFeature
    {
        public int Id { get; set; }
        public abstract HexPoint Region { get; }

        public Token? Article { get; set; }

#nullable disable
        [Required]
        public Token Substantive { get; set; }

        [Required]
        public string SubstantiveValue { get; set; }
#nullable enable

        public ICollection<Adjective> Adjectives { get; set; } = new HashSet<Adjective>();

        public override void Write(ExpressionWriter writer)
        {
            if (Article is not null)
            {
                Article.Write(writer);

                writer.Write(' ');
            }

            foreach (IGrouping<TokenTypes, Token> grouping in Adjectives
                .Select(x => x.Token)
                .GroupBy(x => x.Type)
                .OrderBy(x => x.Key))
            {
                Token[] array = grouping.ToArray();
                int lastIndex = array.Length - 1;

                for (int i = 0; i < lastIndex; i++)
                {
                    array[i].Write(writer);

                    writer.Write(',');
                }

                array[lastIndex].Write(writer);

                writer.Write(' ');
            }

            Substantive.Write(writer);
        }
    }
}
