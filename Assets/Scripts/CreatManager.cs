using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class CreatManager : MonoBehaviour {

	public Vector3 spawnValues;

    [SerializeField]
    GameObject wharfPrefab;
    private bool gameOver;
	private bool restart;

	float timer=0f;
	int frame=0;
    FishDataList fishDataList;
    int toatlRarity = 0;
    int mapID = 0;
    void Start ()
	{
		gameOver = false;
		restart = false;
        string jsonPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Fish.json");
        fishDataList = JsonUtility.FromJson<FishDataList>(jsonPath);
        
        int length = fishDataList.fish.Count;
          
        foreach (var item in fishDataList.fish) {
            toatlRarity += item.rarity[mapID];
        }

        StartCoroutine(SpawnFish());
        StartCoroutine(SpawnWharf());
        StartCoroutine(SpawnRock());

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
		frame++;
		timer += Time.deltaTime;
		if (timer > 1.5f) {
			//Debug.Log (frame / timer);
			frame = 0;
			timer = 0f;
		}
	}

	IEnumerator SpawnFish ()
	{   
		while (true)
		{
            yield return new WaitForSeconds(0.5f+TFMath.GaussRand()*2f);
            int id = GetRandomFishID();
            FishData tempData = fishDataList.fish[id];       
            GameObject obj = (GameObject)Resources.Load("Prefabs/Fish/" + tempData.modelID);
            float LRange = tempData.maxLength - tempData.minLength;
            float WRange = tempData.maxWeight - tempData.minWeight;
            float length = tempData.minLength + LRange * TFMath.GaussRand();
            float weight = tempData.minWeight + WRange * TFMath.GaussRand();
            obj.GetComponent<TFObject>().SetTFObject(length, weight, tempData.score,tempData.speed);
            Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			Instantiate (obj, spawnPosition, Quaternion.Euler(-35,180,0));
            Debug.Log("生成了一条" + length.ToString("f2") + "cm," + weight.ToString("f2") + "kg的" + tempData.name);        
        }
	}

    IEnumerator SpawnWharf() {
        while (true)
        {
            float t = Random.Range(5.0f, 6.0f);
            Vector3 spawnPosition = new Vector3(((float)Random.Range(0, 2) - 0.5f)*20f - 2f, spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(wharfPrefab, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(t);
        }
    }

    IEnumerator SpawnRock()
    {
        while (true)
        {
            float t = Random.Range(12f, 16f);
            yield return new WaitForSeconds(t);
            int id = Random.Range(0, 1);
            GameObject obj = (GameObject)Resources.Load("Prefabs/Rock/rock" + (id + 1).ToString());
            obj.GetComponent<TFObject>().SetTFObject();
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            Instantiate(obj, spawnPosition, Quaternion.identity);        
        }
    }

}