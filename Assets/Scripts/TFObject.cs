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
    float timer = 0;

    /*
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && objectTpye == TFObjectType.fish)
        {
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0.5f);
            //fishCurSpeed = _speed * 1.1f;
        }
    }
    */
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
        if (transform.position.z < -10f)
        {
            Destroy(this.gameObject);
        }
    }
    

    protected abstract void OnCollisionWithWebNode();
    protected abstract void OnCollisionWithPlayer();
    protected abstract void OnCollisionWithWebPole();

}
