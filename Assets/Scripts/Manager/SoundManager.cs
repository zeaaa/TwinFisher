using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip bgm;


    [SerializeField]
    AudioClip nightBgm;
    [SerializeField]
    AudioClip gameoverSound;
    [SerializeField]
    public AudioClip dockSound;


    public Transform[] lights;
    public Transform daylight;
    public Transform nightlight;

    public AudioClip[] bubble;

    public static SoundManager instance;

    AudioSource bgmAS;

    // Start is called before the first frame update
    void Awake()
    {
        bgmAS = transform.Find("Bgm").GetComponent<AudioSource>();
        instance = this;
        Obstacle.GameOverHandler += PlayGameOverSound;
    }

    private void OnDestroy()
    {
       Obstacle.GameOverHandler -= PlayGameOverSound;
    }

    void PlayGameOverSound(int i)
    {    
        bgmAS.loop = false;
        bgmAS.clip = gameoverSound;
        bgmAS.Play();    
    }

    private void Start()
    {
        string path =  "player.log";
        GetTime.LogPlay(path);
        if (GetTime.IsDay())
        {
            bgmAS.clip = bgm;
            daylight.gameObject.SetActive(true);
            nightlight.gameObject.SetActive(false);
            foreach (Transform t in lights) {
                t.gameObject.SetActive(false);
            }
        }
        else
        {
            daylight.gameObject.SetActive(false);
            nightlight.gameObject.SetActive(true);
            bgmAS.clip = nightBgm;
            foreach (Transform t in lights)
            {
                t.gameObject.SetActive(true);
            }
        }
        bgmAS.Play();
    }

    float playTimer = 0;
    // Update is called once per frame
    void Update()
    {
        playTimer += Time.deltaTime;
        //if (playTimer >= bgmAS.clip.length)
        //{    
        //    playTimer = 0;
        //    bgmAS.clip = bgm;
        //    bgmAS.Play();
        //}
    }
}
