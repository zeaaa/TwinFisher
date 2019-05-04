using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{

    [SerializeField]
    Image banner;
    [SerializeField]
    RawImage bg;
    [SerializeField]
    Button b_start;
    [SerializeField]
    Button b_pond;
    public string nextSceneName;
    private AsyncOperation async = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLoad(){
        StartCoroutine("LoadScene");
    }
    IEnumerator LoadScene(){
        b_start.transform.GetComponent<Image>().DOFade(0.0f, 1.5f);
        b_start.transform.DOScale(new Vector3(2.0f, 2.0f, 1.0f), 1.5f);
        
        Tweener moveBanner = banner.rectTransform.DOAnchorPosY(600, 1.5f);
        Tweener moveButton = b_pond.transform.GetComponent<RectTransform>().DOAnchorPosY(-600, 1.5f);
        bg.DOFade(1.0f, 3.0f);
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(3f);
        async.allowSceneActivation = true;
        yield return null;
    }
}
