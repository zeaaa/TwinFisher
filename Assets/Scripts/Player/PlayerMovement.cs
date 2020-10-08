using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Rename("河道宽度")]
    private float riverWide;
    [SerializeField]
    [Rename("玩家移动速度")]
	private float playerMoveSpeed;
	public float tilt;

	public Rigidbody WebPole_L;
    public Rigidbody WebPole_R;
	public GameObject Web;

    public bool skillInput;
    public static float dis;
    public static float p1x;
    public static float p2x;

    public delegate void SkillInput();
    public static event SkillInput SkillInputHandler;
    Animator anim_L;
    Animator anim_R;
    [SerializeField]
    Transform PlayerModel_L;
    [SerializeField]
    Transform PlayerModel_R;
    [SerializeField]
    Transform PlayerForFish_L;
    [SerializeField]
    Transform PlayerForFish_R;
    public Text debug;

    //if move to boarder then clamp it
    void ClampMove(){
        if (WebPole_L.transform.position.x < -riverWide){
            WebPole_L.transform.position = new Vector3(-riverWide + 0.05f,WebPole_L.transform.position.y,WebPole_L.transform.position.z);
            //WebPole_L.velocity = Vector3.zero;
        }
            
        if (WebPole_R.transform.position.x > riverWide)
            WebPole_R.transform.position = new Vector3(riverWide - 0.05f, WebPole_R.transform.position.y, WebPole_R.transform.position.z);
            //WebPole_R.velocity = Vector3.zero;
    }

    bool killMovement = false;

    public float LeftX() {
        return WebPole_L.position.x;
    }

    public float RightX() {
        return WebPole_R.position.x;
    }

    float lastFrameP1RT = 0;
    float lastFrameP2RT = 0;

    bool P1RTPressed = false;
    bool P2RTPressed = false;

    bool P1LTPressed = false;
    bool P2LTPressed = false;

    IEnumerator CountDown1P(float time)
    {
        yield return new WaitForSeconds(time);
        P1RTPressed = false;
    }

    IEnumerator CountDown1PLT(float time)
    {
        yield return new WaitForSeconds(time);
        P1LTPressed = false;
    }

    IEnumerator CountDown2PLT(float time)
    {
        yield return new WaitForSeconds(time);
        P2LTPressed = false;
    }


    IEnumerator CountDown2P(float time)
    {
        yield return new WaitForSeconds(time);
        P2RTPressed = false;
    }

    Coroutine count1p;
    Coroutine count2p;
    Coroutine count1pLT;
    Coroutine count2pLT;

    const float skillInputLast= 0.6f;

    void FixedUpdate ()
	{
        FixPlayerModelPosition();
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX


        float RTP1 = Input.GetAxis("JoyStick1LRT");
        float RTP2 = Input.GetAxis("JoyStick2LRT");
        if (lastFrameP1RT == 0 && RTP1 > 0.05f)
        {
            
            P1RTPressed = true;
            if (count1p != null)
                StopCoroutine(count1p);
            count1p = StartCoroutine(CountDown1P(skillInputLast));

        }
        if (lastFrameP2RT == 0 && RTP2 > 0.05f) {
            P2RTPressed = true;
            if(count2p!=null)
                StopCoroutine(count2p);
            count2p = StartCoroutine(CountDown2P(skillInputLast));
        }
        if (lastFrameP1RT == 0 && RTP1 < 0.05f)
        {
            P1LTPressed = true;
            if (count1pLT != null)
                StopCoroutine(count1pLT);
            count1pLT = StartCoroutine(CountDown1PLT(skillInputLast));
        }
        if (lastFrameP2RT == 0 && RTP2 < 0.05f)
        {
            P2LTPressed = true;
            if (count2pLT != null)
                StopCoroutine(count2pLT);
            count2pLT = StartCoroutine(CountDown2PLT(skillInputLast));
        }


        lastFrameP1RT = RTP1;
        lastFrameP2RT = RTP2;


        if (SceneData.mode == 2) {
            if (((P1RTPressed && P2RTPressed) && !killMovement || (Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.UpArrow))))
            {

                skillInput = true;
                SkillInputHandler();
            }
            else
                skillInput = false;
        }
        else 
        if (SceneData.mode == 1) {
            if (SceneData.singlePlayerID == 0)
            {
                if (((P1LTPressed && P1RTPressed) || Input.GetKey(KeyCode.Space)) && !killMovement)
                {

                    skillInput = true;
                    SkillInputHandler();
                }
                else
                    skillInput = false;
            }
            else if (SceneData.singlePlayerID == 1) {
                if (((P2LTPressed && P2RTPressed) || Input.GetKey(KeyCode.Space)) && !killMovement)
                {

                    skillInput = true;
                    SkillInputHandler();
                }
                else
                    skillInput = false;
            }  
        }
        
        anim_L.SetBool("Skill", skillInput);
        anim_R.SetBool("Skill", skillInput);     
        string[] joyStickNames = Input.GetJoystickNames();

        for (int x = 0; x < joyStickNames.Length; x++)
        {
            print(x + joyStickNames[x]);
        }
        float moveHorizontalL = 0;
        float moveHorizontalR = 0;

        Debug.Log("joy1 "+ Input.GetAxis("JoyStick1LPad"));
        Debug.Log("joy2 "+ Input.GetAxis("JoyStick2LPad"));
        Debug.Log("hl " + Input.GetAxis("HorizontalL"));
        Debug.Log("hr " + Input.GetAxis("HorizontalR"));
        //multiplayer
        if (SceneData.mode == 2)
        {
            if (joyStickNames.Length == 0)
            {
                moveHorizontalL = killMovement ? 0 : Input.GetAxis("HorizontalL");
                moveHorizontalR = killMovement ? 0 : Input.GetAxis("HorizontalR");
            }
            else if (joyStickNames.Length == 1)
            {
                ///
                moveHorizontalL = killMovement ? 0 : Input.GetAxis("HorizontalL");
                moveHorizontalR = killMovement ? 0 : Input.GetAxis("HorizontalR");
            }
            else if (joyStickNames.Length >= 2)
            {
                moveHorizontalL = killMovement ? 0 : Input.GetAxis("JoyStick1LPad");
                moveHorizontalR = killMovement ? 0 : Input.GetAxis("JoyStick2LPad");
            }
            
        }//single player
        else if (SceneData.mode == 1) {
            if (joyStickNames.Length == 0)
            {
                moveHorizontalL = killMovement ? 0 : Input.GetAxis("HorizontalL");
                moveHorizontalR = killMovement ? 0 : Input.GetAxis("HorizontalR");
            }
            else if (joyStickNames.Length == 1)
            {
                moveHorizontalL = killMovement ? 0 : Input.GetAxis("HorizontalL");
                moveHorizontalR = killMovement ? 0 : Input.GetAxis("HorizontalR");
            }
            else if (joyStickNames.Length >= 2)
            {
                if (SceneData.singlePlayerID == 0) {
                    moveHorizontalL = killMovement ? 0 : Input.GetAxis("JoyStick1LPad");
                    moveHorizontalR = killMovement ? 0 : Input.GetAxis("JoyStick1RPad");
                } else if (SceneData.singlePlayerID == 1) {
                    moveHorizontalL = killMovement ? 0 : Input.GetAxis("JoyStick2LPad");
                    moveHorizontalR = killMovement ? 0 : Input.GetAxis("JoyStick2RPad");
                }
               
            }
        }

        
        anim_L.SetFloat("Input", moveHorizontalL);
        anim_R.SetFloat("Input", moveHorizontalR);
        Vector3 movementL = new Vector3(moveHorizontalL, 0.0f, 0.0f);
        Vector3 movementR = new Vector3(moveHorizontalR, 0.0f, 0.0f);


        WebPole_L.velocity = movementL * playerMoveSpeed;
        WebPole_R.velocity = movementR * playerMoveSpeed;

        ClampMove();
