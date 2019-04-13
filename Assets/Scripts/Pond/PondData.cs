using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PondData {
    public string name;
    public string modelID;
    
    public float maxLength;

    public float maxWeight;
    public string place;

    public int score;
    public float speed;
    
}
public class PondDataList{
    public List<PondData> pondfish;
}
