using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    int targetMaterialSlot=0;
    //var scrollThis:Material;
    float speedY=0.5f;
    float speedX=0.0f;
    private float timeWentX=0;
    private float timeWentY=0;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent< Renderer > ();
        rend.materials[targetMaterialSlot].SetTextureOffset("_MainTex",new Vector2(timeWentX, timeWentY));
       
    }

    // Update is called once per frame
    private void OnEnable()
    {
        rend = GetComponent< Renderer > ();

        rend.materials[targetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(0, 0));
        timeWentX = 0;
        timeWentY = 0;
    }

    private void Update()
    {
        timeWentY += Time.deltaTime * speedY;
        timeWentX += Time.deltaTime * speedX;
        rend.materials[targetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(timeWentX, timeWentY));
    }
}
