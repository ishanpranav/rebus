// Ishan Pranav's REBUS: DijkstraPathfindingAlgorithm.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

//using System;
//using System.Collections.Generic;

//namespace Rebus.Server
//{
//    internal class DijkstraPathfindingAlgorithm
//    {
//        public IEnumerable<Concept> EnumerateSteps(IEnumerable<Concept> graph, Concept source, Concept target)
//        {
//            PriorityQueue<Concept, int> vertices = new PriorityQueue<Concept, int>();
//            Dictionary<Concept, int> distancesByVertex = new Dictionary<Concept, int>();
//            Dictionary<Concept, Concept> previousVerticesByVertex = new Dictionary<Concept, Concept>();

//            distancesByVertex.Add(source, 0);

//            foreach (Concept vertex in graph)
//            {
//                if (!vertex.Equals(source))
//                {
//                    distancesByVertex.Add(vertex, Int32.MaxValue);
//                }

//                vertices.Enqueue(vertex, distancesByVertex[vertex]);
//            }

//            while (vertices.Count > 0)
//            {
//                Concept vertex = vertices.Dequeue();

//                if (vertex.Equals(target))
//                {
//                    break;
//                }
//                else
//                {
//                    int distance = distancesByVertex[vertex];

//                    //foreach (Concept neighbor in vertex.engibors)
//                    //{
//                    //    int alternative = distance + 1;

//                    //    if (alternative < distancesByVertex[neighbor])
//                    //    {
//                    //        distancesByVertex[neighbor] = alternative;
//                    //        previousVerticesByVertex[neighbor] = vertex;

//                    //        vertices.Enqueue(neighbor, alternative);
//                    //    }
//                    //}
//                }
//            }

//            if (previousVerticesByVertex.ContainsKey(target) || source.Equals(target))
//            {
//                Stack<Concept> results = new Stack<Concept>();
//                Concept step = target;

//                while (previousVerticesByVertex.ContainsKey(step))
//                {
//                    results.Push(step);

//                    step = previousVerticesByVertex[step];
//                }

//                while (results.Count > 0)
//                {
//                    yield return results.Pop();
//                }
//            }
//        }
//    }
//}
