
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePosition : MonoBehaviour {

	//绳子核心算法：

	//判断两个节点的距离，对节点加弹力

	float nodeDistance =0.1f;  //节点定长
	public float forge=500f;
    public Transform[] linkNodeTran;
    Rigidbody rig;

    float linewide=0.4f;
    public GameObject pf;

    GameObject[] line;



    private void Start()

	{
       
        rig = GetComponent<Rigidbody>();
        //生成连接线
        line = new GameObject[linkNodeTran.Length];
        for (int i = 0; i < linkNodeTran.Length; i++)
        {
            line[i]=Instantiate(pf, transform.position, Quaternion.identity);
            line[i].transform.parent = gameObject.transform;
            //Debug.Log("0++++++++++++++++++++"+i);
        }
    }

	private void Update()
	{
		Judge();
		WebJump ();
		Deceleration();
        AddForce();
	}


    void AddForce() {
        rig.AddForce(Vector3.back*forge*0.02f * Mathf.Abs(PlayerMovement.dis));
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

        //控制指向所有节点的连线
        for (int i = 0; i < linkNodeTran.Length; i++)
        {
            line[i].transform.up= (linkNodeTran[i].position - transform.position).normalized;//mark
            line[i].transform.localScale = new Vector3(linewide, Vector3.Magnitude(linkNodeTran[i].position - transform.position) *2  , linewide);
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
           // rig.AddForce(-transform.forward * forge * 10f);
        }
            
        if (collision.gameObject.CompareTag("Rock"))
            rig.AddForce(-transform.forward * forge * 10f);
    }

}
