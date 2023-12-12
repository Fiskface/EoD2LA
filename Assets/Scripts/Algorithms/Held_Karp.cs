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

        float result = HeldKarp(0, 1);

        for (int i = 0; i < resultTour.Length; i++)
        {
            pointsToFindPath[i] = algorithmPort.unordered[resultTour[i]];
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

        for (int next = 0; next < n; next++)
        {
            if ((mask & (1 << next)) == 0)
            {
                int newMask = mask | (1 << next);
                float cost = graph[current, next] + HeldKarp(next, newMask);

                minCost = Mathf.Min(minCost, cost);
            }
        }
        
        memo[current, mask] = minCost;
        return minCost;
    }

}
