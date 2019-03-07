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
public class TFObject : MonoBehaviour
{
    [SerializeField]
    TFObjectType objectTpye;
    public delegate void Colision(int score,float weight);
    public static event Colision AddScoreHandler;

    public delegate void GameOver();
    public static event GameOver GameOverHandler;


    //set fish type
    public void SetTFObject(float length, float weight, int score, float speed) {
        objectTpye = TFObjectType.fish;
        _scoreValue = score;
        _weight = weight;
        _length = length;
        _speed = speed;
        fishCurSpeed = speed;
    }
    //set rock type
    public void SetTFObject() {
        objectTpye = TFObjectType.rock;
        _speed = GameManager.Speed;
    }

    [ReadOnly]
    [SerializeField]
    private int _scoreValue = 0; 
    [SerializeField]
    [ReadOnly]
    private float _weight = 0;
    [SerializeField]
    [ReadOnly]
    private float _length = 0;
    [ReadOnly]
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    float fishCurSpeed;


    float timer = 0;
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && objectTpye == TFObjectType.fish)
        {
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0.5f);
            fishCurSpeed = _speed * 1.1f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            switch (objectTpye)
            {
                case TFObjectType.fish:
                    {
                        GetComponent<Animator>().Play("speed");
                        fishCurSpeed = _speed * 0.5f;                 
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
            }
        }else
        if (col.gameObject.tag=="Web")
        {
            Debug.Log("a");
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
            }          
        }
    }

    IEnumerator FishColiWithWeb() {
        GetComponent<Animator>().Play("jump");
       
        this.fishCurSpeed = 0;
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
       
        AddScoreHandler(_scoreValue, _weight);
        yield return new WaitForSeconds(0.5f);
        //Destroy(this.gameObject, TFUtility.GetLengthByName(GetComponent<Animator>(), "jump"));
        Destroy(this.gameObject, 0.5f);
    }

    private void FixedUpdate()
    {

        switch (objectTpye)
        {
            case TFObjectType.fish:
                {
                    GetComponent<Rigidbody>().velocity = Vector3.back * fishCurSpeed / Time.fixedDeltaTime;
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

}
