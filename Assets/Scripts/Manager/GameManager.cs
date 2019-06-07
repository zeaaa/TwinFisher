using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    Playing,
    Pause,
    Animating,
    FirstMeet,
    GameOver
}

public class GameManager : MonoBehaviour {

    //public static GameManager instance;

    [SerializeField]
    int _score;
    [SerializeField]
    float _curCapacity;
    [SerializeField]
    [ShowOnly]
    [Rename("最大容量")]
    float _maxCapacity;

    [SerializeField]
    int maxSkillTimes = 3;

    bool _inSkill;
    float _skillDuration= 1.2f;
    int _skillTimes = 3;

    [SerializeField]
    Color WebColor;
    [SerializeField]
    Color WebFullColor;
    [SerializeField]
    Color WebSkillColor;

    [SerializeField]
    public DOTweenPath lp;
    [SerializeField]
    public DOTweenPath rp;

    private GameObject Player;

    public static event EventHandler OnCloseMeetUI;
    public static event EventHandler OnEnterPause;
    public static event EventHandler OnLeavePause;
    public static event EventHandler OnReturnToMenu;

    public static GameState gameState;

    public delegate void UpdateUI(int score,int skillTimes,float capacity);
    public static event UpdateUI UpdateUIHandler;


    [Range(20, 2000)]
    [SerializeField]
    float forge;
    
    [Rename("玩家间碰撞")]
    [SerializeField]
    private bool playerInnerColli;

    private Material webNodeMat;
    private Material webRopeMat;


    public static int totalMeet;
    public static int newMeet;

    void ClearFishMeet() {
        int type = PlayerPrefs.GetInt("TotalFishType");
        bool[] falseArray = new bool[type];
        for (int i = 0; i < type; i++)
        {
            falseArray[i] = false;
        }
        PlayerPrefsX.SetBoolArray("FishType", falseArray);
    }

    void Initialize()
    {
        gameState = GameState.Playing;
        totalMeet = 0;
        newMeet = 0;

        //ClearFishMeet();

        Screen.SetResolution(1080,1920,false);
        _score = 0;
        _inSkill = false;
        _curCapacity = 0;
        _skillTimes = maxSkillTimes;
 
        Fish.AddScoreHandler += AddScore;
        Obstacle.GameOverHandler += GameOver;
        Dock.DockHitHandler += PlayerDock;
        PlayerMovement.SkillInputHandler += Skill;
        
        webNodeMat = Resources.Load<Material>("Materials/WebNode");
        webRopeMat = Resources.Load<Material>("Materials/WebRope");

        ChangeWebMatColor(WebColor);
        ResetCollisionMatrix();
    }

    private void ResetCollisionMatrix()
    {
        //enable
        DisableCollision("Fish", "WebNode", false);
        DisableCollision("Fish", "Web", false);
        DisableCollision("Obstacle", "Player", false);
        DisableCollision("Obstacle", "Web", false);
        //disable
        DisableCollision("Fish", "Obstacle", true);
        DisableCollision("WebNode", "Web", true);
        DisableCollision("WebNode", "WebNode", true);
        DisableCollision("Player", "WebNode", true);
        DisableCollision("WebNode", "Dock", true);
        DisableCollision("WebPole", "Player", true);
        DisableCollision("WebPole", "WebNode", true);
        //
        DisableCollision("Player", "Fish", true);
        DisableCollision("WebPole", "Fish", true);
        
        DisableCollision("PlayerForFish", "Player",true);
        DisableCollision("PlayerForFish", "WebNode", true);
        DisableCollision("PlayerForFish", "WebPole", true);
        DisableCollision("PlayerForFish", "PlayerForFish", true);
        //player collision
        DisableCollision("WebPole", "WebPole", !playerInnerColli);
        DisableCollision("Player", "Player", !playerInnerColli);
    
    }

    bool webFull = false;
    void Awake()
    {
        Initialize();
    }
    // Use this for initialization
    void Start () {
        UpdateUIHandler(_score, _skillTimes,0);  
    }
    private void OnDestroy()
    {
        Fish.AddScoreHandler -= AddScore;
        Obstacle.GameOverHandler -= GameOver;
        Dock.DockHitHandler -= PlayerDock;
        PlayerMovement.SkillInputHandler -= Skill;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetWebNodeForge(forge);
    }
    
