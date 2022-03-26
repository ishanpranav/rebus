// Ishan Pranav's REBUS: QueryableExtensions.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal static class QueryableExtensions
    {
        public static IQueryable<TConcept> AsWritable<TConcept>(this IQueryable<TConcept> source) where TConcept : Concept
        {
            return source
                .Include(x => x.Article)
                .Include(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Adjectives)
                .ThenInclude(x => x.Token);
        }
    }
}
