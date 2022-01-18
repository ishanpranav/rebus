//// Ishan Pranav's REBUS: SensoryRepository.cs
//// Copyright (c) Ishan Pranav. All Rights Reserved.
//// Licensed under the MIT License.

//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;

//namespace Rebus.Server
//{
//    internal class SensoryRepository<TContext> where TContext : DbContext
//    {
//        private readonly DijkstraPathfindingAlgorithm _pathfindingAlgorithm = new DijkstraPathfindingAlgorithm();
//        private readonly IDbContextFactory<TContext> _contextFactory;

//        public SensoryRepository(IDbContextFactory<TContext> contextFactory)
//        {
//            this._contextFactory = contextFactory;
//        }

//        public IReadOnlyCollection<Concept> FindPath(Concept source, Concept target)
//        {
//            using (TContext context = this._contextFactory.CreateDbContext())
//            {
//                return this._pathfindingAlgorithm
//                    .EnumerateSteps(
//                        context
//                           .Set<Concept>()
//                           .Include(x => x.Container)
//                           .Include(x => x.Contents),
//                        source,
//                        target)
//                    .ToArray();
//            }
//        }
//    }
//}
