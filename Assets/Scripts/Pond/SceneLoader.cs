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
    [SerializeField]
    Text meetTime;
    [SerializeField]
    Text research;
    [SerializeField]
    Text range;
    FishDataList fishDataList;
    private AsyncOperation async = null;

    [SerializeField]
    Image boy;
    [SerializeField]
    Image girl;

    [SerializeField]
    Sprite boySelected;
    [SerializeField]
    Sprite girlSelected;

    [SerializeField]
    Image wave1;
    [SerializeField]
    Image wave2;

    Sprite boyOrigin;
    Sprite girlOrigin;


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

        boyOrigin = boy.sprite;
        girlOrigin = girl.sprite;
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
        
        fishCount.text = PlayerPrefs.GetInt("FishCount",0).ToString();
        farthest.text = PlayerPrefs.GetFloat("Farthest",0f).ToString("f2")+"M";
        PlayerPrefs.SetInt("TotalFishType", fishDataList.fish.Count);
        bool[] array = PlayerPrefsX.GetBoolArray("FishType",false,fishDataList.fish.Count);
        int count = 0;
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == true){
                count++;
            }
        }
        fishType.text = count.ToString() + "/" + array.Length;

        
    }

    public void OnPondSelected()
    {
        girl.sprite = girlSelected;
    }
    public void OnPondDeselected()
    {
        girl.sprite = girlOrigin;
    }

    public void OnStartSelected()
    {
        boy.sprite = boySelected;
    }
    public void OnStartDeselected()
    {
        boy.sprite = boyOrigin;
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
        int[] arrayInt = PlayerPrefsX.GetIntArray("FishCountArray", 0, fishDataList.fish.Count);
        meetTime.text = arrayInt[id].ToString();
        range.text = fishDataList.fish[id].range;
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

    //show pannel
    void ShowScroll() {
        Tween jump = girl.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(136, 0), 50, 1, 0.5f);
        jump.onComplete = delegate {
            boy.GetComponent<RectTransform>().DOAnchorPosX(-650, 1f);
            girl.GetComponent<RectTransform>().DOAnchorPosX(650, 1f);
        };
        jump.SetEase(Ease.Flash);

        b_pond.transform.GetComponent<Image>().DOFade(0.0f, 1.5f);
        b_pond.transform.DOScale(new Vector3(2.0f, 2.0f, 1.0f), 1.5f);
        Tweener moveBanner = banner.rectTransform.DOAnchorPosY(900, 1.5f);
        Tweener moveButton = b_start.transform.GetComponent<RectTransform>().DOAnchorPosY(-600, 1.5f);
        wave1.rectTransform.DOAnchorPosY(-200, 1.5f);
        wave2.rectTransform.DOAnchorPosY(-200, 1.5f);
        moveButton.onComplete = delegate {
            scroll.DOAnchorPosY(0, 1f);
            b_back.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 1f).onComplete = delegate {
                b_back.interactable = true;
            };
        };
    }


    // hide pannel
    void HideScroll()
    {   
        b_back.interactable = false;
        scroll.DOAnchorPosY(-675f, 1f).onComplete = delegate {
            wave1.rectTransform.DOAnchorPosY(0, 1.5f);
            wave2.rectTransform.DOAnchorPosY(0, 1.5f);
            boy.rectTransform.DOAnchorPosX(-124, 0.5f);
            girl.rectTransform.DOAnchorPosX(136, 0.5f);
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
        Tween jump = boy.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(-124,0), 50, 1, 0.5f);
        jump.onComplete = delegate {
            boy.GetComponent<RectTransform>().DOAnchorPosX(-650, 1f);
            girl.GetComponent<RectTransform>().DOAnchorPosX(650, 1f);
        };
        jump.SetEase(Ease.Flash);
        b_start.transform.GetComponent<Image>().DOFade(0.0f, 1.5f);
        b_start.transform.DOScale(new Vector3(2.0f, 2.0f, 1.0f), 1.5f);
        
        Tweener moveBanner = banner.rectTransform.DOAnchorPosY(900, 1.5f);
        Tweener moveButton = b_pond.transform.GetComponent<RectTransform>().DOAnchorPosY(-600, 1.5f);
        bg.DOFade(1.0f, 3.0f);
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(3f);
        async.allowSceneActivation = true;
        yield return null;
    }

    
}
