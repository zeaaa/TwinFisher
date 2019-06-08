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

    public Transform pondcamera;

    public Transform daylight;
    public Transform nightlight;

    // Start is called before the first frame update
    void Start()
    {
        bgmAS = transform.Find("Bgm").GetComponent<AudioSource>();
        //instance = this;
        if (GetTime.IsDay())
        {
            bgmAS.clip = bgm;
            daylight.gameObject.SetActive(true);
            nightlight.gameObject.SetActive(false);
            pondcamera.GetComponent<Camera>().backgroundColor = new Color(0.89f,0.92f,0.1f);
        }
        else {
            daylight.gameObject.SetActive(false);
            nightlight.gameObject.SetActive(true);
            bgmAS.clip = nightBgm;
            pondcamera.GetComponent<Camera>().backgroundColor = new Color(0.17f, 0.16f, 0.46f);
        }
        
        bgmAS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
