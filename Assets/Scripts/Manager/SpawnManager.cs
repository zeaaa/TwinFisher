﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;
using System;

struct SpawnPoint{
    public Vector3 point;
    public bool occupied;
}
public class SpawnManager : MonoBehaviour {

    private const float spawnZValue = 125f;

    [SerializeField]
    [Rename("生成鱼")]
    bool b_spawmFish = true;

    [SerializeField]
    [Rename("生成码头")]
    bool b_spawnWharf = true;

    [SerializeField]
    [Rename("生成障碍")]
    bool b_spawnObstacle = true;

    [SerializeField]
    [Range(0, 6)]
    float spawnWide;

    [SerializeField]
    [Range(0, 10)]
    private int spawnPointSize;

    [SerializeField]
    bool drawLine = true;

    private SpawnPoint[] spawnPoints;
    private SpawnPoint[] spawnObstaclePoints;

    [SerializeField]
    [Range(0, 10)]
    private int spawnObstaclePointSize = 1;

    [SerializeField]
    [Range(0, 10)]
    private float spawnObstacleWide = 6;

    static FishDataList fishDataList;
    int toatlRarity = 0;
    int mapID = 0;

    [Rename("刷新鱼数量（每分钟）")]
    [SerializeField]
    int fishAmount;

    [Rename("起始刷新障碍数量（每分钟）")]
    [SerializeField]
    int obstacleAmountBegin;

    [Rename("最终刷新障碍数量（每分钟）")]
    [SerializeField]
    int obstacleAmountFinal;

    [Rename("刷新码头数量（每分钟）")]
    [SerializeField]
    int dockAmount;

    [Rename("第一次刷新障碍的时间")]
    [SerializeField]
    float firstObstacleTime;

    [Rename("第一次刷新码头的时间")]
    [SerializeField]
    float firstDockTime;

    float spawnFishTimer = 0;
    float spawnFishInterval = 0;

    float spawnWharfTimer = 0;
    float spawnWharfInterval = 0;

    float spawnObstacleTimer = 0;
    float spawnObstacleInterval = 0;

    List<int> spawnPointsIdList = new List<int>();

    public static FishDataList GetFishList() {
        return fishDataList;
    }

    void InitSpawnPoints() {
        float offset = spawnWide * 2 / (spawnPointSize - 1);
        spawnPoints = new SpawnPoint[spawnPointSize];
        spawnObstaclePoints = new SpawnPoint[spawnObstaclePointSize];
        for (int i = 0; i < spawnPointSize; i++) {
            spawnPoints[i].point = new Vector3(-spawnWide + i * offset, 0, spawnZValue);
            spawnPoints[i].occupied = false;
        }

        offset = spawnObstacleWide * 2 / (spawnObstaclePointSize - 1);
        for (int i = 0; i < spawnObstaclePointSize; i++)
        {
            spawnObstaclePoints[i].point = new Vector3(-spawnObstacleWide + i * offset, 0, spawnZValue);
            spawnObstaclePoints[i].occupied = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            InitSpawnPoints();

        Vector3 direction = transform.TransformDirection(Vector3.back) * 70;

        if (drawLine) {

            Gizmos.color = Color.black;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Gizmos.DrawRay(spawnPoints[i].point, direction);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < spawnObstaclePoints.Length; i++)
            {
                Gizmos.DrawRay(spawnObstaclePoints[i].point, direction);
            }
        }


    }

    private void Awake()
    {
        InitSpawnPoints();
        Obstacle.GameOverHandler += StopSpawning;
        //PathManager.OnOverDistance += SpawnShoal;
    }

    void OnDestroy() {
        Obstacle.GameOverHandler -= StopSpawning;
        //PathManager.OnOverDistance -= SpawnShoal;
    }

    void StopSpawning(int i) {
        b_spawnWharf = false;
        b_spawnObstacle = false;
    }

    void Start()
    {
        spawnFishInterval = 0.5f + TFMath.GaussRand() * 5f;
        spawnWharfInterval = firstDockTime;
        spawnObstacleInterval = firstObstacleTime;
        AssetBundle ab;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/data.ab");
#endif
#if UNITY_ANDROID
        ab = AssetBundle.LoadFromFile(Application.dataPath+"!assets/data.ab");  
#endif
        TextAsset ta = ab.LoadAsset<TextAsset>("Fish");
        fishDataList = JsonUtility.FromJson<FishDataList>(ta.text);
        ab.Unload(true);
        int length = fishDataList.fish.Count;

        foreach (var item in fishDataList.fish) {
            toatlRarity += item.rarity[mapID];
        }

    }

