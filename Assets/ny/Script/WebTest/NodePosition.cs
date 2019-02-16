
using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class NodePosition : MonoBehaviour {



	//绳子核心算法：

	//判断两个节点的距离，对节点加弹力

	public float nodeDistance = 3;  //节点定长
	public float forge=500f;
	//public Transform lastNodeTran;   //上一个节点的位置
	//public Transform nextNodeTran;	//下一个节点的位置
    public Transform[] linkNodeTran;
    Rigidbody rig;

    public float linewide=0.1f;
    public GameObject pf;

    private float length = 1.5f;

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

	}

	void Judge()

	{
		//附加与距离相关的弹力
        /*
		rig.AddForce((lastNodeTran.position - transform.position).normalized*forge*(Vector3.Magnitude (lastNodeTran.position - transform.position)-nodeDistance));
		rig.AddForce((nextNodeTran.position - transform.position).normalized*forge*(Vector3.Magnitude (nextNodeTran.position - transform.position)-nodeDistance));
        */
        //对左右节点的力
        for(int i=0;i<2;i++)
        {
            rig.AddForce((linkNodeTran[i].position - transform.position).normalized * forge * (Vector3.Magnitude(linkNodeTran[i].position - transform.position) - nodeDistance));
        }
        //控制指向所有节点的连线
        for (int i = 0; i < linkNodeTran.Length; i++)
        {
            line[i].transform.up= (linkNodeTran[i].position - transform.position).normalized;//mark
            line[i].transform.localScale = new Vector3(linewide, Vector3.Magnitude(linkNodeTran[i].position - transform.position) , linewide);
        }


        /*
		//原方案，无用
		//求两点距离时，用平方会比开方好
		//realDistance = Vector3.Magnitude (lastNodeTran.position - transform.position)-Vector3.Magnitude (nextNodeTran.position - transform.position);
		//if (Vector3.Magnitude (lastNodeTran.position - transform.position)> nodeDistance *nodeDistance ) { //用平方来比较
			//已经超过

			//transform.position = Vector3.Lerp ((lastNodeTran.position+nextNodeTran.position)/2 , transform.position, 2*nodeDistance / realDistance);
		//}
		//if (Vector3.Magnitude (nextNodeTran.position - transform.position)> nodeDistance *nodeDistance ) { //用平方来比较

			//已经超过

			//transform.position = Vector3.Lerp ((lastNodeTran.position+nextNodeTran.position)/2 , transform.position, 2*nodeDistance / realDistance);
		//}
		*/

    }

    void Deceleration() //减速

	{

		if (rig.velocity.sqrMagnitude>50)

		{

			rig.velocity /= 3;

		}
		if (rig.velocity.sqrMagnitude<10&& rig.velocity.sqrMagnitude!=0)

		{
           // Debug.Log(rig.velocity.sqrMagnitude);
			rig.velocity =Vector3.zero;


		}

	}
	void WebJump()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rig.AddForce( transform.up * forge*5);

		}
	}

}
