using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForce : BaseAlgorithm
{
    protected override void Algorithm()
    {
        StartOfAlgorithm();
        
        //IList<IList<PointBehaviour>> perms = Permutations(pointsToFindPath);
        //foreach (IList<PointBehaviour> perm in perms)
        {
            
            //Go through here
        }
        
        EndOfAlgorithm();
    }
    
    private static void Test()
    {
        
        
    }

    private static IList<IList<PointBehaviour>> Permutations(List<PointBehaviour> list)
    {
        List<IList<PointBehaviour>> perms = new List<IList<PointBehaviour>>();

        int factorial = 1;
        for (int i = 2; i <= list.Count; i++)
            factorial *= i;

        for (int v = 0; v < factorial; v++)
        {
            List<PointBehaviour> s = new List<PointBehaviour>(list);                
            int k = v;
            for (int j = 2; j <= list.Count; j++)
            {
                int other = k % j;
                (s[j - 1], s[other]) = (s[other], s[j - 1]);

                k /= j;
            }
            perms.Add(s);
        }

        return perms;
    }
}
