using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour {
    [SerializeField]
    Text t_score;
    [SerializeField]
    Text t_skill;
    // Use this for initialization
    [SerializeField]
    RectTransform r_gameOver;
    [SerializeField]
    Image bg;
    [SerializeField]
    Button b_reStart;
    [SerializeField]
    Button b_back;
    [SerializeField]
    Slider s_capacity;

    [SerializeField]
    Image tbc;

    private void Awake()
    {
        GameManager.UpdateUIHandler += UpdateUI;
        GameManager.MGameOverHandler += ShowGameOverUI;
    }
    private void Start()
    {
        b_reStart.onClick.AddListener(delegate () { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); Time.timeScale = 1; });
        b_back.onClick.AddListener(delegate () { SceneManager.LoadScene(0); Time.timeScale = 1; });
        b_reStart.interactable = false;
        b_back.interactable = false;
        
    }

    private void OnDestroy()
    {
        GameManager.UpdateUIHandler -= UpdateUI;
        GameManager.MGameOverHandler -= ShowGameOverUI;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void UpdateUI(int score, int skillTimes, float capacity) {
        t_score.text = "score:" + score.ToString();
        t_skill.text = "skill:" + skillTimes.ToString();
        s_capacity.DOValue(capacity > 1 ? 1 : capacity, 0.5f).SetUpdate(true); ;
    }

    void ShowGameOverUI() {
        StartCoroutine(IEShowGameOverUI());
    }

    IEnumerator IEShowGameOverUI() {
        yield return new WaitForSeconds(1f);
        //Time.timeScale = 0;
        bg.DOFade(0.5f, 1f).SetUpdate(true);
        Tweener move = r_gameOver.DOLocalMove(Vector3.zero, 1.0f);     
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        move.onComplete = delegate (){
            r_gameOver.DOShakeRotation(3.0f, 5.0f).SetUpdate(true);
            b_reStart.interactable = true;
            b_back.interactable = true;        
        };

        Tweener movetbc = tbc.rectTransform.DOAnchorPosX(0, 1.0f);
        movetbc.SetEase(Ease.InQuint);
        movetbc.SetUpdate(true);
    }

}
