using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] cm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloseAll(){
        for(int i=0;i<cm.Length ;i++)
        {
            cm[i].SetActive(false);
        }
    }
}
