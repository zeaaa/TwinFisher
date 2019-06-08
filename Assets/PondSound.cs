using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondSound : MonoBehaviour
{
    [SerializeField]
    AudioClip bgm;

    [SerializeField]
    AudioClip nightBgm;
    AudioSource bgmAS;
    // Start is called before the first frame update
    void Start()
    {
        bgmAS = transform.Find("Bgm").GetComponent<AudioSource>();
        //instance = this;
        if (GetTime.IsDay())
            bgmAS.clip = bgm;
        else
            bgmAS.clip = nightBgm;
        bgmAS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
