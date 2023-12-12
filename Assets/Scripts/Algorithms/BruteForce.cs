using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForce : BaseAlgorithm
{ 
    //Optimize by making a startpoint and not including that in permutations
    private float shortestPathDistance;
    private PointBehaviour[] shortestPath;
    protected override void Algorithm()
    {
        StartOfAlgorithm();

        shortestPathDistance = int.MaxValue;
        
        int i;
        int N = pointsToFindPath.Length;
        shortestPath = new PointBehaviour[N];
        int[] p = new int[N + 1];
        for (i = 0; i < p.Length; i++)
        {
            p[i] = i;
        }

        int j;
        i = 1;
        while (i < N)
        {
            p[i]--;
            j = i % 2 == 0 ? p[i] : 0;
            (pointsToFindPath[j], pointsToFindPath[i]) = (pointsToFindPath[i], pointsToFindPath[j]);
            CalculateDistance();
            i = 1;
            while (p[i] == 0)
            {
                p[i] = i;
                i++;
            }
        }
        
        pointsToFindPath = shortestPath;
        
        EndOfAlgorithm();
    }

    //Might move this to BaseAlgorithm if all use it. 
    private void CalculateDistance()
    {
        float distance = 0;
        for (int i = 1; i < pointsToFindPath.Length; i++)
        {
            distance += Vector2.Distance(pointsToFindPath[i].position, pointsToFindPath[i - 1].position);
        }

        distance += Vector2.Distance(pointsToFindPath[0].position, pointsToFindPath[^1].position);

        if (distance < shortestPathDistance)
        {
            shortestPathDistance = distance;
            shortestPath = (PointBehaviour[])pointsToFindPath.Clone();
        }
    }
}
