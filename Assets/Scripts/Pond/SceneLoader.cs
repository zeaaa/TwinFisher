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
    Button b_list;
    [SerializeField]
    Button b_archievement;

    [SerializeField]
    RectTransform scrollView;
    [SerializeField]
    RectTransform content;
    [SerializeField]
    RectTransform arhievement;
    [SerializeField]
    RectTransform detailView;   

    [SerializeField]
    Text farthest;
    [SerializeField]
    Text fishCount;
    [SerializeField]
    Text fishType;

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

    [SerializeField]
    RectTransform detailContent;
    int curPage;

    [SerializeField]
    Button[] listButtons;
    [SerializeField]
    Button[] detailButtons;

    int defaultFishID;

    private void Awake()
    {
        //PlayerPrefs.SetInt("FishCount", 0);
        //PlayerPrefs.SetFloat("Farthest", 100f);
        gameState = PondGameState.title;
        curPage = 0;
        defaultFishID = 0;
        detailWidth = detailButtons[0].GetComponent<RectTransform>().rect.width;
        listHeight = listButtons[0].GetComponent<RectTransform>().rect.height;
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
        b_list.onClick.AddListener(delegate { UpdateUIGroup(0); });
        b_archievement.onClick.AddListener(delegate { UpdateUIGroup(2); });

        buttonOrigin = b_list.GetComponent<Image>().sprite;
        boyOrigin = boy.sprite;
        girlOrigin = girl.sprite;
        img_sb0 = b_list.transform.Find("Image").GetComponent<Image>();
        img_sb1 = b_archievement.transform.Find("Image").GetComponent<Image>();

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
        farthest.text =( PlayerPrefs.GetFloat("Farthest",0f) * 0.5f).ToString("0");
        PlayerPrefs.SetInt("TotalFishType", fishDataList.fish.Count);
        bool[] array = PlayerPrefsX.GetBoolArray("FishType",false,fishDataList.fish.Count);
        int count = 0;
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == true){
                count++;
            }
        }
        fishType.text = count.ToString();
        FillList();
        FillDetail();
        arhievement.gameObject.SetActive(false);
        detailView.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);
        b_list.GetComponent<Image>().sprite = buttonSelected;
        b_archievement.GetComponent<Image>().sprite = buttonOrigin;
        img_sb0.color = imgSelectedColor;
        img_sb1.color = imgDeSelectedColor;
    }


    //called when switch page
    void UpdateUIGroup(int id)
    {
        curPage = id;
        if (id == 0) {// list page
            arhievement.gameObject.SetActive(false);
            detailView.gameObject.SetActive(false);
            scrollView.gameObject.SetActive(true);
            b_list.GetComponent<Image>().sprite = buttonSelected;
            b_archievement.GetComponent<Image>().sprite = buttonOrigin;
            img_sb0.color = imgSelectedColor;
            img_sb1.color = imgDeSelectedColor;
            MoveListRect();
        }
        else if (id == 1){// detail page
            arhievement.gameObject.SetActive(false);
            detailView.gameObject.SetActive(true);
            scrollView.gameObject.SetActive(false);
            b_list.GetComponent<Image>().sprite = buttonSelected;
            b_archievement.GetComponent<Image>().sprite = buttonOrigin;
            img_sb0.color = imgSelectedColor;
            img_sb1.color = imgDeSelectedColor;
            MoveDetailRect();
        }
        else if (id == 2){// archieve page
            arhievement.gameObject.SetActive(true);
            detailView.gameObject.SetActive(false);
            scrollView.gameObject.SetActive(false);
            b_list.GetComponent<Image>().sprite = buttonOrigin;
            b_archievement.GetComponent<Image>().sprite = buttonSelected;
            img_sb0.color = imgDeSelectedColor;
            img_sb1.color = imgSelectedColor;
        }
    }

    private void OnDestroy()
    {

        spriteAB.Unload(true);
        b_pond.onClick.RemoveAllListeners();
        b_back.onClick.RemoveAllListeners();
        b_archievement.onClick.RemoveAllListeners();
        b_list.onClick.RemoveAllListeners();
        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    buttons[i].onClick.RemoveAllListeners();
        //}
    }


    //in list page , left or right 
    public void OnFishListSelected(GameObject sender) {
        int id = int.Parse(sender.name.Split('(', ')')[1]) - 1;
        defaultFishID = id;
        float offset = id * listHeight;
        if (offset > fishDataList.fish.Count * listHeight - 680f)
            offset = fishDataList.fish.Count * listHeight - 680f;
        content.DOAnchorPosY(offset, 0.5f);
        sender.transform.Find("Selected").gameObject.SetActive(true);
    }



    public void OnFishListDeSelected(GameObject sender)
    {
        sender.transform.Find("Selected").gameObject.SetActive(false);
    }



    public void OnFishDetailSelected(GameObject sender)
    {
        int id = int.Parse(sender.name.Split('(', ')')[1]) - 1;
        float width = sender.GetComponent<RectTransform>().rect.width;
        float offset = id * width;
        defaultFishID = id;
        bool[] array = PlayerPrefsX.GetBoolArray("FishType", false, 17);
        if (array[id])
        {
            OpenFishCamera(id);
            fish[id].GetComponent<PondFishMovement>().OnShow();
        }
        detailContent.DOAnchorPosX(-offset, 0.5f);
       
        sender.GetComponent<Button>().Select();
    }

    float listHeight;

    public void MoveListRect()
    {
        int id = defaultFishID;
        float offset = id * listHeight;
        if (offset > fishDataList.fish.Count * listHeight - 680f)
            offset = fishDataList.fish.Count * listHeight - 680f;
        content.DOAnchorPosY(offset, 0.0f);
        listButtons[id].Select();
        //sender.transform.Find("Selected").gameObject.SetActive(true);
    }

    float detailWidth;

    public void OnFishDetailDeSelected(GameObject sender)
    {
        
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

        //for (int i = 0; i < fishDataList.fish.Count; i++) {
        //    Debug.Log(fishDataList.fish[i].research);
        //}
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
                if (percent > 1)
                    percent = 1.0f;
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
    void FillDetail ()
    {

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
      
#endif
#if UNITY_ANDROID
        spriteAB = AssetBundle.LoadFromFile(Application.dataPath+"!assets/data.ab");  
#endif
        Button[] btns = detailContent.GetComponentsInChildren<Button>();
        bool[] array = PlayerPrefsX.GetBoolArray("FishType", false, fishDataList.fish.Count);
        for (int i = 0; i < btns.Length; i++)
        {
            int[] arrayInt = PlayerPrefsX.GetIntArray("FishCountArray", 0, fishDataList.fish.Count);
            if (array[i])
            {
                btns[i].gameObject.transform.Find("Image").GetComponent<Image>().sprite = spriteAB.LoadAsset<Sprite>((i + 1).ToString() + "d");
                btns[i].gameObject.transform.Find("name").GetComponent<Text>().text = fishDataList.fish[i].name;
                btns[i].gameObject.transform.Find("Info").GetComponent<Text>().text = fishDataList.fish[i].info;
                
                
                float percent;
                if (arrayInt[i] == 0)
                    percent = 0;
                else
                {
                    percent = arrayInt[i] / (float)fishDataList.fish[i].research;
                }
                if (percent > 1)
                    percent = 1.0f;
                btns[i].gameObject.transform.Find("per").GetComponent<Text>().text = (percent * 100f).ToString("0") + "%";
               
            }
            else
            {
                btns[i].gameObject.transform.Find("Image").GetComponent<Image>().sprite = spriteAB.LoadAsset<Sprite>("Null");
                btns[i].gameObject.transform.Find("name").GetComponent<Text>().text = "尚未遇到的鱼";
                btns[i].gameObject.transform.Find("Info").GetComponent<Text>().text = "???";
                float percent = 0;
                btns[i].gameObject.transform.Find("per").GetComponent<Text>().text = percent.ToString("0") + "%";            
            }
            btns[i].gameObject.transform.Find("meet").GetComponent<Text>().text = arrayInt[i].ToString() + "次";
            if (fishDataList.fish[i].range.Split('-')[1].Equals("0"))
                btns[i].gameObject.transform.Find("Range").GetComponent<Text>().text = fishDataList.fish[i].range.Split('-')[0] + "m后出现";
            else
                btns[i].gameObject.transform.Find("Range").GetComponent<Text>().text = fishDataList.fish[i].range.Split('-')[0] + "m - " + fishDataList.fish[i].range.Split('-')[1] + "m";
        }
        //dont unload ab here
    }

    public void OnClickListButton(int id) {
        //Debug.Log("call");
        defaultFishID = id;
        listButtons[id].transform.Find("Selected").gameObject.SetActive(false);
        UpdateUIGroup(1);      
    }

    //0 to 1 enter
    public void MoveDetailRect()
    {
        int id = defaultFishID;
        float offset = id * detailWidth;
        bool[] array = PlayerPrefsX.GetBoolArray("FishType", false, 17);
        if (array[id])
        {
            OpenFishCamera(id);
            fish[id].GetComponent<PondFishMovement>().OnShow();
        }
        detailContent.DOAnchorPosX(-offset, 0.0f);   
        detailButtons[id].Select();
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


    //update here!!
    //!!!!!!!!!!!!
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = false;  //退出全屏         

        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Resolution[] resolutions = Screen.resolutions;
            Screen.SetResolution((int)(Screen.currentResolution.height * 9.0f / 16.0f), Screen.currentResolution.height, true);
            Screen.fullScreen = true;
        }
    
        if (gameState.Equals(PondGameState.title)&& UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject==null) {
            if (Input.GetAxis("Vertical")!=0 || Input.GetAxis("Horizontal")!=0) {
                b_start.Select();
            }
        }
        if (gameState.Equals(PondGameState.pannel)){
            switch (curPage) {
                case 0:// when in list page 
                    {

                        //press b Go back to title 
                        if (Input.GetKeyDown(KeyCode.Joystick1Button1)|| Input.GetKeyDown(KeyCode.Joystick2Button1))
                        {
                            HideScroll();
                            b_pond.Select();
                            break;
                        }

                        // key RB
                        if (Input.GetKeyDown(KeyCode.Joystick1Button5)|| Input.GetKeyDown(KeyCode.Joystick2Button5)) {                    
                            UpdateUIGroup(2);
                        }

                        //l-pad when selected nothing
                        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                        {
                            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                            {
                                listButtons[defaultFishID].Select();
                            }
                        }
                        break; }
                case 1: 
                    {
               
                        //press b Go back to list 
                        if (Input.GetKeyDown(KeyCode.Joystick1Button1)|| Input.GetKeyDown(KeyCode.Joystick2Button1))
                        {
                            UpdateUIGroup(0);
                            
                            break;
                        }

                        //RT
                        if (Input.GetKeyDown(KeyCode.Joystick1Button5)|| Input.GetKeyDown(KeyCode.Joystick2Button5))
                        {
                            UpdateUIGroup(2);
                            break;
                        }

                        //if (Input.GetAxis("Horizontal") > 0)
                        //{
                        //    int id = int.Parse(defaultFish.name.Split('(', ')')[1]) - 1;
                        //    if(id < fishDataList.fish.Count)
                        //        ShowDetail(id + 1);
                        //    break;
                        //}

                        //if (Input.GetAxis("Horizontal") < 0)
                        //{
                        //    int id = int.Parse(defaultFish.name.Split('(', ')')[1]) - 1;
                        //    if (id > 0)
                        //        ShowDetail(id - 1);
                        //    break;
                        //}

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
                        //press b Go back to title 
                        if (Input.GetKeyDown(KeyCode.Joystick1Button1)|| Input.GetKeyDown(KeyCode.Joystick2Button1))
                        {
                            HideScroll();
                            b_pond.Select();
                            break;
                        }


                        //lt
                        if (Input.GetKeyDown(KeyCode.Joystick1Button4)|| Input.GetKeyDown(KeyCode.Joystick2Button4))
                        {
                            UpdateUIGroup(0);
                            
                            break;
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
        moveButton.onComplete =  delegate { SceneData.nextSceneId = 2; SceneManager.LoadScene(1); };
        //bg.DOFade(1.0f, 3.0f);
        yield return null;
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
}
