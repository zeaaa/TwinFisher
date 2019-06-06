using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    const float playerPos = 72;
    public int no = 0;
    bool past = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PastOver() {
        no++;
        TextMesh[] tms = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh tm in tms)
        {
            tm.text = "No:" + no;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!past && transform.position.z < playerPos) {
            past = true;
            PastOver();
        }   
    }
   
}
