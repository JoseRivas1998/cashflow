using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Slider slider;

    public void BeginLoading(string sceneToLoad)
    {
        StartCoroutine(LoadScene(sceneToLoad));
    }

    private IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncOperation.isDone)
        {
            slider.value = asyncOperation.progress;
            yield return null;
        }
    }

}
