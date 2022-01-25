// Ishan Pranav's REBUS: ArgumentSignature.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    [Table(nameof(ArgumentSignature))]
    [Index(nameof(Argument), nameof(Type), nameof(Constraint), IsUnique = true)]
    internal sealed class ArgumentSignature : Writable
    {
        public int Id { get; set; }
        public Argument Argument { get; set; }
        public ArgumentType Type { get; set; }
        public Characteristics? Constraint { get; set; }

        public ICollection<CommandSignature> Commands { get; set; } = new HashSet<CommandSignature>();

        public override void Write(ExpressionWriter writer)
        {
            switch (Type)
            {
                case ArgumentType.Quotation:
                    writer.Write('"');
                    break;

                case ArgumentType.Reflexive:
                    writer.Write("MYSELF");
                    break;
            }

            writer.Write(Argument);

            switch (Type)
            {
                case ArgumentType.Quotation:
                    writer.Write('"');
                    break;

                case ArgumentType.Number:
                    writer.Write("(#)");
                    break;

                case ArgumentType.Concept:
                    writer.Write('(');
                    writer.Write(Constraint);
                    writer.Write(')');
                    break;
            }
        }
    }
}
