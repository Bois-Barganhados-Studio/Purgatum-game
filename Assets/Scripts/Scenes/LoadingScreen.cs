using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject LoadingScreenPanel;
    public Image LoadingBarFill;
    public float speed;
    public float delayDuration = 10f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        LoadingScreenPanel.SetActive(true);

        yield return new WaitForSeconds(delayDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        while (!operation.isDone)
        {
            yield return null;
        }
    }
}