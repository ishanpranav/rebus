// Ishan Pranav's REBUS: DijkstraPathfinder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.Server.Pathfinders
{
    /// <summary>
    /// Performs Dijkstra&apos;s algorithm to find paths between graph nodes.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/A*_search_algorithm">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm">Dijkstra&apos;s algorithm - Wikipedia</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/A*_search_algorithm">A* search algorithm - Wikipedia</seealso>
    /// <typeparam name="T">The type of each node in the graph.</typeparam>
    public abstract class DijkstraPathfinder<T> : AStarPathfinder<T> where T : notnull, IEquatable<T>
    {
        /// <inheritdoc/>
        protected override int Heuristic(T source, T destination)
        {
            return 0;
        }
    }
}
