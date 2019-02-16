using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    float _skillDuration= 3.0f;
    int _skillTimes = 3;

    [SerializeField]
    Color WebColor;
    [SerializeField]
    Color WebFullColor;
    [SerializeField]
    Color WebSkillColor;

    private GameObject Web;
    private GameObject Player;

    public static float Speed;

    public delegate void UpdateUI(int score,int skillTimes,float capacity);
    public static event UpdateUI UpdateUIHandler;

    public delegate void MGameOver();
    public static event MGameOver MGameOverHandler;
    void Initialize()
    { 
        _score = 0;
        _inSkill = false;
        _curCapacity = 0;
        Speed = _playerSpeed;
        Web = GameObject.FindGameObjectWithTag("Web");
        Player = GameObject.FindGameObjectWithTag("PlayerBase");

        TFObject.AddScoreHandler += AddScore;
        TFObject.GameOverHandler += GameOver;
        Wharf.DockHandler += Dock;
        PlayerMovement.SkillInputHandler += Skill;

        //TODO: 
        DisableCollision("Fish", "Rock", true);
        DisableCollision("WebNode", "Web", true);
        DisableCollision("WebNode", "WebNode", true);
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
            Web.GetComponent<MeshRenderer>().material.color = WebFullColor;
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
        Web.GetComponent<MeshRenderer>().material.color = WebColor;
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
        DisableCollision("Fish", "Web", true);
        //DisableCollision("Fish", "Player", true);
        DisableCollision("Rock", "Web", true);    
        DisableCollision("Rock", "Player", true);
        Web.GetComponent<MeshRenderer>().material.color = WebSkillColor;
        //after _skillDuration realTime;
        yield return new WaitForSeconds(_skillDuration);
        _inSkill = false;
        if (webFull) {
            Web.GetComponent<MeshRenderer>().material.color = WebFullColor;
        }       
        else {
            Web.GetComponent<MeshRenderer>().material.color = WebColor;          
            DisableCollision("Fish", "Web", false);
            //DisableCollision("Fish", "Player", false);
        }
        DisableCollision("Rock", "Web", false);   
        DisableCollision("Rock", "Player", false);
    }
}
