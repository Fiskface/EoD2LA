using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointBehaviour : MonoBehaviour
{
    [NonSerialized] public Vector2 position = Vector2.zero;

    public void NewPosition()
    {
        var x = 8f;
        var y = 4.5f;
        position = new Vector2(Random.Range(-x, x), Random.Range(-y, y));
        transform.position = position;
    }
}
