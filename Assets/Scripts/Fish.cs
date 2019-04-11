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

    public void SetFish(float speed, int score, float length, float weight)
    {
        base.SetSpeed(speed);
        _score = score;
        _length = length;
        _weight = weight;
    }


    private void Start()
    {
        animLength = TFUtility.GetLengthByName(GetComponent<Animator>(), "jump");
    }
    private void FixedUpdate() {
        //Debug.Log(GetComponent<Rigidbody>().velocity.z);
        if (inCollision == false)
            GetComponent<Rigidbody>().velocity = Vector3.back * (_speed + PathManager.GetCurSpeed()) / Time.fixedUnscaledDeltaTime;
        else {       
            //if (GetComponent<Rigidbody>().velocity.z > 0)
               // GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        base.DestroyWhenOutofMap();
    }

    IEnumerator FishColiWithWeb()
    {
       
        GetComponent<Animator>().Play("jump");
        GetComponentInChildren<Renderer>().material.SetTexture("_MainTex",tex);
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
         //GetComponent<Rigidbody>().velocity = Vector3.zero;

         //GetComponent<Collider>().enabled = false;

         AddScoreHandler(_score, _weight);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(animLength - 0.5f);
        Destroy(this.gameObject);
    }
}
