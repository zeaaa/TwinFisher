using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;


public class FloatArgs : EventArgs {

     public FloatArgs(float v){
        value = v;
      }
    public float value;
}
public class UIManager : MonoBehaviour {
    [SerializeField]
    Text t_score;
    [SerializeField]
    Text t_skill;
    // Use this for initialization
    [SerializeField]
    RectTransform r_gameOver;
    [SerializeField]
    Image bg;
    [SerializeField]
    Button b_reStart;
    [SerializeField]
    Button b_back;
    [SerializeField]
    Slider s_capacity;

    [SerializeField]
    Button b_continue;
    [SerializeField]
    Button b_toTitle;

    [SerializeField]
    Image fog;

    [SerializeField]
    Image pauseImg;
    
    [SerializeField]
    RectTransform firstMeetUI;

    [SerializeField]
    RectTransform pauseUI;

    [SerializeField]
    Text t_meetFishName;

    [SerializeField]
    Text t_meetFishInfo;

    [SerializeField]
    Button b_meetFishOk;

    [SerializeField]
    Image meetFishImg;

    [SerializeField]
    Text t_toatlCount;
    [SerializeField]
    Text t_meetCount;
    [SerializeField]
    Text t_rank;

    [SerializeField]
    Sprite[] meetFishImgList;

    [Rename("遇到新鱼弹出窗口")]
    [SerializeField]
    bool popMeetFishUI = true;
    private void Awake()
    {
        GameManager.UpdateUIHandler += UpdateUI;
        Obstacle.GameOverHandler += ShowGameOverUI;

        if(popMeetFishUI)
            Fish.FirstMeetHandler += ShowFirstMeetUI;
        GameManager.OnCloseMeetUI += CloseMeetUI;
        GameManager.OnEnterPause += OpenPauseUI;
        GameManager.OnLeavePause += ClosePauseUI;
        b_meetFishOk.onClick.AddListener(HideMeetUI); 
        fog.color = new Color(1, 1, 1, 0);
        fog.DOFade(1.0f, 0.0f);
    }
    private void OnDestroy()
    {
        GameManager.UpdateUIHandler -= UpdateUI;
        Obstacle.GameOverHandler -= ShowGameOverUI;
        if (popMeetFishUI)
            Fish.FirstMeetHandler -= ShowFirstMeetUI;
        GameManager.OnCloseMeetUI -= CloseMeetUI;
        GameManager.OnEnterPause -= OpenPauseUI;
        GameManager.OnLeavePause -= ClosePauseUI;
        b_meetFishOk.onClick.RemoveAllListeners();
    }


    private void ShowFirstMeetUI(int id) {
        GameManager.gameState = GameState.Animating;
        Time.timeScale = 0;
        Tweener move = firstMeetUI.DOLocalMove(Vector3.zero, 1.0f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        move.onComplete = delegate { GameManager.gameState = GameState.FirstMeet; };
        t_meetFishName.text = SpawnManager.GetFishList().fish[id].name;
        t_meetFishInfo.text = SpawnManager.GetFishList().fish[id].info;
        meetFishImg.sprite = meetFishImgList[id];
        //b_meetFishOk.Select();
    }

    public void OpenPauseUI(object sender,EventArgs args) {
        GameManager.gameState = GameState.Animating;
        Time.timeScale = 0;
        Tweener move = pauseUI.DOLocalMove(Vector3.zero, 1.0f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        pauseImg.gameObject.SetActive(true);
        move.onComplete = delegate { GameManager.gameState = GameState.Pause; };
    }


    public void ClosePauseUI(object sender, EventArgs args)
    {
        GameManager.gameState = GameState.Animating;
        Tweener move = pauseUI.DOLocalMove(Vector3.up * 1600f, 0.5f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        pauseImg.gameObject.SetActive(false);
        move.onComplete = delegate { GameManager.gameState = GameState.Playing; Time.timeScale = 1; };
    }

    public void CloseMeetUI(object sender,EventArgs args) {
        Tweener move = firstMeetUI.DOLocalMove(Vector3.up * 1600f, 0.5f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        move.onComplete = delegate { Time.timeScale = 1; GameManager.gameState = GameState.Playing; };
    }

    public void HideMeetUI() {
        Tweener move = firstMeetUI.DOLocalMove(Vector3.up*1600f, 0.5f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        move.onComplete = delegate { Time.timeScale = 1; };
    }

    private void Start()
    {
        b_reStart.onClick.AddListener(delegate () { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); Time.timeScale = 1; });
        b_back.onClick.AddListener(delegate () { SceneManager.LoadScene(0); Time.timeScale = 1; });
        b_reStart.interactable = false;
        b_back.interactable = false;
        fog.DOFade(0.0f, 3.0f);      
    }

   

    // Update is called once per frame
    void Update () {
        
    }

    

    public void CallSlider() {
        onCapacityChanged.Invoke(this, new FloatArgs(s_capacity.value));
    }

    public static event EventHandler<FloatArgs> onCapacityChanged;

    public static bool CapacityChanging = false;

    void UpdateUI(int score, int skillTimes, float capacity) {
        t_score.text = "score:" + score.ToString();
        t_skill.text =  skillTimes.ToString();
        CapacityChanging = true;
        Tween t = s_capacity.DOValue(capacity > 1 ? 1 : capacity, 0.5f).SetUpdate(true);
        t.onComplete = delegate { CapacityChanging = false; };
    }

    void ShowGameOverUI(int i) {
        StartCoroutine(IEShowGameOverUI());
    }

    IEnumerator IEShowGameOverUI() {
        Debug.Log("set text");
        GameManager.gameState = GameState.Animating;
        
        t_toatlCount.text = GameManager.totalMeet.ToString();
        t_meetCount.text = GameManager.newMeet.ToString();
        yield return new WaitForSeconds(1f);
        if (PathManager.rank > 0)
            t_rank.text = "   第" + PathManager.rank.ToString() + "名!";
        else
            t_rank.text = "收获";
        //Time.timeScale = 0;
        bg.DOFade(0.5f, 1f).SetUpdate(true);
        Tweener move = r_gameOver.DOLocalMove(Vector3.zero, 1.0f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        move.onComplete = delegate (){
            
            r_gameOver.DOShakeRotation(3.0f, 5.0f).SetUpdate(true);
            b_reStart.interactable = true;
            b_back.interactable = true;
            GameManager.gameState = GameState.GameOver;     
        };
    }

}
