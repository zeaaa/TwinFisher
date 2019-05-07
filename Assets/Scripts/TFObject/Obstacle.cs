using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : TFObject{
    bool gameOver = false;
    public delegate void GameOver(int i);
    public static event GameOver GameOverHandler;
    protected override void OnCollisionWithPlayer(Collision col)
    {
        Debug.Log("colli");
        if (!gameOver) {
            GameOverHandler(0);
            col.gameObject.GetComponent<Animator>().SetInteger("GameOver", 0);
            gameOver = true;
        }
            
    }

    protected override void OnCollisionWithWebNode(Collision col)
    {
        if (!gameOver)
        {
            GameOverHandler(1);
            gameOver = true;
        }
    }

    protected override void OnCollisionWithWebPole(Collision col)
    {
       
    }

    public void SetObstacle(float speed) {
        base.SetSpeed(speed);
    }

    private void FixedUpdate()
    {
        SetSpeed(PathManager.GetCurSpeed());
        transform.Translate(Vector3.back * _speed);
        base.DestroyWhenOutofMap();
    }

    private void Awake()
    {
       SetObstacle(PathManager.curspeed);
    }

    protected override void OnCollisionWithPlayer(string s,Collision col)
    {
        throw new NotImplementedException();
    }
}
