using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AlgorithmPort", menuName = "Scriptable objects/Algorithm Port")]
public class AlgorithmPortSO : ScriptableObject
{
    public UnityAction SignalAlgorithm = delegate {  };
    public UnityAction SignalIntervalIncrease = delegate {  };
    public float MaxTimePerInterval = 10;
    public string path;
    public int amountOfAlgorithms = 0;

    public List<PointBehaviour> unordered;
    public PointBehaviour[] shortestPath;

    public void RemoveAlgorithm()
    {
        amountOfAlgorithms--;
        if (amountOfAlgorithms == 0)
        {
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void OnEnable()
    {
        amountOfAlgorithms = 0;
    }
}
