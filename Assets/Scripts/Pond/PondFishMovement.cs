using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PondFishMovement : MonoBehaviour
{

    [SerializeField]
    UnityEngine.AI.NavMeshAgent nav;

    public float speed;
    float timer;
    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        timer = 0;
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
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        
        int t = Random.Range(0, navMeshData.vertices.Length - 3);

        
        for (int i = 0; i < navMeshData.areas.Length; i++) {
            Debug.Log(navMeshData.areas[i]);
        }
       //while(navMeshData.areas[t]!=)
       
        //for(int i=0;i<p.Length;i++)p[0+i].transform.position= navMeshData.vertices[t+i];

        Vector3 point = Vector3.Lerp(navMeshData.vertices[t], navMeshData.vertices[t + 1], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[t + 2], Random.value);


        return point;
    }
}
