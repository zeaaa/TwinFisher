using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] bgm;

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
        status = bgm.Length - 1;
        bgmAS.loop = false;
        bgmAS.clip = bgm[status];
        bgmAS.Play();    
    }

    private void Start()
    {
        bgmAS.clip = bgm[status];
        bgmAS.Play();
        
    }

    int status = 0;
    float playTimer = 0;
    // Update is called once per frame
    void Update()
    {
        playTimer += Time.deltaTime;
        if(status<1)
        if (playTimer >= bgmAS.clip.length)
        {
            status++;
            playTimer = 0;
            bgmAS.clip = bgm[status];
            bgmAS.Play();
        }
    }
}
