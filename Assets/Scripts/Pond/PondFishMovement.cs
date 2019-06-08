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
    public float speed;
    float timer;
    Vector3 target;

    [SerializeField]
    Transform lookAtCamera;

    

    float waitTime = 2.0f;
    float idleTime = 1.0f;

    Animator anim;
    private int area;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        path = GetComponent<DOTweenPath>();
        state = FishState.move;
        //path.loopType = LoopType.Incremental;
        //path.loops = -1;
    }

    private void Start()
    {

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
       
        switch (state) {
            case FishState.idle: {
                    timer += Time.deltaTime;
                    if (timer>idleTime) {
                        timer = 0;
                        anim.speed = 1.0f;
                        state = FishState.move;
                        path.DOPlay();
                    }
                    

                    break;
                }
            case FishState.move:
                {
                    timer += Time.deltaTime;
                    if (timer > waitTime) {
                        timer = 0;
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
