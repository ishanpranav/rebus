// Ishan Pranav's REBUS: Concept.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(Concept))]
    [Index(nameof(Q), nameof(R))]
    internal abstract class Concept : Writable
    {
        public int Id { get; set; }
        public int Q { get; set; }
        public int R { get; set; }

        [NotMapped]
        public HexPoint Region
        {
            get
            {
                return new HexPoint(Q, R);
            }
            set
            {
                Q = value.Q;
                R = value.R;
            }
        }

        public Token? Article { get; set; }

#nullable disable
        [Required]
        public Token Substantive { get; set; }
#nullable enable

        public string SubstantiveValue { get; set; } = string.Empty;

        public ICollection<Adjective> Adjectives { get; set; } = new HashSet<Adjective>();

        public Player? Player { get; set; }
        public int? PlayerId { get; set; }

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
