// Ishan Pranav's REBUS: QueryableExtensions.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Rebus.Server
{
    internal static class QueryableExtensions
    {
        public static IIncludableQueryable<TEntity, ICollection<Token>> IncludeConcept<TEntity>(this IQueryable<TEntity> source) where TEntity : class, IEntity
        {
            return source
                .Include(x => x.Concept)
                .ThenInclude(x => x.Signatures)
                .ThenInclude(x => x.Article)
                .ThenInclude(x => x.Signatures)
                .ThenInclude(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Concept)
                .ThenInclude(x => x.Signatures)
                .ThenInclude(x => x.Adjectives);
        }
    }
}
