using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PondUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    PondDataList pondDataList;

    public PondUI[] pondUIList ;
    void Start()
    {
        string jsonPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Pond.json");
        pondDataList=JsonUtility.FromJson<PondDataList>(jsonPath);
        SetPondUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetPondUI(){
        for (int id = 0;id< pondDataList.pondfish.Count;id++)
        {
            PondData tempData = pondDataList.pondfish[id]; 
            
            if(tempData.maxLength<0){
                pondUIList[id].SetBlock(true);
                pondUIList[id].SetButton(false);
                continue;
            }                                                         //未捕获过该鱼
            pondUIList[id].SetName(tempData.name);
            pondUIList[id].SetLength(tempData.maxLength);
            pondUIList[id].SetWeight(tempData.maxWeight);
            pondUIList[id].SetScore(tempData.score);
            
        }
    }
}
