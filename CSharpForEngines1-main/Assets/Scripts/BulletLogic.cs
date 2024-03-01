using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour, I_Shot
{
    public int damage = 1;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out I_Shot shotObject))
            {
                shotObject.Hit(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out I_Shot shotObject))
            {
                shotObject.Hit(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        
    }
    
    public void Hit(int damage)
    {
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
