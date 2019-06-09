using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class FishUpDown : MonoBehaviour
{
    [SerializeField]
    float offset;
    [SerializeField]
    float speed;
    float curY;
    // Start is called before the first frame update
    void Awake()
    {
        curY = transform.position.y;
        //StartCoroutine(GoUp(0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up*dir*Time.deltaTime*speed);
    }

    IEnumerator GoUp(float time) {
        transform.DOMoveY(curY + offset, time);
        yield return new WaitForSeconds(time);
        StartCoroutine(GoDown(0.47f));
    }

    IEnumerator GoDown(float time)
    {
        transform.DOMoveY(curY - offset, time);
        yield return new WaitForSeconds(time);
        StartCoroutine(GoUp(0.53f));
    }
    float dir = 1.0f;

    public void GoUpEvent() {
        dir = -1.0f;
    }

    public void GoDownEvent()
    {
        dir = 1.0f;
    }

    
}
