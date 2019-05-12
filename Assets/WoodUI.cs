using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodUI : MonoBehaviour
{
    [SerializeField]
    int id;
    Color c = new Color(19/256f, 179/256f,180/256f);
    // Start is called before the first frame update

    private void Update()
    {
        if (transform.position.y < -50)
            Destroy(this.gameObject);
    }

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
        GetComponent<Rigidbody>().velocity = Vector3.down * 20f;
        if (id == 0)
            WoodUIManager.instance.StartLoad();
        if (id == 1)
            WoodUIManager.instance.EnterPond();
    }
}
