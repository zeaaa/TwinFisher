using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PondFishMovement : MonoBehaviour
{

    [SerializeField]
    UnityEngine.AI.NavMeshAgent nav;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        nav=gameObject.AddComponent<NavMeshAgent>();
        nav.height=1f;
        nav.radius=1f;
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination (GetRandomLocation());
    }
    public void SetFishSpeed(float speed) {
        
      
        nav.speed=speed;
    }

    public Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);

        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
}
