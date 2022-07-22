// Ishan Pranav's REBUS: AStarSearch.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server.Searches
{
    /// <summary>
    /// Performs the A* search algorithm to find paths between graph nodes.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/A*_search_algorithm">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/A*_search_algorithm">A* search algorithm - Wikipedia</seealso>
    /// <typeparam name="T">The type of each node in the graph.</typeparam>
    public abstract class AStarSearch<T> : ISearch<T> where T : notnull, IEquatable<T>
    {
        /// <summary>
        /// Performs the heuristic function.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="destination">The destination node.</param>
        /// <returns>The estimated distance from the <paramref name="source"/> to the <paramref name="destination"/>.</returns>
        protected abstract int Heuristic(T source, T destination);

        /// <summary>
        /// Performs the cost function.
        /// </summary>
        /// <param name="value">The source node.</param>
        /// <param name="neighbor">The neighbor.</param>
        /// <returns>The cost of traveling from the specified <paramref name="value"/> to its specified <paramref name="neighbor"/>.</returns>
        protected virtual int Cost(T value, T neighbor)
        {
            return 1;
        }

        /// <summary>
        /// Gets the neighbors of a node.
        /// </summary>
        /// <param name="value">The source node.</param>
        /// <returns>The neighbors of the specified <paramref name="value"/>.</returns>
        protected abstract IEnumerable<T> GetNeighbors(T value);

        /// <inheritdoc/>
        public Stack<T> Search(T source, T destination)
        {
            // function A_Star(start, goal, h)

            // openSet := {start}
            // ...
            // fScore := map with default value of Infinity
            // ...

            PriorityQueue<T, int> openSet = new PriorityQueue<T, int>();

            // ...
            // fScore[start] := h(start)
            // ...

            openSet.Enqueue(source, Heuristic(source, destination));

            // cameFrom := an empty map

            Dictionary<T, T> previousNodes = new Dictionary<T, T>();

            // gScore := map with default value of Infinity
            // gScore[start] := 0

            Dictionary<T, int> cumulativeCosts = new Dictionary<T, int>()
            {
                { source, 0 }
            };

            // while openSet is not empty
            //   current := the node in openSet having the lowest fScore[] value
            // ...
            //   openSet.Remove(current)
            // ...

            while (openSet.TryDequeue(out T? current, out _))
            {
                // if current = goal

                if (current.Equals(destination))
                {
                    // return reconstruct_path(cameFrom, current)

                    // function reconstruct_path(cameFrom, current)
                    //   total_path := {current}

                    Stack<T> results = new Stack<T>(previousNodes.Count);

                    results.Push(current);

                    // while current in cameFrom.Keys
                    //   current := cameFrom[current]

                    while (previousNodes.TryGetValue(current, out current))
                    {
                        // total_path.prepend(current)

                        results.Push(current);
                    }

                    return results;
                }
                else
                {
                    // for each neighbor of current

                    foreach (T neighbor in GetNeighbors(current))
                    {
                        // tentative_gScore := gScore[current] + d(current, neighbor)

                        int tenativeCumulativeCost = cumulativeCosts[current] + Cost(current, neighbor);

                        // if tentative_gScore < gScore[neighbor]

                        if (!cumulativeCosts.ContainsKey(neighbor) || tenativeCumulativeCost < cumulativeCosts[neighbor])
                        {
                            // cameFrom[neighbor] := current

                            previousNodes[neighbor] = current;

                            // gScore[neighbor] := tentative_gScore

                            cumulativeCosts[neighbor] = tenativeCumulativeCost;

                            // fScore[neighbor] := tentative_gScore + h(neighbor)
                            // if neighbor not in openSet
                            //   openSet.add(neighbor)

                            openSet.Enqueue(neighbor, tenativeCumulativeCost + Heuristic(neighbor, destination));
                        }
                    }
                }
            }

            // return failure

            return new Stack<T>();
        }
    }
}
