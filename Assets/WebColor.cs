using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WebColor : MonoBehaviour
{
    public Color emptyColor;
    public Color fullColor;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public bool vertical = true;

    [SerializeField]
    Gradient g;

    [Range(0,1)]
    public float value = 1.0f;
    Mesh mesh;
    Vector3[] v3;
    List<Color> lc;

    Color leftColor;
    Color rightColor;

    const int totalLength = 6;
    public int id;

    public bool flip = false;

    [ReadOnly]
    public float rangeStart;
    [ReadOnly]
    public float rangeEnd;
    void Awake()
    {
        rangeStart = (id) / (totalLength);
        rangeEnd = (id + 1) / (totalLength);
        UIManager.onCapacityChanged += SetValue;
        leftColor = emptyColor;
        rightColor = emptyColor;
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        mesh = meshFilter.mesh;
        v3 = mesh.vertices;
        lc = new List<Color>();
        if (vertical)
        {
            for (int i = 0; i < v3.Length; i++)
            {
                Color c;
                float scaler = flip ? -1.0f : 1.0f;
                if (v3[i].y * scaler < 0)
                    c = emptyColor;
                else
                    c = emptyColor;
                lc.Add(new Color(c.r, c.g, c.b));
            }
            mesh.colors = lc.ToArray();
        }
    }

    private void Update()
    {
        if (value < 0.5f)
            for (int i = 0; i < v3.Length; i++)
            {
                Color c;
                float scaler = flip ? -1.0f : 1.0f;
                if (v3[i].y * scaler < 0)
                    c = Color.Lerp(emptyColor, fullColor, value * 2);
                else
                    c = emptyColor;
                lc[i] = new Color(c.r, c.g, c.b);
            }
        else
            for (int i = 0; i < v3.Length; i++)
            {
                Color c;
                float scaler = flip ? -1.0f : 1.0f;
                if (v3[i].y * scaler < 0)
                    c = fullColor;
                else
                    c = Color.Lerp(emptyColor, fullColor, (value - 0.5f) * 2);
                lc[i] = new Color(c.r, c.g, c.b);
            }
        mesh.colors = lc.ToArray();
    }

    public void SetValue(object Sender,FloatArgs args) {
        

        //rangeStart = (id) / (float)totalLength;
        //rangeEnd = (id + 1) / (float)totalLength;
        //float mainV = args.value;
        //Debug.Log(mainV);
        //if (vertical)
        //{
        //    if (mainV > rangeStart && mainV <= rangeEnd) {
        //        value = (mainV - rangeStart) * totalLength;

        //    } else if (mainV < rangeStart){
        //        value = 0;
        //    }
        //    else {
        //        value = 1;
        //    }
        //    if (value < 0.5f)
        //        for (int i = 0; i < v3.Length; i++)
        //        {
        //            Color c;
        //            float scaler = flip ? -1.0f : 1.0f;
        //            if (v3[i].y * scaler < 0)
        //                c = Color.Lerp(emptyColor, fullColor, value * 2);
        //            else
        //                c = emptyColor;
        //            lc[i] = new Color(c.r, c.g, c.b);
        //        }
        //    else
        //        for (int i = 0; i < v3.Length; i++)
        //        {
        //            Color c;
        //            float scaler = flip ? -1.0f : 1.0f;
        //            if (v3[i].y * scaler < 0)
        //                c = fullColor;
        //            else
        //                c = Color.Lerp(emptyColor, fullColor, (value - 0.5f) * 2);
        //            lc[i] = new Color(c.r, c.g, c.b);
        //        }
        //    mesh.colors = lc.ToArray();

        //}


        
    }



}
