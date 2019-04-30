using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dock : MonoBehaviour {
    public delegate void DockHit();
    public static event DockHit DockHitHandler;
    bool isOpen;
    Transform dock;
    Transform tree;
    float speed;
    private void Awake()
    {
        speed = 0;
        dock = transform.Find("1");
        tree = transform.Find("2");
        Open(false);
    }

    private void OnTriggerEnter(Collider colli)
    {
        
        if (colli.gameObject.tag == "Player" && isOpen)
        {
            DockHitHandler();
            //TODO 防止多次触发
            Debug.Log("hit wharf" + colli.gameObject.name);
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.z < 40f)
        {
            Open(false);
        }
    }

    public void Open(bool b) {
        isOpen = b;
        dock.gameObject.SetActive(b);
        tree.gameObject.SetActive(!b);
    }
}
