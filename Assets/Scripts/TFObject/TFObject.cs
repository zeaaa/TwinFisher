using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class TFObject : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    public float _speed = 0;

    protected void SetSpeed(float speed) { _speed = speed; }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("WebNode"))
        {
            OnCollisionWithWebNode();
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            OnCollisionWithPlayer();
        }      
        else if (col.gameObject.CompareTag("WebPole"))
        {
            OnCollisionWithWebPole();
        }
    }


    protected void DestroyWhenOutofMap()
    {
        if (transform.position.z <50f)
        {
            Destroy(this.gameObject);
        }
    }
    

    protected abstract void OnCollisionWithWebNode();
    protected abstract void OnCollisionWithPlayer();
    protected abstract void OnCollisionWithWebPole();

}
