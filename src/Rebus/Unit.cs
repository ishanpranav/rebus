// Ishan Pranav's REBUS: Unit.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [DataContract]
    [Index(nameof(Name), IsUnique = true)]
    [Table(nameof(Unit))]
    public class Unit : IEquatable<Unit>
    {
        [DataMember(Order = 0)]
        public int Id { get; set; }

        public int ZoneId { get; set; }

#nullable disable
        public Zone Zone { get; set; }
#nullable enable

        public int? CargoId { get; set; }
        public Commodity? Cargo { get; set; }

        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        public bool Equals(Unit? other)
        {
            return other is not null && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Unit);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
