using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PondManager : MonoBehaviour
{
    public GameObject[] fishList;



    // Start is called before the first frame update
    void Start()
    {
       // string jsonPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Fish.json");
       // pondDataList = JsonUtility.FromJson<PondDataList>(jsonPath); 
        SetFish();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetFish(){

        bool[] array = PlayerPrefsX.GetBoolArray("FishType", false, 17);
        for (int id = 0;id< 17;id++)
        {
           // PondData tempData = pondDataList.pondfish[id]; 

            if(!array[id]){
                Destroy(fishList[id]);  
               continue;
            }                                                         //未捕获过该鱼

            GameObject obj = fishList[id];
            //obj.GetComponent<TFObject>().SetTFObject(tempData.maxLength , tempData.maxWeight,tempData.speed);
            //PondFishMovement pfm=obj.AddComponent<PondFishMovement>();
            //pfm.speed=tempData.speed;
           
            //Debug.Log("生成了一条" + tempData.maxLength.ToString("f2") + "cm," + tempData.maxWeight.ToString("f2") + "kg的" + tempData.name);
        }
    }
}
