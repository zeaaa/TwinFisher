using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVScroll : MonoBehaviour
{
    [Rename("左右移动速度")]
    [SerializeField]
    float USpeed;
    [Rename("上下浮动曲线")]
    [SerializeField]
    AnimationCurve curve;
    [Rename("一次上下浮动的时长")]
    [SerializeField]
    float time;
    Image tex;

    void Start()
    {
        if (null == tex)
            tex = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        
        float newOffsetU = USpeed * Time.time;
        float newOffsetV = curve.Evaluate((Time.time/time)%1);
 
 
            tex.material.mainTextureOffset = new Vector2(newOffsetU, -newOffsetV);

        
    }
}