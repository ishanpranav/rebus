// Ishan Pranav's REBUS: IPathfinderFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines a method for creating a <see cref="IPathfinder{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of each node in the graphs searched by the pathfinder.</typeparam>
    public interface IPathfinderFactory<T>
    {
        /// <summary>
        /// Creates a pathfinder.
        /// </summary>
        /// <param name="playerId">The player.</param>
        /// <returns>A new pathfinder instance.</returns>
        IPathfinder<T> CreatePathfinder(int playerId);
    }
}
