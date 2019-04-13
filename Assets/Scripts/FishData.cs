using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FishData {
    public string name;
    public string modelID;
    public float minLength;
    public float maxLength;
    public float minWeight;
    public float maxWeight;
    public string info;
    public int[] rarity;
    public int score;
    public float speed;
}

public class FishDataList{
    public List<FishData> fish;
}
