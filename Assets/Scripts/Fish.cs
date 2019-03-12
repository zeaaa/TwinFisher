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

    protected override void OnCollisionWithPlayer()
    { 
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
        SetSpeed(0.6f);
    }
    private void FixedUpdate() {
        GetComponent<Rigidbody>().velocity = Vector3.back * _speed / Time.fixedDeltaTime;
        base.DestroyWhenOutofMap();
    }

    IEnumerator FishColiWithWeb()
    {
       
        GetComponent<Animator>().Play("jump");

        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //GetComponent<Rigidbody>().velocity = Vector3.zero;

        //GetComponent<Collider>().enabled = false;

        AddScoreHandler(_score, _weight);
        float animLength = TFUtility.GetLengthByName(GetComponent<Animator>(), "jump");
        Debug.Log(animLength);
        yield return new WaitForSeconds(animLength);
        Destroy(this.gameObject);
    }
}
