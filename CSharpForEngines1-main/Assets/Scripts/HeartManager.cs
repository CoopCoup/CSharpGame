using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtHeartSystem : MonoBehaviour
{
    public GameObject heartPrefab;
    public TopDownCharacterController playerScript;
    List<Heart> hearts = new List<Heart>();

    private void Start()
    {
        DrawHearts();
    }

    private void Update()
    {
        DrawHearts();
    }

    private void OnEnable()
    {
        TopDownCharacterController.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        TopDownCharacterController.OnPlayerDamaged -= DrawHearts;
    }

    public void DrawHearts()
    {
        ClearHearts();

        for(int i = 0; i < 3; i++) 
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerScript.health - i, 0, 1);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        Heart heartComponent = newHeart.GetComponent<Heart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<Heart>();
    }
}
