using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Held_Karp : BaseAlgorithm
{
    private float[,] graph;
    float[,] memo; 
    private int[,] parent;
    int n;
    
    protected override void Algorithm()
    {
        StartOfAlgorithm();
        
        n = algorithmPort.unordered.Count;
        
        graph = new float[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                graph[i, j] = Vector2.Distance(algorithmPort.unordered[i].position, algorithmPort.unordered[j].position);
            }
        }
        
        memo = new float[n, 1 << n];
        parent = new int[n, 1 << n];

        float result = HeldKarp(0, 1);

        List<int> path = new List<int>();
        int mask = 1;
        int current = 0;

        for (int i = 0; i < n - 1; i++)
        {
            int next = parent[current, mask];
            path.Add(next);
            current = next;
            mask |= (1 << next);
        }

        path.Add(0);

        for (int i = 0; i < path.Count; i++)
        {
            pointsToFindPath[i] = algorithmPort.unordered[path[i]];
        }

        EndOfAlgorithm();
    }
    
    private float HeldKarp(int current, int mask)
    {
        if (mask == (1 << n) - 1)
            return graph[current, 0];

        if (memo[current, mask] != 0)
            return memo[current, mask];
        
        float minCost = float.MaxValue;
        int minNext = -1;

        for (int next = 0; next < n; next++)
        {
            if ((mask & (1 << next)) == 0)
            {
                int newMask = mask | (1 << next);
                float cost = graph[current, next] + HeldKarp(next, newMask);

                if (cost < minCost)
                {
                    minCost = cost;
                    minNext = next;
                }
            }
        }

        memo[current, mask] = minCost;
        parent[current, mask] = minNext;

        return minCost;
    }
}
