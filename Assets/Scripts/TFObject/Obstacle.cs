using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : TFObject{
    bool gameOver = false;
    public delegate void GameOver(int i);
    public static event GameOver GameOverHandler;
    protected override void OnCollisionWithPlayer()
    {
        if (!gameOver) {
            GameOverHandler(0);
            gameOver = true;
        }
            
    }

    protected override void OnCollisionWithWebNode()
    {
        if (!gameOver)
        {
            GameOverHandler(1);
            gameOver = true;
        }
    }

    protected override void OnCollisionWithWebPole()
    {
        if (!gameOver)
        {
            GameOverHandler(0);
            gameOver = true;
        }
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

}
