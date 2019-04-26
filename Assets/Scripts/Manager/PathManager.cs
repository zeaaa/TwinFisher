using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PathManager : MonoBehaviour {

    [SerializeField]
    Transform pathTransform;

    [ReadOnly]
    [SerializeField]
    float mileage = 0;

    //Just for showing on inspector
    [ReadOnly]
    [SerializeField]
    float speed = 0;

    public static float curspeed = 0;

    [Rename("场景最大速度")]
    [SerializeField]
    float _maxSpeed;


    [Rename("加速所需时间")]
    [SerializeField]
    float _accelTime;

    [Rename("开场镜头动画")]
    [SerializeField]
    bool cameraAnim = true;

    List<BGScroller> paths;



    public static float GetCurSpeed() {
        return curspeed;
    }
    
	// Use this for initialization
	void Awake () {
        paths = new List<BGScroller>();
        BGScroller[] bgs = pathTransform.GetComponentsInChildren<BGScroller>();
        for (int i = 0; i < bgs.Length; i++) {
            paths.Add(bgs[i]);
        }
        Obstacle.GameOverHandler += StopScrolling;
    }

    private void OnDestroy()
    {
        Obstacle.GameOverHandler -= StopScrolling;
    }

    void StopScrolling(int i) {
        if (i == 0) {
            curspeed = 0;
            SetSpeed(0);
        }     
        else
            StartCoroutine(ChangeSpeed(0f, 0.5f));
    }

    void Start()
    {
        SetSpeed(0);
        float cameraAnimTime = 3.0f;
        if (cameraAnim)
        {
            Camera.main.transform.DOMoveZ(45f, cameraAnimTime).SetEase(Ease.Linear).onComplete = delegate { StartCoroutine(ChangeSpeed(_maxSpeed, _accelTime)); };
            Camera.main.transform.DORotate(new Vector3(50, 0, 0), cameraAnimTime);
        }
        else {
            StartCoroutine(ChangeSpeed(_maxSpeed, _accelTime));
            Camera.main.transform.position = new Vector3(0,56f,45f);
            Camera.main.transform.localEulerAngles = new Vector3(50, 0, 0);
        }
       
    }

    private void FixedUpdate()
    {
        mileage += speed * Time.fixedDeltaTime;
        speed = curspeed;
    }


    IEnumerator ChangeSpeed(float endSpeed, float time) {
        WaitForFixedUpdate wffu = new WaitForFixedUpdate();
        float startTime = Time.time;
        float startSpeed = curspeed;
        while (true) {
            float timeSinceStarted = Time.time - startTime;
            float percentage= timeSinceStarted / time;
            curspeed = Mathf.Lerp(startSpeed, endSpeed, percentage);
            SetSpeed(curspeed);
            yield return wffu;
            if (percentage >= 1.0f)
            {
                break;
            }
        }
        SetSpeed(endSpeed);
    }

    void SetSpeed(float speed)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            paths[i].SetSpeed(speed);
        }
    }
}
