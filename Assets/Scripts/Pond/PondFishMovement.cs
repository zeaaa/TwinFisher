using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
enum FishState {
    idle,
    move,
    show
}

public class PondFishMovement : MonoBehaviour
{

    DOTweenPath path;


    FishState state;


    float timer;
    Vector3 target;

    [SerializeField]
    Transform lookAtCamera;

    
    [Rename("游多久停")]
    [SerializeField]
    float waitTime = 5.0f;
    [Rename("停多久")]
    [SerializeField]
    float idleTime = 10.0f;
    float startTime = 10.0f;

    float m_waitTime;
    float m_idleTime;

    Animator anim;
    private int area;
    // Start is called before the first frame update
    void Awake()
    {
        
        anim = GetComponent<Animator>();
        path = GetComponent<DOTweenPath>();
        //path.duration = speed;
        state = FishState.move;
        //path.loopType = LoopType.Incremental;
        //path.loops = -1;
    }

    private void Start()
    {
        m_waitTime = Random.Range(0, 5);
        m_idleTime = Random.Range(0, 5);
       
    }

    Vector3 stayPos;

    public void OnShow() {
        if (state != FishState.show)
            StartCoroutine(Show());
    }

    IEnumerator Show() {
        FishState temp = state;
        state = FishState.show;
             

        if (temp == FishState.move)
            path.DOPause();
        if (temp == FishState.idle)
            anim.speed = 1.0f;

        state = FishState.show;
        Vector3 towards = transform.forward;
       
        transform.DOLookAt(lookAtCamera.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        anim.Play("jump");
        yield return new WaitForSeconds(0.5f);
        //transform.DOLookAt(towards, 0.5f);
        //yield return new WaitForSeconds(0.5f);

        state = temp;
        if(state == FishState.idle)
            anim.speed = 0.1f;
        if (state == FishState.move)
            path.DOPlay();
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = new Quaternion(-0.1f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        switch (state) {
            case FishState.idle: {
                    timer += Time.deltaTime;
                    if (timer>m_idleTime) {
                        timer = 0;
                        m_idleTime = idleTime + TFMath.GaussRand() * idleTime;
                        anim.speed = 1.0f;
                        state = FishState.move;
                        path.DOPlay();
                    }
                    

                    break;
                }
            case FishState.move:
                {
                    timer += Time.deltaTime;
                    if (timer > m_waitTime) {
                        timer = 0;
                        m_waitTime = waitTime + TFMath.GaussRand() * waitTime;
                        anim.speed = 0.1f;
                        state = FishState.idle;
                        path.DOPause();
                    }
                    break;
                }
            case FishState.show:
                {
                    break;
                }
            default:break;
        }
    } 
}
