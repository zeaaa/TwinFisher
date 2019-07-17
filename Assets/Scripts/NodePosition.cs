
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePosition : MonoBehaviour {

	//绳子核心算法
	//判断两个节点的距离，对节点加弹力
	float nodeDistance =0.1f;  //节点定长
	public float forge=500f;

    //left right up down
    public Transform[] linkNodeTran;

    bool isInnerWeb = false;

    BoxCollider col;

    Rigidbody rig;

    float linewide=0.4f;
    public GameObject pf;

    public bool IsInner() {
        return isInnerWeb;
    }

    GameObject[] line;

    public bool left;
    public bool right;
    public bool up;
    public bool down;

    private void Start()
	{
        
        rig = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        //生成连接线
        line = new GameObject[4];
        if (linkNodeTran[0] && left)
        {
            line[0] = Instantiate(pf, transform.position, Quaternion.identity);
           // line[0].GetComponentInChildren<WebColor>().flip = true;
          //  line[0].GetComponentInChildren<WebColor>().vertical = true;
          //  line[0].GetComponentInChildren<WebColor>().id = int.Parse(gameObject.name[7].ToString());
            line[0].transform.parent = gameObject.transform;
        }
        if (linkNodeTran[1] && right)
        {
            line[1] = Instantiate(pf, transform.position, Quaternion.identity);
            line[1].transform.parent = gameObject.transform;
           // line[1].GetComponentInChildren<WebColor>().id = int.Parse(gameObject.name[7].ToString())+1;
            //line[1].GetComponentInChildren<WebColor>().vertical = true;
        }
        if (linkNodeTran[2] && up)
        {
            line[2] = Instantiate(pf, transform.position, Quaternion.identity);
            line[2].transform.parent = gameObject.transform;
            ///line[2].GetComponentInChildren<WebColor>().id = int.Parse(gameObject.name[7].ToString())+1;
            //line[2].GetComponentInChildren<WebColor>().vertical = false;
        }
        if (linkNodeTran[3] && down)
        {
            line[3] = Instantiate(pf, transform.position, Quaternion.identity);
            line[3].transform.parent = gameObject.transform;
            //line[3].GetComponentInChildren<WebColor>().id = int.Parse(gameObject.name[7].ToString())+1;
           // line[3].GetComponentInChildren<WebColor>().vertical = false;
        }
    }

	private void FixedUpdate()
	{
        JudgeInner();
        Judge();
		WebJump ();
		Deceleration();
        AddForce();
	}


    void AddForce() {
        rig.AddForce(Vector3.back*forge*0.02f * Mathf.Abs(PlayerMovement.dis));
    }

    void JudgeInner() {
        if (PlayerMovement.p1x < transform.position.x && PlayerMovement.p2x > transform.position.x)
        {
            isInnerWeb = true;
        }
        else
            isInnerWeb = false;
      
        col.enabled = isInnerWeb;
    }

	void Judge()
	{
         for (int i=0;i< 2; i++)
        {
          /*  if (gameObject.name[7] == '0' || gameObject.name[7] == '4') {
                if (linkNodeTran[i].name[0] == 'L' || linkNodeTran[i].name[0] == 'R')
                    rig.AddForce((linkNodeTran[i].position - transform.position).normalized * forge * 0.8f * (Vector3.Magnitude(linkNodeTran[i].position - transform.position) - nodeDistance));
            }
            else*/
            
           rig.AddForce((linkNodeTran[i].position - transform.position).normalized * forge *(Vector3.Magnitude(linkNodeTran[i].position - transform.position) - nodeDistance));
          
        }


        if (linkNodeTran[0] && left)
        {
            line[0].transform.up = (linkNodeTran[0].position - transform.position).normalized;//mark
            line[0].transform.localScale = new Vector3(linewide, Vector3.Magnitude(linkNodeTran[0].position - transform.position)* 2f, linewide);
        }
        if (linkNodeTran[1] && right)
        {
            line[1].transform.up = (linkNodeTran[1].position - transform.position).normalized;//mark
            line[1].transform.localScale = new Vector3(linewide, Vector3.Magnitude(linkNodeTran[1].position - transform.position) * 2f, linewide);
        }
        if (linkNodeTran[2] && up)
        {
            line[2].transform.up = (linkNodeTran[2].position - transform.position).normalized;//mark
            line[2].transform.localScale = new Vector3(linewide, Vector3.Magnitude(linkNodeTran[2].position - transform.position) * 2f, linewide);
        }
        if (linkNodeTran[3] && down)
        {
            line[3].transform.up = (linkNodeTran[3].position - transform.position).normalized;//mark
            line[3].transform.localScale = new Vector3(linewide, Vector3.Magnitude(linkNodeTran[3].position - transform.position) * 2, linewide);
        }



        GetComponent<BoxCollider>().size = new Vector3(Mathf.Abs(PlayerMovement.dis), GetComponent<BoxCollider>().size.y, GetComponent<BoxCollider>().size.z);
        /*
		//原方案，无用
		//求两点距离时，用平方会比开方好
		//realDistance = Vector3.Magnitude (lastNodeTran.position - transform.position)-Vector3.Magnitude (nextNodeTran.position - transform.position);
		//if (Vector3.Magnitude (lastNodeTran.position - transform.position)> nodeDistance *nodeDistance ) { //用平方来比较
			//已经超过
			//transform.position = Vector3.Lerp ((lastNodeTran.position+nextNodeTran.position)/2 , transform.position, 2*nodeDistance / realDistance);
		//}
		*/

    }
    Vector3 _velo;
    void Deceleration() //减速
	{
      // if(rig.velocity.sqrMagnitude>10)
           //Debug.Log(rig.velocity.sqrMagnitude);

        
        if (rig.velocity.sqrMagnitude>20)
		{
            rig.velocity = _velo;
		}
		if (rig.velocity.sqrMagnitude<20&& rig.velocity.sqrMagnitude!=0)
		{          
			rig.velocity =Vector3.zero;
		}
        _velo = rig.velocity;
    }

	void WebJump()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rig.AddForce( transform.up * forge*5);
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Fish")) {
            //rig.AddForce(-transform.forward * forge * 8f);
        }
            
        if (collision.gameObject.CompareTag("Rock"))
            rig.AddForce(-transform.forward * forge * 10f);
    }

}
