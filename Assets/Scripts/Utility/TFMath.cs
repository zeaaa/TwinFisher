using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFMath{
    //return a number from 0 to 1 by GaussRand
    public static float GaussRand()
    {
        float u = Random.Range(0, 1f);
        float v = Random.Range(0, 1f);
        return Mathf.Sqrt(-2.0f * Mathf.Log(u)) * Mathf.Sin(2.0f * Mathf.PI * v)/6.0f + 0.5f;
    }
}
