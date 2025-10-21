using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWithDelay(int index, float delay = 1f)
    {
        StartCoroutine(LoadSceneCoroutine(index, delay));
    }

    private IEnumerator LoadSceneCoroutine(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

        while (!operation.isDone)
            yield return null;
    }
}
