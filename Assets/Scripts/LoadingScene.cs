using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    Slider uiSlider;
    [SerializeField]
    Button multiplayer;
    [SerializeField]
    Button singlePlayer;
    private AsyncOperation async;
    private uint _nowprocess;
    // Use this for initialization
    void Start()
    {
        _nowprocess = 0;
        multiplayer.gameObject.SetActive(false);
        singlePlayer.gameObject.SetActive(false);
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        async = SceneManager.LoadSceneAsync(SceneData.nextSceneId);
        async.allowSceneActivation = false;
        yield return async;

    }

    bool LoadComplete =false;

    void Update()
    {
        if (async == null)
        {
            return;
        }

        uint toProcess;
        //Debug.Log(async.progress * 100);
        if (async.progress < 0.9f)//坑爹的progress，最多到0.9f
        {
            toProcess = (uint)(async.progress * 100);
        }
        else
        {
            toProcess = 100;
        }

        if (_nowprocess < toProcess)
        {
            _nowprocess++;
        }

        uiSlider.value = _nowprocess / 100f;

        if (!LoadComplete&&_nowprocess == 100)//async.isDone应该是在场景被激活时才为true
        {
            LoadComplete = true;
            uiSlider.gameObject.SetActive(false);
            multiplayer.gameObject.SetActive(true);
            singlePlayer.gameObject.SetActive(true);

           
        }

        if (LoadComplete) {
           
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                SceneData.mode = 2;
                async.allowSceneActivation = true;
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                SceneData.singlePlayerID = 0;
                SceneData.mode = 1;
                async.allowSceneActivation = true;
            }
            else if (Input.GetKeyDown(KeyCode.Joystick2Button0))
            {               
                SceneData.mode = 2;
                async.allowSceneActivation = true;
            }
            else if (Input.GetKeyDown(KeyCode.Joystick2Button1))
            {
                SceneData.singlePlayerID = 1;
                SceneData.mode = 1;
                async.allowSceneActivation = true;
            }

        }
    }
    public void GoToSceneByMode(int mode) {
        SceneData.mode = mode;
        async.allowSceneActivation = true;
    }

}
