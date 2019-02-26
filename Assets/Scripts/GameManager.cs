using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
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
    float _playerSpeed;

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

    private GameObject Web;
    private GameObject Player;

    public static float Speed;

    public delegate void UpdateUI(int score,int skillTimes,float capacity);
    public static event UpdateUI UpdateUIHandler;

    public delegate void MGameOver();
    public static event MGameOver MGameOverHandler;


    private Material webNodeMat;
    private Material webRopeMat;
    void Initialize()
    {

        
        _score = 0;
        _inSkill = false;
        _curCapacity = 0;
        Speed = _playerSpeed;
        Web = GameObject.FindGameObjectWithTag("Web");



        TFObject.AddScoreHandler += AddScore;
        TFObject.GameOverHandler += GameOver;
        Wharf.DockHandler += Dock;
        PlayerMovement.SkillInputHandler += Skill;
        webNodeMat = Resources.Load<Material>("Materials/WebNode");
        webRopeMat = Resources.Load<Material>("Materials/WebRope");

        ChangeWebMatColor(WebColor);

        //TODO:  I've forgot what to do 

        ResetCollisionMatrix();
     
    }


    private void ResetCollisionMatrix()
    {
        //enable
        DisableCollision("Fish", "Web", false);
        DisableCollision("Rock", "Player", false);
        DisableCollision("Rock", "Web", false);
        //disable
        DisableCollision("Fish", "Rock", true);
        DisableCollision("WebNode", "Web", true);
        DisableCollision("WebNode", "WebNode", true);
        DisableCollision("Player", "WebNode", true);
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
        TFObject.AddScoreHandler -= AddScore;
        TFObject.GameOverHandler -= GameOver;
        Wharf.DockHandler -= Dock;
        PlayerMovement.SkillInputHandler -= Skill;
    }


    // Update is called once per frame
    void Update () {

	}

    private void AddScore(int value,float weight)
    {

        _score += value;
        _curCapacity += weight;
        if (_curCapacity > _maxCapacity) {
            webFull = true;
            DisableCollision("Fish", "Web", true);
            ChangeWebMatColor(WebFullColor);
        }
            
        if (value < 0)
            value = 0;
        UpdateUIHandler(_score, _skillTimes, _curCapacity / _maxCapacity);
    }

    private void GameOver() {
        MGameOverHandler();
        //Destroy(Player);
        Debug.Log("GAMEOVER");
    }

    private void Dock() {
        _curCapacity = 0;
        _skillTimes = maxSkillTimes;
        webFull = false;
        DisableCollision("Fish", "Web", false);
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
        DisableCollision("Fish", "Web", true);
        //DisableCollision("Fish", "Player", true);
        DisableCollision("Rock", "Web", true);    
        DisableCollision("Rock", "Player", true);
        ChangeWebMatColor(WebSkillColor);
        //after _skillDuration realTime;
        yield return new WaitForSeconds(_skillDuration);
        _inSkill = false;
        if (webFull) {
            ChangeWebMatColor(WebFullColor);
        }       
        else {
            ChangeWebMatColor(WebColor);
            DisableCollision("Fish", "Web", false);
            //DisableCollision("Fish", "Player", false);
        }
        DisableCollision("Rock", "Web", false);   
        DisableCollision("Rock", "Player", false);
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
    }
}
