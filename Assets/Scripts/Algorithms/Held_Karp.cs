using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Held_Karp : BaseAlgorithm
{
    private float[,] graph;
    float[,] memo; 
    int n;
    private int[] resultTour;
    
    protected override void Algorithm()
    {
        StartOfAlgorithm();
        
        n = algorithmPort.unordered.Count;
        resultTour = new int[n];
        
        graph = new float[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                graph[i, j] = Vector2.Distance(algorithmPort.unordered[i].position, algorithmPort.unordered[j].position);
            }
        }

        
        memo = new float[n, 1 << n];

        var result = HeldKarp(0, new HashSet<int>(), new List<int>());
        result.Item2.CopyTo(resultTour);

        for (int i = 0; i < resultTour.Length; i++)
        {
            pointsToFindPath[i] = algorithmPort.unordered[resultTour[i]];
        }

        EndOfAlgorithm();
    }
    
    private Tuple<float, List<int>> HeldKarp(int current, HashSet<int> visited, List<int> path)
    {
        visited.Add(current);

        if (visited.Count == n)
        {
            // All cities visited, add the starting city to complete the tour
            path.Add(0);
            return new Tuple<float, List<int>>(graph[current, 0], path);
        }

        if (memo[current, GetMask(visited)] != 0)
        {
            return new Tuple<float, List<int>>(memo[current, GetMask(visited)], path);
        }

        float minCost = float.MaxValue;
        List<int> minPath = null;

        for (int next = 0; next < n; next++)
        {
            if (!visited.Contains(next))
            {
                List<int> newPath = new List<int>(path);
                newPath.Add(next);

                visited.Add(next);
                var result = HeldKarp(next, visited, newPath);
                visited.Remove(next);

                float cost = graph[current, next] + result.Item1;

                if (cost < minCost)
                {
                    minCost = cost;
                    minPath = result.Item2;
                }
            }
        }

        visited.Remove(current);
        memo[current, GetMask(visited)] = minCost;
        return new Tuple<float, List<int>>(minCost, minPath);
    }

    private int GetMask(HashSet<int> visited)
    {
        int mask = 0;
        foreach (var city in visited)
        {
            mask |= 1 << city;
        }
        return mask;
    }

}
