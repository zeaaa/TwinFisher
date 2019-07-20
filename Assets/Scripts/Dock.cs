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
    Transform woodPlate;

    private void Awake()
    {
        speed = 0;
        dock = transform.Find("1");
        woodPlate = transform.Find("Dock");
        tree = transform.Find("2");
        Open(false);
    }

    private void OnTriggerEnter(Collider colli)
    {
        
        if (colli.gameObject.tag == "Player" && isOpen)
        {


            DockHitHandler();
            transform.DOScale(1.1f, 0.2f).onComplete = delegate { transform.DOScale(1.0f, 0.2f); };
            AudioSource source;
            if (GetComponent<AudioSource>())
                source = GetComponent<AudioSource>();
            else
                source = gameObject.AddComponent<AudioSource>();
            source.clip = SoundManager.instance.dockSound;
            source.loop = false;
            source.Play();

            //TODO 防止多次触发
            Debug.Log("hit wharf" + colli.gameObject.name);
        }
    }

    private void FixedUpdate()
    {

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.left, out hit ,3f,1 << LayerMask.NameToLayer("Obstacle")))
        {
            Destroy(hit.collider.gameObject);
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 3f, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            Destroy(hit.collider.gameObject);
        }
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