    // Update is called once per frame
    void Update () {
        if (gameState.Equals(GameState.GameOver)) {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
                //reload
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); Time.timeScale = 1;
            }
            else
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                SceneManager.LoadScene(0); Time.timeScale = 1;
            }
        }
        else if (gameState.Equals(GameState.FirstMeet))
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                GameManager.gameState = GameState.Animating;
                OnCloseMeetUI.Invoke(this, EventArgs.Empty);
            }
        } else if (gameState.Equals(GameState.Playing)) {

            if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7))
            {
                OnEnterPause.Invoke(this, EventArgs.Empty);
            }
            
        }
        else if (gameState.Equals(GameState.Pause))
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0)|| Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                OnLeavePause.Invoke(this, EventArgs.Empty);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick2Button1))
            {
                //OnReturnToMenu.Invoke(this, EventArgs.Empty);
                SceneManager.LoadScene(0); Time.timeScale = 1;
            }
        }
    }

    

    private void AddScore(int value,float weight)
    {

        _score += value;
        _curCapacity += weight;
        if (_curCapacity > _maxCapacity) {
            webFull = true;
            DisableCollision("Fish", "WebNode", true);
            ChangeWebMatColor(WebFullColor);
        }
        if (value < 0)
            value = 0;
        UpdateUIHandler(_score, _skillTimes, _curCapacity / _maxCapacity);
    }

    private void GameOver(int i) {
        DisableCollision("Fish", "WebNode", true);
        StartCoroutine(Delay(1.0f));
        //player
        if (i == 0) {    
            DisableCollision("Obstacle", "WebNode", true);
        }//web
        if (i == 1) {
            SetWebNodeForge(100f);
            GameObject.Find("PlayerL").GetComponent<Animator>().SetInteger("GameOver", 1);
            GameObject.Find("PlayerR").GetComponent<Animator>().SetInteger("GameOver", 1);
        }
       
        Debug.Log("GAMEOVER" + i);
    }


    IEnumerator Delay(float time) {
        yield return new WaitForSecondsRealtime(time);
        //GameObject.Find("PlayerL").GetComponent<Animator>().Play("GameOverIdle");
        //GameObject.Find("PlayerR").GetComponent<Animator>().Play("GameOverIdle");
        GameObject.Find("PlayerL").GetComponent<Animator>().SetInteger("GameOver", 2);
        GameObject.Find("PlayerR").GetComponent<Animator>().SetInteger("GameOver", 2);
    }

    private void PlayerDock() {
        _curCapacity = 0;
        _skillTimes = maxSkillTimes;
        webFull = false;
        DisableCollision("Fish", "WebNode", false);
        ChangeWebMatColor(WebColor);
        UpdateUIHandler(_score, _skillTimes,0);
    }

    private void Skill() {
        if (_skillTimes > 0 && !_inSkill)
        {
            StartCoroutine(SkillCoroutine());
            _skillTimes -= 1;
            UpdateUIHandler(_score, _skillTimes,_curCapacity/_maxCapacity);
        }
    }

    void DisableCollision(string layerName1,string layerName2,bool enable) {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(layerName1), LayerMask.NameToLayer(layerName2), enable);
    }

    IEnumerator SkillCoroutine()
    {
        //wait for next fixedUpdate
        yield return new WaitForFixedUpdate();
        _inSkill = true;
        WebAnim();
        DisableCollision("Fish", "WebNode", true);
        //DisableCollision("Fish", "Player", true);
        DisableCollision("Obstacle", "WebNode", true);    
        DisableCollision("Obstacle", "Player", true);
        ChangeWebMatColor(WebSkillColor);
        //after _skillDuration realTime;
        yield return new WaitForSeconds(_skillDuration);
        _inSkill = false;
        if (webFull) {
            ChangeWebMatColor(WebFullColor);
        }       
        else {
            ChangeWebMatColor(WebColor);
            DisableCollision("Fish", "WebNode", false);
            //DisableCollision("Fish", "Player", false);
        }
        DisableCollision("Obstacle", "WebNode", false);   
        DisableCollision("Obstacle", "Player", false);
    }

    void ChangeWebMatColor(Color c) {    
        webNodeMat.color = c;
        webRopeMat.color = c;
    }

    void WebAnim() {   
        //DORestart:  recalculate the relative path
        lp.DORestart(true);
        rp.DORestart(true);
        lp.DOPlay();
        rp.DOPlay();
        SetWebNodeForge(0);
        lp.gameObject.transform.DOLocalRotate(new Vector3(75, 0, -8), 0.4f, RotateMode.Fast).onComplete = delegate{
            lp.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 7), 0.6f, RotateMode.Fast).onComplete = delegate{SetWebNodeForge(forge);};

        };
        rp.gameObject.transform.DOLocalRotate(new Vector3(75, 0, 8), 0.4f, RotateMode.Fast).onComplete = delegate{
            rp.gameObject.transform.DOLocalRotate(new Vector3(0, 0, -7), 0.6f, RotateMode.Fast);
        };

        DOTweenPath t;
        GameObject[] os = GameObject.FindGameObjectsWithTag("WebNode");
        foreach (GameObject o in os) {
            if (o.GetComponent<DOTweenPath>()) {
                t = o.GetComponent<DOTweenPath>();
                t.DORestart(true);
                t.DOPlay();
            }          
        }
    }

    void SetWebNodeForge(float forge) {
        GameObject[] os = GameObject.FindGameObjectsWithTag("WebNode");
        foreach (GameObject o in os)
        {
            if (o.GetComponent<NodePosition>())
            {
                o.GetComponent<NodePosition>().forge = forge;
            }
        }
    }
}
