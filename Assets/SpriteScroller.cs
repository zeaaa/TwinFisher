
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour
{

    [SerializeField]
    int id;

    [SerializeField]
    float length;

    [SerializeField]
    float speed;

    [SerializeField]
    float time = 1.0f;

    float startY;

    [SerializeField]
    AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {   
        float y = curve.Evaluate((Time.time / time) % 1);

        transform.position = new Vector3( transform.position.x - speed* Time.deltaTime, startY +y -1.0f,transform.position.z);
        ///transform.Translate(new Vector3(-1.0f,y)*speed);
        if (transform.position.x<(-length) + id*length)
            transform.position = new Vector3(transform.position.x + length,transform.position.y,transform.position.z);
    }
}
