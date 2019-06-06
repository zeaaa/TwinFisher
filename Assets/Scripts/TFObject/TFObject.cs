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
        if(!GameManager.gameState.Equals(GameState.GameOver))
        if (col.gameObject.CompareTag("WebNode"))
        {
            OnCollisionWithWebNode(col);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            OnCollisionWithPlayer(col);
        }
        else if (col.gameObject.CompareTag("WebPole"))
        {
            OnCollisionWithWebPole(col);
        }
        else if (col.gameObject.CompareTag("PlayerForFish")) {
            OnCollisionWithPlayer("Fish",col);
        }
    }


    protected void DestroyWhenOutofMap()
    {
        if (transform.position.z <50f)
        {
            Destroy(this.gameObject);
        }
    }
    

    protected abstract void OnCollisionWithWebNode(Collision col);
    protected abstract void OnCollisionWithPlayer(Collision col);
    protected abstract void OnCollisionWithWebPole(Collision col);
    protected abstract void OnCollisionWithPlayer(string s, Collision col);


}
