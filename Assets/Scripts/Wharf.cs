using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wharf : MonoBehaviour {
    public delegate void Dock();
    public static event Dock DockHandler;

    Transform dock;
    Transform tree;

    private void Start()
    {
        speed = 0;
        dock = transform.Find("1");
        tree = transform.Find("2");
    }

    float speed;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DockHandler();
            //TODO 防止多次触发
            Debug.Log("hit wharf");
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back*speed);
        if (transform.position.z < -10f) {
            Destroy(this.gameObject);
        }
    }

    public void Open(bool b) {
        dock.gameObject.SetActive(b);
        tree.gameObject.SetActive(!b);
    }
}
