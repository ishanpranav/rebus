// Ishan Pranav's REBUS: AStarPathfinder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Pathfinders
{
    /// <summary>
    /// An <see cref="IPathfinder{T}"/> used to find a path between two graph nodes.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/A*_search_algorithm">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/A*_search_algorithm">A* search algorithm - Wikipedia</seealso>
    public abstract class AStarPathfinder<T> : IPathfinder<T> where T : notnull, IEquatable<T>
    {
        protected abstract int Heuristic(T source, T destination);

        protected abstract IEnumerable<T> Neighbors(T value);

        protected virtual int Cost(T source, T neighbor)
        {
            return 1;
        }

        public Stack<T> GetSteps(T source, T destination)
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

            Dictionary<T, int> gScores = new Dictionary<T, int>()
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

                    foreach (T neighbor in Neighbors(current))
                    {
                        // tentative_gScore := gScore[current] + d(current, neighbor)

                        int tentativeGScore = gScores[current] + Cost(current, neighbor);

                        // if tentative_gScore < gScore[neighbor]

                        if (!gScores.ContainsKey(neighbor) || tentativeGScore < gScores[neighbor])
                        {
                            // cameFrom[neighbor] := current

                            previousNodes[neighbor] = current;

                            // gScore[neighbor] := tentative_gScore

                            gScores[neighbor] = tentativeGScore;

                            // fScore[neighbor] := tentative_gScore + h(neighbor)
                            // if neighbor not in openSet
                            //   openSet.add(neighbor)

                            openSet.Enqueue(neighbor, tentativeGScore + Heuristic(neighbor, destination));
                        }
                    }
                }
            }

            // return failure

            return new Stack<T>();
        }
    }
}
