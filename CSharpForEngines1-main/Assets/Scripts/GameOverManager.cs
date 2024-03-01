using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void LoadLevel1()
    {
        StartCoroutine(LoadAsyncScene());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
