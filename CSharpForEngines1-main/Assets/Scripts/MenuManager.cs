using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsPanel;
    bool isControlsPanelOpen = false;

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
    public void OpenAndCloseControlsPanel() 
    {
        isControlsPanelOpen = !isControlsPanelOpen;
        controlsPanel.SetActive(isControlsPanelOpen);
    }
    public void Quit()
    {
          Application.Quit();
    }
}
