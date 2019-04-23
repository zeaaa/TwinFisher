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


    public delegate void UpdateUI(int score,int skillTimes,float capacity);
    public static event UpdateUI UpdateUIHandler;


    public delegate void MGameOver();
    public static event MGameOver MGameOverHandler;

    //GameObject[] webNode = GameObject.FindGameObjectsWithTag("WebNode");
    


    [Range(20, 2000)]
    [SerializeField]
    float forge;
    private Material webNodeMat;
    private Material webRopeMat;
    void Initialize()
    {
        Screen.SetResolution(1080,1920,false);
        _score = 0;
        _inSkill = false;
        _curCapacity = 0;
        _skillTimes = maxSkillTimes;
 
        Fish.AddScoreHandler += AddScore;
        Obstacle.GameOverHandler += GameOver;
        Wharf.DockHandler += Dock;
        PlayerMovement.SkillInputHandler += Skill;
        webNodeMat = Resources.Load<Material>("Materials/WebNode");
        webRopeMat = Resources.Load<Material>("Materials/WebRope");

        ChangeWebMatColor(WebColor);
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
        DisableCollision("WebNode", "Wharf", true);
        DisableCollision("WebPole", "Player", true);
        DisableCollision("WebPole", "WebNode", true);
        //player collision
        DisableCollision("WebPole", "WebPole", true);
        DisableCollision("Player", "Player", true);

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
        Wharf.DockHandler -= Dock;
        PlayerMovement.SkillInputHandler -= Skill;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetWebNodeForge(forge);
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
            DisableCollision("Fish", "WebNode", true);
            ChangeWebMatColor(WebFullColor);
        }
            
        if (value < 0)
            value = 0;
        UpdateUIHandler(_score, _skillTimes, _curCapacity / _maxCapacity);
    }

    private void GameOver(int i) {

        //GameObject.Find("PlayerL").GetComponent<Animator>().SetInteger("GameOver", i);
        GameObject.Find("PlayerR").GetComponent<Animator>().SetInteger("GameOver", i);

        MGameOverHandler();
        //Destroy(Player);
        Debug.Log("GAMEOVER");
    }


    private void Dock() {
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
        DisableCollision("Rock", "WebNode", true);    
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
            DisableCollision("Fish", "WebNode", false);
            //DisableCollision("Fish", "Player", false);
        }
        DisableCollision("Rock", "WebNode", false);   
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