#endif

#if UNITY_ANDROID
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.UpArrow))
        {
            skillInput = true;
            SkillInputHandler();
        }
        else
            skillInput = false;

        
#endif
            //Player_L.position = new Vector3(Mathf.Clamp (Player_L.position.x, boundary.xMin, Player_R.position.x), Player_L.position.y, Player_L.position.z);
            //Player_R.position = new Vector3(Mathf.Clamp (Player_R.position.x, Player_L.position.x, boundary.xMax), Player_R.position.y, Player_R.position.z);

            //Player_R.rotation = Quaternion.Euler (0.0f, Player_R.velocity.x * tilt, 0.0f);
            //Player_L.rotation = Quaternion.Euler (0.0f, Player_L.velocity.x * tilt, 0.0f);

            // Web.transform.rotation = Quaternion.Euler(60.0f, 0.0f, 0.0f);
        Web.transform.position = new Vector3((WebPole_R.position.x + WebPole_L.position.x) / 2, Web.transform.position.y, Web.transform.position.z);
        p1x = WebPole_L.position.x;
        p2x = WebPole_R.position.x;
        dis = p2x - p1x;
        Web.transform.localScale = new Vector3(Mathf.Abs(dis), 9f, 1.5f);
    }

    void KillMovement(int i) {
        killMovement = true;
    }


    private void Awake()
    {
        Obstacle.GameOverHandler += KillMovement;
        anim_L = PlayerModel_L.GetComponent<Animator>();
        anim_R = PlayerModel_R.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        Obstacle.GameOverHandler -= KillMovement;
    }

    void Start ()
	{
//#if UNITY_ANDROID
//        Input.gyro.enabled = true;
//#endif

        //Application.targetFrameRate = 30;
        //playerL = transform.parent.Find("PlayerL");
        //playerR = transform.parent.Find("PlayerR");
    }

    void FixPlayerModelPosition() {
        PlayerModel_L.transform.position = new Vector3(WebPole_L.transform.position.x - 1.0f, PlayerModel_L.transform.position.y, PlayerModel_L.transform.position.z);
        PlayerModel_R.transform.position = new Vector3(WebPole_R.transform.position.x + 1.0f, PlayerModel_R.transform.position.y, PlayerModel_R.transform.position.z);
        PlayerForFish_L.transform.position = new Vector3(WebPole_L.transform.position.x - 1.0f, PlayerModel_L.transform.position.y, PlayerModel_L.transform.position.z);
        PlayerForFish_R.transform.position = new Vector3(WebPole_R.transform.position.x + 1.0f, PlayerModel_R.transform.position.y, PlayerModel_R.transform.position.z);
    }
}
