using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out I_Heal shotObject))
            {
                shotObject.Heal();
                Destroy(gameObject);
            }
        }
    }
}
