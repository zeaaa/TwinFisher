using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraRotate : MonoBehaviour
{

    [SerializeField]
    AnimationCurve curve;
    DOTweenPath path;
    // Start is called before the first frame update
    void Start()
    {
        path= GetComponent<DOTweenPath>();
        path.easeCurve = curve;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
