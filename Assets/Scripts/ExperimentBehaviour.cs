using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

public class ExperimentBehaviour : MonoBehaviour
{
    public AlgorithmPortSO algorithmPort;

    [Header("Simulation")] 
    public GameObject pointGameObject;
    public int minPoints = 4;
    public int maxPoints = 100;
    public int increasePerInterval = 1;
    public int samplesPerInterval = 100;
    public float maxAverageTimePerInterval = 0.5f;
    [Header("Runtime")] public int currentPoints = 0;
    
    private int frameCounter = 0;
    private List<int> antalPunkterList = new List<int>();

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        
        algorithmPort.MaxTimePerInterval = maxAverageTimePerInterval * samplesPerInterval;

        algorithmPort.unordered = new List<PointBehaviour>();
        for (int i = 0; i < minPoints; i++)
        {
            SpawnPoint();
        }

        algorithmPort.shortestPath = new PointBehaviour[algorithmPort.unordered.Count];
        algorithmPort.unordered.CopyTo(algorithmPort.shortestPath);

        currentPoints = algorithmPort.unordered.Count;
        antalPunkterList.Add(currentPoints);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCounter >= samplesPerInterval)
        {
            if (algorithmPort.unordered.Count < maxPoints)
            {
                IncreaseInterval();
                frameCounter = 0;
            }
            else
            {
                Application.Quit();
                //UnityEditor.EditorApplication.isPlaying = false;
            }
            
        }
        
        UpdateValues();
        
        Profiler.BeginSample("Sorting", this);
        algorithmPort.SignalAlgorithm();
        Profiler.EndSample();
        
        DoVisualisation();
            
        frameCounter++;
    }

    private void UpdateValues()
    {
        foreach (var pb in algorithmPort.unordered)
        {
            pb.NewPosition();
        }
    }
    
    private void DoVisualisation()
    {
        Vector3[] positions = new Vector3[algorithmPort.shortestPath.Length];
        for (int i = 0; i < algorithmPort.shortestPath.Length; i++)
        {
            positions[i] = algorithmPort.shortestPath[i].position;
        }
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);
    }
    
    private void SpawnPoint()
    {
        GameObject point = Instantiate(pointGameObject);
        var pb = point.GetComponent<PointBehaviour>();
        pb.NewPosition();
        algorithmPort.unordered.Add(pb);
    }
    
    private void IncreaseInterval()
    {
        algorithmPort.SignalIntervalIncrease();
        
        for (int i = 0; i < increasePerInterval; i++)
        {
            SpawnPoint();
        }

        algorithmPort.shortestPath = new PointBehaviour[algorithmPort.unordered.Count];
        algorithmPort.unordered.CopyTo(algorithmPort.shortestPath);

        currentPoints = algorithmPort.unordered.Count;
        antalPunkterList.Add(currentPoints);
    }
    
    private void WriteToFile()
    {
        string path = algorithmPort.path;
        path = path + "AntalPunkter.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write("Antal Punkter;");
            for (var i = 0; i < antalPunkterList.Count; i++)
            {
                writer.Write(antalPunkterList[i] + ";");
            }
        }
    }

    private void OnDisable()
    {
        WriteToFile();
    }
}

