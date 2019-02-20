using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public float tilt;
	//public Boundary boundary;

	public Rigidbody Player_L;
	public Rigidbody Player_R;
	public GameObject Web;

    public bool skillInput;
    public static float dis;

    public delegate void SkillInput();
    public static event SkillInput SkillInputHandler;

    public GameObject lnode;
    public GameObject rnode;

    Transform playerL;
    Transform playerR;
    void FixedUpdate ()
	{
        FixPlayerModelPosition();

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.UpArrow)) {
            skillInput = true;
            SkillInputHandler();
        }
        else
            skillInput = false;




        float moveHorizontalL = Input.GetAxis ("HorizontalL");
		float moveHorizontalR = Input.GetAxis ("HorizontalR");

		Vector3 movementL = new Vector3 (moveHorizontalL, 0.0f,0.0f);
		Vector3 movementR = new Vector3 (moveHorizontalR, 0.0f,0.0f);
		Player_L.velocity = movementL * speed;
		Player_R.velocity = movementR * speed;

		//Player_L.position = new Vector3(Mathf.Clamp (Player_L.position.x, boundary.xMin, Player_R.position.x), Player_L.position.y, Player_L.position.z);
		//Player_R.position = new Vector3(Mathf.Clamp (Player_R.position.x, Player_L.position.x, boundary.xMax), Player_R.position.y, Player_R.position.z);

		Player_R.rotation = Quaternion.Euler (0.0f, Player_R.velocity.x * tilt, 0.0f);
		Player_L.rotation = Quaternion.Euler (0.0f, Player_L.velocity.x * tilt, 0.0f);

       // Web.transform.rotation = Quaternion.Euler(60.0f, 0.0f, 0.0f);
        Web.transform.position =  new Vector3((Player_R.position.x + Player_L.position.x) / 2,Web.transform.position.y,Web.transform.position.z);
		Web.transform.localScale = new Vector3((Player_R.position - Player_L.position).x-1.5f,9f,1.5f);
        dis = Player_R.position.x - Player_L.position.x;
        if (Input.GetKeyDown(KeyCode.Q))
        {

           
        }

    }

	void Start ()
	{
        //Application.targetFrameRate = 30;
        playerL = transform.Find("PlayerL");
        playerR = transform.Find("PlayerR");
    }

    void FixPlayerModelPosition() {
        playerL.transform.position = new Vector3(Player_L.transform.position.x - 1.0f, playerL.transform.position.y, playerL.transform.position.z);
        playerR.transform.position = new Vector3(Player_R.transform.position.x + 1.0f, playerR.transform.position.y, playerR.transform.position.z);
    }
}
