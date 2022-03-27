// Ishan Pranav's REBUS: IPathfinder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    /// <summary>
    /// Defines a method for finding paths between graph nodes.
    /// </summary>
    /// <typeparam name="T">The type of each node in the graph.</typeparam>
    public interface IPathfinder<T>
    {
        /// <summary>
        /// Performs the search.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="destination">The destination node.</param>
        /// <returns>A stack containing the ordered set of nodes leading from the <paramref name="source"/> node to the <paramref name="destination"/> node, or an empty stack if no such path exists.</returns>
        Stack<T> Search(T source, T destination);
    }
}
