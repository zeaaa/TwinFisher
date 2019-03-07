using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wharf : MonoBehaviour {
    public delegate void Dock();
    public static event Dock DockHandler;

    private void Start()
    {
        speed = GameManager.Speed;
    }

    float speed;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DockHandler();
            //TODO 防止多次触发
            //this.GetComponent<Renderer>().material.DOColor()
            transform.DOShakeScale(1.0f, 1.5f, 2).onComplete = delegate {
                
            };
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back*speed);
        if (transform.position.z < -10f) {
            Destroy(this.gameObject);
        }
    }
}
