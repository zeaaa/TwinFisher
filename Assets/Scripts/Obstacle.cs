using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : TFObject{

    public delegate void GameOver();
    public static event GameOver GameOverHandler;
    protected override void OnCollisionWithPlayer()
    {
        GameOverHandler();
        throw new NotImplementedException();
    }

    protected override void OnCollisionWithWebNode()
    {
        GameOverHandler();
    }

    protected override void OnCollisionWithWebPole()
    {
        GameOverHandler();
    }

    public void SetObstacle(float speed) {
        base.SetSpeed(speed);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * _speed);
    }
}