    int GetRandomFishID() {
        int rand = UnityEngine.Random.Range(1, toatlRarity + 1);
        for (int i = 0; i < fishDataList.fish.Count; i++)
        {
            rand -= fishDataList.fish[i].rarity[mapID];
            if (rand <= 0)
            {
                float min = float.Parse(fishDataList.fish[i].range.Split('-')[0]);
                float max = float.Parse(fishDataList.fish[i].range.Split('-')[1]);
                if (max == 0) max = 9999999f;
                if (min <= PathManager.GetCurrentMileage() && max >= PathManager.GetCurrentMileage())
                    return i;
                else
                    return GetRandomFishID();
            }
        }
        return 0;
    }

    void Update()
    {
        if (b_spawmFish)
            SpawnFish();
        if (b_spawnWharf)
            SpawnWharf();
        if (b_spawnObstacle)
            SpawnObstacle();
    }

    void SpawnFish()
    {
        spawnFishTimer += Time.deltaTime;
        if (spawnFishTimer > spawnFishInterval) {
            spawnFishTimer = 0;
            spawnFishInterval = TFMath.GaussRand() * 60 / (float)fishAmount;

            int pt = GetSpawnPoint();
            if (pt > -1) {
                //spawnPoints[pt].occupied = true;
                SpawnFishByID(GetRandomFishID(), pt);
            }
            else
                return;

            //if (Random.Range(0, 8) == 0)
            //StartCoroutine(Spawnshoal());
        }
    }

    void SpawnShoal(object sender,EventArgs args) {
        StartCoroutine(Spawnshoal());
    }

    int GetSpawnPoint() {
        spawnPointsIdList.Clear();
        for (int i = 0; i < spawnPointSize; i++)
        {
            if (spawnPoints[i].occupied == false)
            {
                spawnPointsIdList.Add(i);
            }
        }
        if (spawnPointsIdList.Count != 0)
            return spawnPointsIdList[UnityEngine.Random.Range(0, spawnPointsIdList.Count)];
        else
            return -1;
    }

    void SpawnFishByID(int fishID, int spawnPointID) {
        FishData tempData = fishDataList.fish[fishID];
        GameObject obj = (GameObject)Resources.Load("Prefabs/Fish/" + tempData.modelID);
        float LRange = tempData.maxLength - tempData.minLength;
        float WRange = tempData.maxWeight - tempData.minWeight;
        float length = tempData.minLength + LRange * TFMath.GaussRand();
        float weight = tempData.minWeight + WRange * TFMath.GaussRand();
        obj.GetComponent<Fish>().SetFish(tempData.speed, tempData.score, length, weight, spawnPointID, fishID);
        Vector3 spawnPosition = spawnPoints[spawnPointID].point;

        Instantiate(obj, spawnPosition, Quaternion.Euler(-35, 180, 0));
        // Debug.Log("生成了一条" + length.ToString("f2") + "cm," + weight.ToString("f2") + "kg的" + tempData.name);
    }

    IEnumerator Spawnshoal() {
        for (int i = 0; i < 20; i++) {
            SpawnFishByID(0, GetSpawnPoint());
            yield return new WaitForSeconds(0.2f);
        }
    }

    public static event EventHandler OnSentSpawnMessage;

    void SpawnWharf() {
        spawnWharfTimer += Time.deltaTime;      
        if (spawnWharfTimer > spawnWharfInterval) {
            spawnWharfTimer = 0;
            spawnWharfInterval = TFMath.GaussRand() * 60 / (float)dockAmount;
            //Debug.Log(BGScroller.currentID + "xx"+ BGScroller.nextID);
            //OnSentSpawnMessage.Invoke(this, EventArgs.Empty);
            BGScroller.spawnTimes++;
        }
    }


    void SpawnObstacle()
    {
        spawnObstacleTimer += Time.deltaTime;
        if (spawnObstacleTimer > spawnObstacleInterval)
        {
            spawnObstacleTimer = 0;
            float amount = obstacleAmountBegin + (Time.timeSinceLevelLoad / 60.0f)*(obstacleAmountFinal - obstacleAmountBegin);
            if (Time.timeSinceLevelLoad > 60.0f)
                amount = obstacleAmountFinal;
            spawnObstacleInterval = 1.0f + TFMath.GaussRand() * 60 / amount;


            //int pt = GetSpawnPoint();
            int pt = UnityEngine.Random.Range(0, spawnObstaclePoints.Length);
            if (pt > -1) {
                int id = UnityEngine.Random.Range(0, 5);
                GameObject obj = (GameObject)Resources.Load("Prefabs/Rock/rock" + (id + 1).ToString());
                Vector3 spawnPosition = spawnObstaclePoints[pt].point;
                Instantiate(obj, spawnPosition, Quaternion.identity);
            }
        }
    }
}