using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName;
    private AsyncOperation async = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLoad(){
        StartCoroutine("LoadScene");
    }
    IEnumerator LoadScene(){
        
        async=SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        yield return new WaitForSeconds(3f);
        


        async.allowSceneActivation = true;
        yield return null;
    }
}
