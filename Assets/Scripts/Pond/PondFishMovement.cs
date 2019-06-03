using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PondFishMovement : MonoBehaviour
{

    [SerializeField]
    UnityEngine.AI.NavMeshAgent nav;
    NavMeshTriangulation navData;
    public float speed;
    float timer;
    Vector3 target;

    private int area;
    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        timer = 0;
        area = 4;
        Debug.Log(nav.areaMask);
        navData = NavMesh.CalculateTriangulation();
        StartCoroutine(RandMove());
        
       
        
    }

    // Update is called once per frame
    void Update()
    {


        timer += Time.deltaTime;
        //Debug.Log((nav.destination-target).magnitude);
    }
    public void SetFishSpeed(float speed)
    {

        nav.speed = speed;
    }
    IEnumerator RandMove()
    {
        while (true)
        {
            target = GetRandomLocation();
            nav.SetDestination(target);

            //if(!nav.hasPath){nav.SetDestination(transform.position);continue;}
            timer = 0;
            yield return new WaitUntil(TestWait);

        }
    }
    bool TestWait()
    {

        if ((nav.destination - target).magnitude < 0.6f || timer > 5f) return true;
        else return false;
    }
    
    public Vector3 GetRandomLocation()
    {

        navData = NavMesh.CalculateTriangulation();

        List<int> okVertice;
        okVertice = new List<int>();
        for (int i = 0; i < navData.areas.Length; i+=3) {
            if (navData.areas[i / 3] == area) {
                okVertice.Add(i);
            }
        }
        Debug.Log(okVertice.Count);
        int t = Random.Range(0,okVertice.Count);
        //Debug.Log(t);

        //for(int i=0;i<p.Length;i++)p[0+i].transform.position= navMeshData.vertices[t+i];

        Vector3 point = Vector3.Lerp(navData.vertices[t*3], navData.vertices[t*3 + 1], Random.value);
        point = Vector3.Lerp(point, navData.vertices[t*3 + 2], Random.value);


        return point;
    }
}
