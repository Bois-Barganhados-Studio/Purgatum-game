using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void SceneLoadedDelegate(Scene scene, LoadSceneMode mode);

public class LoadingScreen : MonoBehaviour
{
    public GameObject LoadingScreenPanel;
    public Image LoadingBarFill;
    public float speed;
    public float delayDuration = 10f;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    public void LoadScene(int sceneId, SceneLoadedDelegate onSceneLoaded)
    {
        StartCoroutine(LoadSceneAsync(sceneId, onSceneLoaded));
    }

    IEnumerator LoadSceneAsync(int sceneId, SceneLoadedDelegate onSceneLoaded = null)
    {
        LoadingScreenPanel.SetActive(true);

        yield return new WaitForSeconds(delayDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        if (onSceneLoaded != null)
        {
            operation.completed += (asyncOperation) =>
            {
                onSceneLoaded?.Invoke(SceneManager.GetSceneByBuildIndex(sceneId), LoadSceneMode.Single);
            };
        }

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelAsync());
    }

    IEnumerator LoadNextLevelAsync()
    {
        LoadingScreenPanel.SetActive(true);

        yield return new WaitForSeconds(delayDuration);

        /*AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!operation.isDone)
        {
            yield return null;
        }*/
        
        LoadingScreenPanel.SetActive(false);
    }
}