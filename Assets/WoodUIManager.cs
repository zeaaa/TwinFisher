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
    Transform infoBoard;

    Transform mainUI;

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

    [SerializeField]
    Transform back;

    private AsyncOperation async = null;

    private void Awake()
    {
        instance = this;
        mainUI = transform.Find("MainUI");
    }

    [SerializeField]
    Transform backl;


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
            start.transform.DORotate(new Vector3(23f,-177.4f,0f),1.0f).onComplete = delegate {
                Destroy(mainUI.gameObject);
                back.gameObject.SetActive(true);
                infoBoard.gameObject.SetActive(true);
                infoBoard.DOMoveY(infoBoard.transform.position.y + 10.0f, 1.0f);
                backl.DOMoveY(backl.transform.position.y - 10.0f, 1.0f).onComplete = delegate {
                    backl.DOMoveY(backl.transform.position.y + 0.4f, 0.2f).onComplete = delegate {
                        backl.DOMoveY(backl.transform.position.y - 0.4f, 0.4f);
                    };
                    

                };
               
            };
           
            //c.gameObject.SetActive(true);
            //ui.gameObject.SetActive(true);
        };
        

    }

    public void GetBack()
    {
        
        infoBoard.DOMoveY(infoBoard.transform.position.y - 10.0f, 1.0f);
        backl.DOMoveY(backl.transform.position.y + 10.0f, 1.0f).onComplete = delegate {   
            back.gameObject.SetActive(false);
            infoBoard.gameObject.SetActive(false);
            start.DOFieldOfView(60, 1.0f);
            start.transform.DORotate(new Vector3(14f, -177.4f, 0f), 1.0f).onComplete = delegate {
                GameObject ui = (GameObject)Resources.Load("Prefabs/MainUI");
                GameObject UI = Instantiate(ui, transform.position+Vector3.up*20,Quaternion.identity);
                UI.transform.parent = transform;
                mainUI = UI.transform;
                l = mainUI.transform.Find("w");
                l.DOMoveY(l.transform.position.y - 20.0f, 1.0f);



            };

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
