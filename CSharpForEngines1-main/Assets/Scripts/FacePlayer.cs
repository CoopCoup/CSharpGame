using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FacePlayer : MonoBehaviour
{
    public TurretLogic ownerRef;
    public bool playerInSight;
    public GameObject playerRef;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSight = true;
            playerRef = collision.gameObject;
            ownerRef.FacePlayer(playerInSight, playerRef);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSight = false;
            ownerRef.PlayerGone();
        }
            
    }

}
