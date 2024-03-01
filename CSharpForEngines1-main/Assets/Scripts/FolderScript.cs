using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FolderScript : MonoBehaviour
{
    public GameObject teleportLocation;
    private SpriteRenderer spriteRenderer;
    public Sprite locked, open;
    public bool levelEnd;
    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (levelEnd == true)
        {
            spriteRenderer.sprite = locked;
        }
        else
        {
            spriteRenderer.sprite = open;
        }
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    private void LoadNextLevel()
    {
        StartCoroutine(LoadAsyncScene());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!levelEnd)
            {
                collision.transform.position = teleportLocation.transform.position;
            }
            else
            {
                LoadNextLevel();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
