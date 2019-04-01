using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

struct SpawnPoint{
    public Vector3 point;
    public bool occupied;
}
public class SpawnManager : MonoBehaviour {

    private bool gameOver;
	private bool restart;
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
    [Range(0,6)]
    float spawnWide;

    [SerializeField]
    [Range(0,10)]
    private int spawnPointSize;

    private SpawnPoint[] spawnPoints;

	float timer=0f;
	int frame=0;
    FishDataList fishDataList;
    int toatlRarity = 0;
    int mapID = 0;

    float spawnFishTimer = 0;
    float spawnFishInterval = 0;

    float spawnWharfTimer = 0;
    float spawnWharfInterval = 0;

    float spawnObstacleTimer = 0;
    float spawnObstacleInterval = 0;

    List<int> spawnPointsId = new List<int>();
    void InitSpawnPoints() {
        float offset = spawnWide * 2 / (spawnPointSize-1);
        spawnPoints = new SpawnPoint[spawnPointSize];
        for (int i = 0; i < spawnPointSize; i++) {
            spawnPoints[i].point = new Vector3(-spawnWide + i * offset, 0, spawnZValue) ;
            spawnPoints[i].occupied = false;
        }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
            InitSpawnPoints();

        Vector3 direction = transform.TransformDirection(Vector3.back) * 70;
        Gizmos.color = Color.red;
        for (int i = 0; i < spawnPoints.Length; i++) {        
            Gizmos.DrawRay(spawnPoints[i].point, direction);
        }
        
    }

    private void Awake()
    {
        InitSpawnPoints();
    }
    void Start ()
	{
		gameOver = false;
		restart = false;
        spawnFishInterval = 0.5f + TFMath.GaussRand() * 2f;
        spawnWharfInterval = Random.Range(5.0f, 6.0f);
        spawnObstacleInterval = Random.Range(12f, 16f);

        string jsonPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Fish.json");
        fishDataList = JsonUtility.FromJson<FishDataList>(jsonPath);
        
        int length = fishDataList.fish.Count;
          
        foreach (var item in fishDataList.fish) {
            toatlRarity += item.rarity[mapID];
        }
     
    }

    int GetRandomFishID() {
        int rand = Random.Range(1, toatlRarity+1);
        for (int i = 0;i< fishDataList.fish.Count;i++)
        {
            rand -= fishDataList.fish[i].rarity[mapID];
            if (rand <= 0)
            {
                return i;
            }
        }
        return 0;
    }

	void Update ()
	{
        if (b_spawmFish)
            SpawnFish();
        if (b_spawnWharf)
            SpawnWharf();
        if (b_spawnObstacle)
            SpawnObstacle();
    }
    
    void SpawnFish ()
	{
        spawnFishTimer += Time.deltaTime;
        if (spawnFishTimer > spawnFishInterval) {
            spawnFishTimer = 0;
            spawnFishInterval = 0.5f + TFMath.GaussRand() * 2f;

            int pt = GetSpawnPoint();
            if (pt>-1)
                SpawnFishByID(GetRandomFishID(),pt);

            if (Random.Range(0, 8) == 0)
                StartCoroutine(Spawnshoal());
        }   
	}

    int GetSpawnPoint() {
        spawnPointsId.Clear();
        for (int i = 0; i < spawnPointSize; i++)
        {
            if (spawnPoints[i].occupied == false)
            {
                spawnPointsId.Add(i);
            }
        }
        if (spawnPointsId.Count != 0)
            return Random.Range(0, spawnPointsId.Count);
        else
            return -1;
    }

    void SpawnFishByID(int fishID,int spawnPointID) {        
        FishData tempData = fishDataList.fish[fishID];
        GameObject obj = (GameObject)Resources.Load("Prefabs/Fish/" + tempData.modelID);
        float LRange = tempData.maxLength - tempData.minLength;
        float WRange = tempData.maxWeight - tempData.minWeight;
        float length = tempData.minLength + LRange * TFMath.GaussRand();
        float weight = tempData.minWeight + WRange * TFMath.GaussRand();
        obj.GetComponent<Fish>().SetFish(tempData.speed, tempData.score, length, weight);
        Vector3 spawnPosition = spawnPoints[spawnPointsId[spawnPointID]].point;
        
        Instantiate(obj, spawnPosition, Quaternion.Euler(-35, 180, 0));
        // Debug.Log("生成了一条" + length.ToString("f2") + "cm," + weight.ToString("f2") + "kg的" + tempData.name);
    }

    IEnumerator Spawnshoal() {       
        for (int i = 0; i < 10; i++) {
            SpawnFishByID(0, GetSpawnPoint());
            yield return new WaitForSeconds(0.2f);
        }
    }

    bool generateWharf = false;
    void SpawnWharf() {
        spawnWharfTimer += Time.deltaTime;
        
        if (spawnWharfTimer > spawnWharfInterval) {
            spawnWharfTimer = 0;
            spawnWharfInterval = Random.Range(10,12f);
            GameObject.Find("Path" + BGScroller.nextID.ToString()).GetComponent<BGScroller>().OpenWharf(true);
        }
    }
    
    void SpawnObstacle()
    {
        spawnObstacleTimer += Time.deltaTime;
        if (spawnObstacleTimer > spawnObstacleInterval)
        {
            spawnObstacleTimer = 0;
            spawnObstacleInterval = Random.Range(4f, 6f);

            
            int pt = GetSpawnPoint();
            if (pt > -1) {
                int id = Random.Range(0, 1);
                GameObject obj = (GameObject)Resources.Load("Prefabs/Rock/rock" + (id + 1).ToString());
                obj.GetComponent<Obstacle>().SetObstacle(0.4f);
                Vector3 spawnPosition = spawnPoints[pt].point;
                Instantiate(obj, spawnPosition, Quaternion.identity);
            }        
        }
    }

}