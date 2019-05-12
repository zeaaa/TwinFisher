using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodUI : MonoBehaviour
{

    Color c = new Color(19/256f, 179/256f,180/256f);
    // Start is called before the first frame update


    private void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", c);
    }

    private void OnMouseOver()
    {
        
       
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", 0.2f*Color.black);
    }

    private void OnMouseDown()
    {
        CharacterJoint[] cjs = GetComponents<CharacterJoint>();
        foreach (CharacterJoint cj in cjs) {
            Destroy(cj);
        }
        //Destroy(GetComponent<Rigidbody>());
        //transform.Translate(Vector3.left);
        GetComponent<Rigidbody>().velocity = Vector3.down * 20f;
    }
}
