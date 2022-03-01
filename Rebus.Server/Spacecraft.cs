// Ishan Pranav's REBUS: Spacecraft.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Rebus.Server
{
    [Table(nameof(Spacecraft))]
    [Guid("38F9F54B-5918-4A65-B3DA-817385E3C239")]
    internal sealed class Spacecraft : IEntity
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Q { get; set; }
        public int R { get; set; }

        [NotMapped]
        public HexPoint Location
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

#nullable disable
        [Required]
        public Concept Concept { get; set; }
#nullable enable

        public int ConceptId { get; set; }

        public void Write(ExpressionWriter writer)
        {
            using (writer.CreateScope(ScopeType.Parenthetical))
            {
                writer.Write(Id);
            }
        }
    }
}
