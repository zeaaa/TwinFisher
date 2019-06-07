using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public enum PondGameState {
    title,
    pannel,
    animating
}

public class SceneLoader : MonoBehaviour
{
    public static PondGameState gameState;

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
    RectTransform content;
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

    Sprite buttonOrigin;
    [SerializeField]
    Sprite buttonSelected;

    [SerializeField]
    Button defaultFish;

    Image img_sb0;
    Image img_sb1;
    Image img_sb2;

    [SerializeField]
    Color imgSelectedColor;
    [SerializeField]
    Color imgDeSelectedColor;

    [SerializeField]
    Transform[] cameraGroup;

    [SerializeField]
    Transform[] fish;

    int curPage;

    //Button[] buttons;
    private void Awake()
    {
        //PlayerPrefs.SetInt("FishCount", 0);
        //PlayerPrefs.SetFloat("Farthest", 100f);
        gameState = PondGameState.title;
        curPage = 0;
    }

    void OpenMainCamera() {
        cameraGroup[0].gameObject.SetActive(true);
        for (int k = 1; k< cameraGroup.Length; k++) {
            cameraGroup[k].gameObject.SetActive(false);
        }
    }

    void OpenPondCamera()
    {     
        for (int k = 0; k < cameraGroup.Length-1; k++)
        {
            cameraGroup[k].gameObject.SetActive(false);
        }
        cameraGroup[cameraGroup.Length - 1].gameObject.SetActive(true);
    }

