using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

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

    [SerializeField]
    Text Mile;

    public static float GetCurSpeed() {
        return curspeed;
    }
    
	// Use this for initialization
	void Awake () {
        rank = -1;
        paths = new List<BGScroller>();
        BGScroller[] bgs = pathTransform.GetComponentsInChildren<BGScroller>();
        for (int i = 0; i < bgs.Length; i++) {
            paths.Add(bgs[i]);
        }
        Obstacle.GameOverHandler += StopScrolling;
    }

     static int times = 0;

    private void OnDestroy()
    {
        Obstacle.GameOverHandler -= StopScrolling;
    }

    public static int rank;

    void StopScrolling(int i) {
       
        float distance = PlayerPrefs.GetFloat("Farthest");
        if(distance< mileage)
            PlayerPrefs.SetFloat("Farthest", mileage);
        float[] data = PlayerPrefsX.GetFloatArray("Rank", 0, 10);
        for (int k = 0; k< data.Length; k++) {
            if (data[k] < mileage) {
                for (int j = data.Length- 1 ; j > k; j--) {
                    data[j] = data[j - 1];
                }
                data[k] = mileage;
                rank = k+1;
                Debug.Log("set rank");
                break;
            }
        }
        
        //for (int k = 0; k < data.Length; k++) {
        //    print(data[k]);
        //}
        PlayerPrefsX.SetFloatArray("Rank",data);
        if (i == 0)
        {
            Debug.Log("stop");
            StopAllCoroutines();
            curspeed = 0;
            SetSpeed(0);
        }
        else {
            StartCoroutine(ChangeSpeed(0f, 0.5f));     
        }
            
    }

    void Start()
    {
        SetSpeed(0);
        float cameraAnimTime = 3.0f;
        if (cameraAnim)
        {
            Camera.main.transform.DOMoveZ(52f, cameraAnimTime).SetEase(Ease.Linear).onComplete = delegate { StartCoroutine(ChangeSpeed(_maxSpeed, _accelTime)); };
            Camera.main.transform.DORotate(new Vector3(50, 0, 0), cameraAnimTime);
        }
        else {
            StartCoroutine(ChangeSpeed(_maxSpeed, _accelTime));
            Camera.main.transform.position = new Vector3(0,46f,52f);
            Camera.main.transform.localEulerAngles = new Vector3(50, 0, 0);
        }
       
    }

    public const float scaler = 0.5f;

    float shoal = 500f;

    private void FixedUpdate()
    {
        mileage += speed * Time.fixedDeltaTime * 50;
        Mile.text = (mileage*scaler).ToString("0") + "米" ;
        speed = curspeed;

        if (mileage > shoal) {
            shoal += 500;
            OnOverDistance.Invoke(this, EventArgs.Empty);
        }
    }

    public static event EventHandler OnOverDistance;


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
        //PlayerPrefs.SetFloat("Farthest", mileage);
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
