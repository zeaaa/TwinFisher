using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public static int currentID = 0;
    public static int nextID = 1;
    [ShowOnly]
    private float scrollSpeed;
	public float length;
    public int id;
    Transform wharf;

    int passBy;
    [SerializeField]
    GameObject Line;

    public static int spawnTimes;


    private const int totalCount = 4;

    private void Awake()
    {
        passBy = 0;
        scrollSpeed = 0;
        spawnTimes = 0;
        wharf = transform.Find("Dock");
        //SpawnManager.OnSentSpawnMessage += WaitToGenerate;
    }

    void Start ()
	{
        //ClearRank();
        lines = new List<GameObject>();
        GenerateMark(length*id);

    }

    private void OnDestroy()
    {
       // SpawnManager.OnSentSpawnMessage -= WaitToGenerate;
    }

    static void WaitToGenerate(object sender,EventArgs args) {
        Debug.Log("call");
        spawnTimes++;
    }

    List<GameObject> lines;



    const float startOffest = 72f;
    public void GenerateMark(float start) {
        float[] data = PlayerPrefsX.GetFloatArray("Rank", 0, 10);
        for (int i = 0;i<data.Length;i++) {
            if (data[i] != 0) { 
                float mile = data[i] + startOffest;
                if (mile > start && mile < (start + length)) {
                    GameObject line = Instantiate(Line, this.gameObject.transform);
                    float relativePos;
                    if (data[i] > length) {
                        relativePos = (int)data[i] % (int)length + 4;
                    }
                    else {
                        relativePos = (int)data[i]+ 4;
                    }
                    if (relativePos > length)
                        relativePos -= length;
                    line.transform.localPosition = new Vector3(0.1f, 0, relativePos);
                    line.name = data[i].ToString();
                    TextMesh[] tms = line.GetComponentsInChildren<TextMesh>();
                    foreach (TextMesh tm in tms) {
                        tm.text = "No:" + (i + 1).ToString();
                    }
                   
                    line.GetComponentInChildren<RankLine>().no = i + 1;
                   lines.Add(line);
                }
            }
        }       
    }


	void FixedUpdate ()
	{
       
		transform.position += Vector3.forward * -scrollSpeed;
        if (transform.position.z < 0 - length)
        {
            passBy++;
            for (int i = 0; i < lines.Count; i++)
                Destroy(lines[i]);
            lines.Clear();
            GenerateMark((passBy*totalCount+id)*length);
            transform.position += Vector3.forward * length * totalCount;
            currentID++;
            if (currentID > (totalCount - 1))
                currentID -= totalCount;
            nextID = currentID + (totalCount - 1);
            if (nextID > (totalCount - 1))
                nextID -= totalCount;
            //OpenWharf(false);
            if (spawnTimes > 0)
            {
                spawnTimes--;
                OpenDock(true);
            }
            else {

                OpenDock(false);
            }

        }
	}


    
    public void OpenDock(bool status) {
        wharf.GetComponent<Dock>().Open(status);
    }
    
    public void SetSpeed(float speed)
    {
        scrollSpeed = speed;
    }


}