    void OpenFishCamera(int id)
    {     
        for (int k = 0; k < cameraGroup.Length; k++)
        {
            if (k == (id + 1)) 
                cameraGroup[k].gameObject.SetActive(true);
            else
                cameraGroup[k].gameObject.SetActive(false);
        }
   
    }
    // Start is called before the first frame update
    void Start()
    {
        //Application.OpenURL(@"C:\Windows\System32\osk.exe");
        b_pond.onClick.AddListener(ShowScroll);
        b_back.onClick.AddListener(HideScroll);

        b_sb0.onClick.AddListener(ShowList);
        b_sb1.onClick.AddListener(delegate { ShowDetail(0); });
        b_sb2.onClick.AddListener(ShowArchievement);

        buttonOrigin = b_sb0.GetComponent<Image>().sprite;
        boyOrigin = boy.sprite;
        girlOrigin = girl.sprite;
        img_sb0 = b_sb0.transform.Find("Image").GetComponent<Image>();
        img_sb1 = b_sb1.transform.Find("Image").GetComponent<Image>();
        img_sb2 = b_sb2.transform.Find("Image").GetComponent<Image>();

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
        farthest.text =( PlayerPrefs.GetFloat("Farthest",0f) * 0.5f).ToString("f2")+"M";
        PlayerPrefs.SetInt("TotalFishType", fishDataList.fish.Count);
        bool[] array = PlayerPrefsX.GetBoolArray("FishType",false,fishDataList.fish.Count);
        int count = 0;
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == true){
                count++;
            }
        }
        fishType.text = count.ToString() + "/" + array.Length;
        FillList();
        ShowListUI();
    }



    void ShowListUI()
    {
        arhievement.gameObject.SetActive(false);
        detailView.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);
        b_sb0.GetComponent<Image>().sprite = buttonSelected;
        b_sb1.GetComponent<Image>().sprite = buttonOrigin;
        b_sb2.GetComponent<Image>().sprite = buttonOrigin;
        img_sb0.color = imgSelectedColor;
        img_sb1.color = imgDeSelectedColor;
        img_sb2.color = imgDeSelectedColor;
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

        spriteAB.Unload(true);
        b_pond.onClick.RemoveAllListeners();
        b_back.onClick.RemoveAllListeners();
        b_sb1.onClick.RemoveAllListeners();
        b_sb2.onClick.RemoveAllListeners();
        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    buttons[i].onClick.RemoveAllListeners();
        //}
    }

    public void OnFishListSelected(GameObject sender) {
        int id = int.Parse(sender.name.Split('(', ')')[1]) - 1;
        float offset = id * 204f;
        if (offset > fishDataList.fish.Count * 204f - 680f)
            offset = fishDataList.fish.Count * 204f - 680f;

        content.DOAnchorPosY(offset, 0.5f);
        sender.transform.Find("Selected").gameObject.SetActive(true);
    }

    public void OnFishListDeSelected(GameObject sender)
    {
        sender.transform.Find("Selected").gameObject.SetActive(false);
    }


    public void OnClickShowDetail(GameObject sender) {
        int id = int.Parse(sender.name.Split('(', ')')[1]) - 1;
        ShowDetail(id);
    }
    //when switch scene , unload automaticly
    AssetBundle spriteAB;
    void FillList() {

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        spriteAB = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/ponduifish.ab");
#endif
#if UNITY_ANDROID
        spriteAB = AssetBundle.LoadFromFile(Application.dataPath+"!assets/data.ab");  
#endif
        Button[] btns = scrollView.GetComponentsInChildren<Button>();
        bool[] array = PlayerPrefsX.GetBoolArray("FishType", false, fishDataList.fish.Count);

        for (int i = 0; i < fishDataList.fish.Count; i++) {
            Debug.Log(fishDataList.fish[i].research);
        }
        for (int i = 0; i < btns.Length; i++) {
            if (array[i]) {
                btns[i].gameObject.transform.Find("Image").GetComponent<Image>().sprite = spriteAB.LoadAsset<Sprite>((i+1).ToString());
                btns[i].gameObject.transform.Find("name").GetComponent<Text>().text = fishDataList.fish[i].name;
                int[] arrayInt = PlayerPrefsX.GetIntArray("FishCountArray", 0, fishDataList.fish.Count);
                float percent;
                if (arrayInt[i] == 0)
                    percent = 0;
                else
                {
                    
                    percent = arrayInt[i] / (float)fishDataList.fish[i].research;
                }                
                btns[i].gameObject.transform.Find("per").GetComponent<Text>().text = (percent * 100f).ToString("0") + "%";
                btns[i].gameObject.transform.Find("Slider").GetComponent<Slider>().value = percent;
            }
            else {
                btns[i].gameObject.transform.Find("Image").GetComponent<Image>().sprite = spriteAB.LoadAsset<Sprite>("Null");
                btns[i].gameObject.transform.Find("name").GetComponent<Text>().text = "尚未遇到的鱼";
                float percent = 0;
                btns[i].gameObject.transform.Find("per").GetComponent<Text>().text = percent.ToString("0") + "%";
                btns[i].gameObject.transform.Find("Slider").GetComponent<Slider>().value = percent;
            }
            if (i < 9)
                btns[i].gameObject.transform.Find("no").GetComponent<Text>().text = "0" + (i+1).ToString() + ".";
            else
                btns[i].gameObject.transform.Find("no").GetComponent<Text>().text = (i + 1).ToString() + ".";
        }    
        //dont unload ab here
    }

    void ShowList()
    {
        curPage = 0;
        ShowListUI();
    }

    public void ShowDetail(int id)
    {
        curPage = 1;
        fish[id].GetComponent<Animator>().Play("jump");
        OpenFishCamera(id);
        arhievement.gameObject.SetActive(false);
        detailView.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        b_sb0.GetComponent<Image>().sprite = buttonOrigin;
        b_sb1.GetComponent<Image>().sprite = buttonSelected;
        b_sb2.GetComponent<Image>().sprite = buttonOrigin;
        img_sb0.color = imgDeSelectedColor;
        img_sb1.color = imgSelectedColor;
        img_sb2.color = imgDeSelectedColor;
        name.text = fishDataList.fish[id].name;
        info.text = fishDataList.fish[id].info;
        int[] arrayInt = PlayerPrefsX.GetIntArray("FishCountArray", 0, fishDataList.fish.Count);
        meetTime.text = arrayInt[id].ToString();
        range.text = fishDataList.fish[id].range;
        
    }

    void ShowArchievement()
    {
        curPage = 2;
        arhievement.gameObject.SetActive(true);
        detailView.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(false);
        b_sb0.GetComponent<Image>().sprite = buttonOrigin;
        b_sb1.GetComponent<Image>().sprite = buttonOrigin;
        b_sb2.GetComponent<Image>().sprite = buttonSelected;

        img_sb0.color = imgDeSelectedColor;
        img_sb1.color = imgDeSelectedColor;
        img_sb2.color = imgSelectedColor;
    }

    //show pannel
    void ShowScroll() {
        OpenPondCamera();
        gameState = PondGameState.animating;
        //ShowList();
        Tween jump = girl.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(136, 0), 50, 1, 0.5f);
        jump.onComplete = delegate {
            boy.GetComponent<RectTransform>().DOAnchorPosX(-650, 1f);
            girl.GetComponent<RectTransform>().DOAnchorPosX(650, 1f);
        };
        jump.SetEase(Ease.Flash);

        b_pond.transform.GetComponent<Image>().DOFade(0.0f, 1.5f);
        b_pond.transform.DOScale(new Vector3(2.0f, 2.0f, 1.0f), 1.5f);
        b_pond.interactable = false;
        b_start.interactable = false;
        Tweener moveBanner = banner.rectTransform.DOAnchorPosY(900, 1.5f);
        Tweener moveButton = b_start.transform.GetComponent<RectTransform>().DOAnchorPosY(-600f, 1.5f);
        wave1.rectTransform.DOAnchorPosY(-200, 1.5f);
        wave2.rectTransform.DOAnchorPosY(-200, 1.5f);
        moveButton.onComplete = delegate {
            scroll.DOAnchorPosY(0, 1f);
            b_back.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 1f).onComplete = delegate {
                b_back.interactable = true;
                gameState = PondGameState.pannel;
            };
        };
    }


    // hide pannel
    void HideScroll()
    {
        OpenMainCamera();
        gameState = PondGameState.animating;
        b_back.interactable = false;
        scroll.DOAnchorPosY(-1000f, 1f).onComplete = delegate {
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
            b_pond.transform.GetComponent<RectTransform>().DOAnchorPosY(0, 1.5f).onComplete = delegate {
                b_pond.interactable = true;
                b_start.interactable = true;
                gameState = PondGameState.title;
            } ;
        };
        b_back.gameObject.GetComponent<RectTransform>().DOAnchorPosY(190f, 1f);
    }


    float lastFrameLRT = 0;
    // Update is called once per frame
    void Update()
    {
        bool LTPressed= false;
        bool RTPressed = false;
        float LRT = Input.GetAxis("JoyStick1LRT");
        if (lastFrameLRT == 0 && LRT > 0.05f)
            RTPressed = true;
        if (lastFrameLRT == 0 && LRT < -0.05f)
            LTPressed = true;
           
        lastFrameLRT = LRT;
        if (gameState.Equals(PondGameState.title)&& UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject==null) {
            if (Input.GetAxis("Vertical")!=0 || Input.GetAxis("Horizontal")!=0) {
                b_start.Select();
            }
        }
        Debug.Log(LTPressed);
        if (gameState.Equals(PondGameState.pannel)){
            switch (curPage) {
                case 0:
                    { 
                        if (RTPressed) {
                            Debug.Log("call");
                            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null || UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name.Equals(b_back.name))
                            {
                                int id = int.Parse(defaultFish.name.Split('(', ')')[1]) - 1;
                                ShowDetail(id);
                            } else {
                                int id = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name.Split('(', ')')[1]) - 1;
                                ShowDetail(id);
                                defaultFish = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                            }                        
                        }

                        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                        {
                            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                            {
                                defaultFish.Select();
                            }
                        }
                        break; }
                case 1: 
                    {
                        if (LTPressed)
                        {
                            defaultFish.Select();
                            ShowList();
                            break;
                        }
                        if (RTPressed)
                        {
                            ShowArchievement();
                            break;
                        }
                        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                        {
                            if (Input.GetAxis("Vertical") > 0 )
                            {
                                b_back.Select();
                            }
                        }
                        break;                      
                    }
                case 2:
                    {
                        if (LTPressed)
                        {
                            int id = int.Parse(defaultFish.name.Split('(', ')')[1]) - 1;
                            ShowDetail(id);
                        }                     
                        
                        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                        {
                            if (Input.GetAxis("Vertical") > 0)
                            {
                                b_back.Select();
                            }
                        }
                        break;
                    }
                default:break;
            }
        }
    }


    public void StartLoad(){
        b_start.interactable = false;
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene(){
        Debug.Log("start");
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
        SceneData.nextSceneId = 2;
        bg.DOFade(1.0f, 3.0f).onComplete =   delegate{ SceneManager.LoadScene(1); };
        yield return null;


       
        
    }

    
}
