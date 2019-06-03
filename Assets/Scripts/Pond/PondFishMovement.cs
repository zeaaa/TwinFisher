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

    public GameObject cube;
    Animator anim;
    private int area;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        
        nav = gameObject.GetComponent<NavMeshAgent>();
        timer = 0;
        okVertice = new List<int>();
        area = nav.areaMask;
        navData = NavMesh.CalculateTriangulation();
    }

    private void Start()
    {
        anim.speed = 0.1f;
        StartCoroutine(RandMove());
    }

    // Update is called once per frame
    void Update()
    {


        timer += Time.deltaTime;

        if( (nav.destination - target).magnitude < 0.1f){
            anim.speed = 0.1f;
        }
    }
    public void SetFishSpeed(float speed)
    {
        nav.speed = speed;
    }
    IEnumerator RandMove()
    {
        while (true)
        {
            float x = Random.Range(4, 10f);
            yield return new WaitForSeconds(x);
            target = GetRandomLocation();
            nav.SetDestination(target);
            anim.speed = 1f;

            //if(!nav.hasPath){nav.SetDestination(transform.position);continue;}
            timer = 0;
            yield return new WaitUntil(Wait);
        }
    }

    bool Wait() {
        if ((nav.destination - target).magnitude < 0.1f && timer > 20.0f)
            return true;
        else
            return false;
    }

    List<int> okVertice;
    public Vector3 GetRandomLocation()
    {

        //navData = NavMesh.CalculateTriangulation();

        
   
        for (int i = 0; i < navData.areas.Length; i+=3) {
            if (1<<navData.areas[i / 3] == area) {
               
                okVertice.Add(i);
            }
        }
        int t = Random.Range(0,okVertice.Count);
        Vector3 point = Vector3.Lerp(navData.vertices[navData.indices[okVertice[t]]], navData.vertices[navData.indices[okVertice[t] + 1]], Random.value);
        point = Vector3.Lerp(point, navData.vertices[navData.indices[okVertice[t] + 2]], Random.value);
        okVertice.Clear();
        return point;
    }
}
