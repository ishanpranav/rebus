// Ishan Pranav's REBUS: Commodity.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rebus
{
    [Index(nameof(Mass), IsUnique = true)]
    [Table(nameof(Commodity))]
    public class Commodity
    {
        public int Id { get; set; }
        public double Mass { get; set; }
    }
}
