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

    [SerializeField]
    Button b_sb0;
    [SerializeField]
    Button b_sb1;
    [SerializeField]
    Button b_sb2;
    [SerializeField]
    RectTransform scrollView;
    [SerializeField]
    RectTransform arhievement;
    [SerializeField]
    RectTransform detailView;   
    [SerializeField]
    Text name;
    [SerializeField]
    Text info;
    [SerializeField]
    Text farthest;
    [SerializeField]
    Text fishCount;
    [SerializeField]
    Text fishType;
    FishDataList fishDataList;
    private AsyncOperation async = null;


    //Button[] buttons;
    private void Awake()
    {
        //PlayerPrefs.SetInt("FishCount", 0);
        //PlayerPrefs.SetFloat("Farthest", 100f);
    }
    // Start is called before the first frame update
    void Start()
    {
        b_pond.onClick.AddListener(ShowScroll);
        b_back.onClick.AddListener(HideScroll);
        b_sb0.onClick.AddListener(ShowArchievement);
        b_sb1.onClick.AddListener(ShowList);
        b_sb2.onClick.AddListener(delegate { ShowDetail(0);});


        //strangely it doesnt work
        //buttons = scrollView.GetComponentsInChildren<Button>();
        //for (int i = 0; i < buttons.Length; i++) {
        //    Debug.Log(i);
        //    Debug.Log(buttons[i].name);
        //    buttons[i].onClick.AddListener(delegate { ShowDetail(i); });
        //}

        AssetBundle ab;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/data.ab");
#endif
#if UNITY_ANDROID
        ab = AssetBundle.LoadFromFile(Application.dataPath+"!assets/data.ab");  
#endif
        TextAsset ta = ab.LoadAsset<TextAsset>("Fish");
        fishDataList = JsonUtility.FromJson<FishDataList>(ta.text);
        ab.Unload(true);
        fishType.text = "0/0";
        fishCount.text = PlayerPrefs.GetInt("FishCount").ToString();
        farthest.text = PlayerPrefs.GetFloat("Farthest").ToString("f2")+"M";
    }

    private void OnDestroy()
    {
        b_pond.onClick.RemoveAllListeners();
        b_back.onClick.RemoveAllListeners();
        b_sb1.onClick.RemoveAllListeners();
        b_sb2.onClick.RemoveAllListeners();
        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    buttons[i].onClick.RemoveAllListeners();
        //}
    }


    public void ShowDetail(int id) {
        arhievement.gameObject.SetActive(false);
        detailView.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        name.text = fishDataList.fish[id].name;
        info.text = fishDataList.fish[id].info;
    }

    void ShowList()
    {
        arhievement.gameObject.SetActive(false);
        detailView.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);
    }

    void ShowArchievement()
    {
        arhievement.gameObject.SetActive(true);
        detailView.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(false);
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
