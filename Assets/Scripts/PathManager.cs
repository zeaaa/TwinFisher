using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    [SerializeField]
    Transform pathTransform;

    [ReadOnly]
    [SerializeField]
    float mileage = 0;
    [ReadOnly]
    [SerializeField]
    float speed = 0;

    List<BGScroller> paths;

    
	// Use this for initialization
	void Awake () {
        paths = new List<BGScroller>();
        BGScroller[] bgs = pathTransform.GetComponentsInChildren<BGScroller>();
        for (int i = 0; i < bgs.Length; i++) {
            paths.Add(bgs[i]);
        }
    }

    void Start()
    {
       StartCoroutine(ChangeSpeed(GameManager.Speed, 3.0f));      
    }

    private void FixedUpdate()
    {
        mileage += speed * Time.fixedDeltaTime;
    }


    IEnumerator ChangeSpeed(float endSpeed, float time) {
        WaitForFixedUpdate wffu = new WaitForFixedUpdate();
        float startTime = Time.time;
        float startSpeed = speed;
        while (true) {
            float timeSinceStarted = Time.time - startTime;
            float percentage= timeSinceStarted / time;
            speed = Mathf.Lerp(startSpeed, endSpeed, percentage);
            SetSpeed(speed);
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
