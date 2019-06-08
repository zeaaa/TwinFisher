using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class FishUpDown : MonoBehaviour
{
    [SerializeField]
    float offset;

    float curY;
    // Start is called before the first frame update
    void Awake()
    {
        curY = transform.position.y;
        StartCoroutine(GoUp(0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GoUp(float time) {
        transform.DOMoveY(curY + offset, time);
        yield return new WaitForSeconds(time);
        StartCoroutine(GoDown(0.5f));
    }

    IEnumerator GoDown(float time)
    {
        transform.DOMoveY(curY - offset, time);
        yield return new WaitForSeconds(time);
        StartCoroutine(GoUp(0.8f));
    }
}
