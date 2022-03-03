// Ishan Pranav's REBUS: QueryableExtensions.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal static class QueryableExtensions
    {
        public static IQueryable<TConcept> IncludeSignatures<TConcept>(this IQueryable<TConcept> source) where TConcept : Concept
        {
            return source
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Article)
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Adjectives);
        }
    }
}
