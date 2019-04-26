using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Fish : TFObject
{
    [ReadOnly]
    [SerializeField]
    private int _score = 0;
    [ReadOnly]
    [SerializeField]
    private float _length = 0;
    [ReadOnly]
    [SerializeField]
    private float _weight = 0;

    private int _spId = 0;

    private bool inCollision = false;

    public delegate void Colision(int score, float weight);
    public static event Colision AddScoreHandler;

    [SerializeField]
    Texture2D tex;
    float animLength;
    protected override void OnCollisionWithPlayer()
    {
        if (inCollision == false)
        {
            StartCoroutine(FishColiWithWeb());
            inCollision = true;
        }
        //throw new NotImplementedException();
    }

    protected override void OnCollisionWithWebNode()
    {
        if (inCollision == false) {
            StartCoroutine(FishColiWithWeb());
            inCollision = true;
        }      
    }

    protected override void OnCollisionWithWebPole()
    {
       // throw new NotImplementedException();
    }

    public void SetFish(float speed, int score, float length, float weight,int spawnPointId)
    {
        base.SetSpeed(speed);
        _score = score;
        _length = length;
        _weight = weight;
        _spId = spawnPointId;
    }


    private void Start()
    {
        animLength = TFUtility.GetLengthByName(GetComponent<Animator>(), "jump");
    }

    int curHash = 0;

    bool inDodge = false;
    
    private void FixedUpdate() {
        Vector3 dir = Vector3.back;
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + Vector3.back * 10, Color.red);

        if ( Physics.Raycast(transform.position , Vector3.back, out hit, 10))
        {        
            curHash = hit.collider.gameObject.GetHashCode();
            if (!inDodge && (hit.collider.gameObject.CompareTag("Fish") || hit.collider.gameObject.CompareTag("Rock")))
            {
                inDodge = true;
                   
                    StartCoroutine(Dodge(curHash));
                   
                Debug.Log("Start Dodge");

            }
        }
        else
        {
             curHash = 0;
        }
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (!inCollision && !inDodge)
        {
           
            GetComponent<Rigidbody>().velocity = dir * (_speed + PathManager.GetCurSpeed()) / Time.fixedUnscaledDeltaTime;
        }
        else
        {
            //if (GetComponent<Rigidbody>().velocity.z > 0)
            // GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        base.DestroyWhenOutofMap();
    }

    IEnumerator Dodge(int hash){      
        GetComponent<Animator>().Play("speed");

        float dis1 = 100f;
        float dis2 = 100f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(1f, 0, -1f), out hit, 10)){
            dis1 = Vector3.Distance(hit.point,transform.position);
        }
        if (Physics.Raycast(transform.position, new Vector3(-1f, 0, -1f), out hit, 10))
        {
            dis2 = Vector3.Distance(hit.point, transform.position);
        }

        int d = dis1 > dis2 ? 1 : -1;
        //int d = (UnityEngine.Random.Range(0, 2) * 2 - 1);
        Vector3 dir = new Vector3(0.3f*d ,0,-0.3f);
        transform.localEulerAngles = new Vector3(0, -180f - d * 45f, 0);

        float timer = 0;
        WaitForFixedUpdate wffu = new WaitForFixedUpdate();
        int diffCounter = 0;
        while (true) {
            transform.localEulerAngles = new Vector3(0, -180f - d * 45f, 0);
            GetComponent<Rigidbody>().velocity = dir * (_speed + PathManager.GetCurSpeed()) / Time.fixedUnscaledDeltaTime;
            timer += Time.fixedDeltaTime;
            /*if (timer > 2f) {
                Debug.Log("End by time");
                break;
            }*/
            if (diffCounter > 5)
            {
                Debug.Log("End by exit");
                break;
            }      
            if (hash != curHash)
            {
                diffCounter++;
            }
            yield return wffu;
        }
        GetComponent<Animator>().Play("normal");
        transform.localEulerAngles = new Vector3(0, -180f, 0);
        inDodge = false;
    }

    IEnumerator FishColiWithWeb()
    {
        
        GetComponent<Animator>().Play("jump");
        GetComponentInChildren<Renderer>().material.SetTexture("_MainTex",tex);
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        AudioSource source= gameObject.AddComponent<AudioSource>();
       
        int size = SoundManager.instance.bubble.Length;
        int i = UnityEngine.Random.Range(0, size);
        source.clip = SoundManager.instance.bubble[i];
        source.Play();
        //GetComponent<Rigidbody>().velocity = Vector3.zero;

        //GetComponent<Collider>().enabled = false;
         
         AddScoreHandler(_score, _weight);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(animLength - 0.5f);
        Destroy(this.gameObject);
    }
}
