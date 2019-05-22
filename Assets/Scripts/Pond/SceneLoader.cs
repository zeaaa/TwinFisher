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
    [SerializeField]
    Button b_back;
    [SerializeField]
    RectTransform scroll;
    public string nextSceneName;
    private AsyncOperation async = null;
    // Start is called before the first frame update
    void Start()
    {
        b_pond.onClick.AddListener(ShowScroll);
        b_back.onClick.AddListener(HideScroll);
    }

    void ShowScroll() {
        b_pond.transform.GetComponent<Image>().DOFade(0.0f, 1.5f);
        b_pond.transform.DOScale(new Vector3(2.0f, 2.0f, 1.0f), 1.5f);
       
        Tweener moveBanner = banner.rectTransform.DOAnchorPosY(600, 1.5f);
        Tweener moveButton = b_start.transform.GetComponent<RectTransform>().DOAnchorPosY(-600, 1.5f);
        moveButton.onComplete = delegate {
            scroll.DOAnchorPosY(0, 1f);
            b_back.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 1f).onComplete = delegate {
                b_back.interactable = true;
            };
        };
    }

    void HideScroll()
    {
        b_back.interactable = false;
        scroll.DOAnchorPosY(-675f, 1f).onComplete = delegate {
            Color c = b_pond.transform.GetComponent<Image>().color;
            b_pond.transform.GetComponent<Image>().color = new Color(c.r,c.g, c.b,1);
            b_pond.transform.localScale = Vector3.one;
            b_pond.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-600);

            Tweener moveBanner = banner.rectTransform.DOAnchorPosY(0, 1.5f);
            b_start.transform.GetComponent<RectTransform>().DOAnchorPosY(0, 1.5f);
            b_pond.transform.GetComponent<RectTransform>().DOAnchorPosY(0, 1.5f);
        };
        b_back.gameObject.GetComponent<RectTransform>().DOAnchorPosY(190f, 1f);
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
