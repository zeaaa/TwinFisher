using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
enum TFObjectType
{
    fish = 0,
    powerUpItem = 1,
    rock = 2
}
public abstract class TFObject : MonoBehaviour
{
    [SerializeField]
    TFObjectType objectTpye;
   

  

  

    //set fish type
    /*public  void SetTFObject(float length, float weight, int score, float speed) {
        objectTpye = TFObjectType.fish;
        _scoreValue = score;
        _weight = weight;
        _length = length;
        _speed = speed;
        fishCurSpeed = speed;
    }*/
    //set rock type
   



    [ReadOnly]
    [SerializeField]
    public float _speed = 0;

    public virtual void test(float speed) { _speed = speed; }

    protected void SetSpeed(float speed) { _speed = speed; }
    float timer = 0;
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

    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            OnCollisionWithPlayer();
            /*
            switch (objectTpye)
            {
                case TFObjectType.fish:
                    {
                        GetComponent<Animator>().Play("speed");
                       // fishCurSpeed = _speed * 0.5f;                 
                        break;
                    }
                case TFObjectType.rock:
                    {
                        GameOverHandler();
                        break;
                    }
                case TFObjectType.powerUpItem:
                    {
                        break;
                    }
                default: break;
            }*/
        }else
        if (col.gameObject.tag=="Web")
        {
            OnCollisionWithWebNode();
            /*
            switch (objectTpye) {
                case TFObjectType.fish: {

                        StartCoroutine(FishColiWithWeb());
                        break;
                    }
                case TFObjectType.rock:
                    {
                        GameOverHandler();
                        break;
                    }
                case TFObjectType.powerUpItem:
                    {
                        break;
                    }
                default:break;
            }*/          
        }
    }



    

    private void FixedUpdate()
    {
        Debug.Log("fx");
        switch (objectTpye)
        {
            case TFObjectType.fish:
                {
                  //  GetComponent<Rigidbody>().velocity = Vector3.back * fishCurSpeed / Time.fixedDeltaTime;
                    break;
                }
            case TFObjectType.rock:
                {
                    transform.Translate(Vector3.back * _speed);
                    break;
                }
            case TFObjectType.powerUpItem:
                {
                    break;
                }
            default: break;
        }

        
        if (transform.position.z < -10f)
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void OnCollisionWithWebNode();
    protected abstract void OnCollisionWithPlayer();
    protected abstract void OnCollisionWithWebPole();

}
