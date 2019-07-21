using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondSound : MonoBehaviour
{
    [Rename("颠倒黑白")]
    [SerializeField]
    public bool filp = false;

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
        Cursor.visible = false;
        //Resolution[] resolutions = Screen.resolutions;f

        Screen.SetResolution((int)(Screen.currentResolution.height*9.0f/16.0f),Screen.currentResolution.height,false);
        Screen.fullScreen = false;
        bgmAS = transform.Find("Bgm").GetComponent<AudioSource>();
        //instance = this;
        bool isday = GetTime.IsDay();
        if (filp)
            isday = !isday;

        if (isday)
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
