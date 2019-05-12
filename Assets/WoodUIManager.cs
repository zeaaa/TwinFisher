using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WoodUIManager : MonoBehaviour
{
    public static WoodUIManager instance;

    [SerializeField]
    Transform l;

    [SerializeField]
    Transform c;

    [SerializeField]
    Camera start;

    [SerializeField]
    Transform ui;


    [SerializeField]
    RawImage bg;

    private AsyncOperation async = null;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartShake());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterPond() {
        l.DOMoveY(l.transform.position.y + 20.0f, 1.0f).onComplete = delegate {
            start.DOFieldOfView(28,1.0f);
            start.transform.DORotate(new Vector3(23f,-177.4f,0f),1.0f);
            //c.gameObject.SetActive(true);
            //ui.gameObject.SetActive(true);
        };
        

    }

    void BannerUp() {
        l.DOMoveY(l.transform.position.y + 20.0f, 1.0f);
    }

    IEnumerator StartShake() {
        l.DOMoveY(l.transform.position.y + 2.0f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        l.DOMoveY(l.transform.position.y - 2.0f, 0.1f);
    }

    public void StartLoad()
    {
        StartCoroutine("LoadScene");
    }


    IEnumerator LoadScene()
    {
        BannerUp();
        bg.DOFade(1.0f, 3.0f);
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(3f);
        async.allowSceneActivation = true;
        yield return null;
    }
}
