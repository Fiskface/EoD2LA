using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchAndBound : BaseAlgorithm
{
    private int n;
    private int[] finalPath;
    private bool[] visited;
    private float finalRes;
    protected override void Algorithm()
    {
        StartOfAlgorithm();

        n = algorithmPort.unordered.Count;
        finalPath = new int[n];
        visited = new bool[n];
        finalRes = Int32.MaxValue;


        float [,] graph = new float[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                graph[i, j] = Vector2.Distance(algorithmPort.unordered[i].position, algorithmPort.unordered[j].position);
            }
        }

        TSP(graph);
        
        Debug.Log("BnB: " + finalRes);
        for (int i = 0; i < finalPath.Length; i++)
        {
            pointsToFindPath[i] = algorithmPort.unordered[finalPath[i]];
        }
        
        EndOfAlgorithm();
    }

    private void CopyToFinal(int[] currentPath)
    {
        for (int i = 0; i < n; i++)
        {
            finalPath[i] = currentPath[i];
        }
    }

    private float FirstMin(float[,] g, int i)
    {
        float min = Int32.MaxValue;
        for(int j = 0; j < n; j++)
        {
            if (g[i, j] < min && i != j)
            {
                min = g[i, j];
            }
        }

        return min;
    }

    private float SecondMin(float[,] g, int i)
    {
        float first = Int32.MaxValue, second = Int32.MaxValue;
        for (int j = 0; j < n; j++)
        {
            if(i == j) continue;
            if (g[i, j] <= first)
            {
                second = first;
                first = g[i, j];
            }
            else if (g[i, j] <= second && g[i, j] != first) second = g[i, j];
        }

        return second;
    }

    private void TSPRec(float[,] g, float currentBound, float currentWeight, int level, int[] currentPath)
    {
        if (level == n)
        {
            if (g[currentPath[level - 1], currentPath[0]] != 0)
            {
                float currentRes = currentWeight + g[currentPath[level - 1], currentPath[0]];

                if (currentRes < finalRes)
                {
                    CopyToFinal(currentPath);
                    finalRes = currentRes;
                }
            }
            return;
        }

        for (int i = 0; i < n; i++)
        {
            if (g[currentPath[level - 1], i] != 0 && visited[i] == false)
            {
                float temp = currentBound;
                currentWeight += g[currentPath[level - 1], i];

                if (level == 1)
                    currentBound -= ((FirstMin(g, currentPath[level - 1]) + FirstMin(g, i)) / 2);
                else
                    currentBound -= ((SecondMin(g, currentPath[level - 1]) + FirstMin(g, i)) / 2);

                if (currentBound + currentWeight < finalRes)
                {
                    currentPath[level] = i;
                    visited[i] = true;
                    
                    TSPRec(g, currentBound, currentWeight, level + 1, currentPath);
                }

                currentWeight -= g[currentPath[level - 1], i];
                currentBound = temp;
                
                Array.Fill(visited, false);
                for (int j = 0; j < level; j++) visited[currentPath[j]] = true;
            }
        }
    }

    private void TSP(float[,] g)
    {
        int[] currentPath = new int[n + 1];

        float currentBound = 0;
        Array.Fill(currentPath, -1);
        Array.Fill(visited, false);

        for (int i = 0; i < n; i++) currentBound += (FirstMin(g, i) + SecondMin(g, i));

        currentBound /= 2;

        visited[0] = true;
        currentPath[0] = 0;
        
        TSPRec(g, currentBound, 0, 1, currentPath);
    }
}
