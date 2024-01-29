using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour, I_Shot
{

    //bullet setup
    public GameObject bulletGunPoint;
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    private Vector2 direction;

    public void Hit(int damage)
    {
        Destroy(gameObject);
    }

    //firing projectile
    private void Fire()
    {
        GameObject bulletToSpawn = Instantiate(bulletPrefab, bulletGunPoint.transform.position, Quaternion.identity);


        if (bulletToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
        }

        StartCoroutine(FireDelay());

    }

    bool canFire = true;

    IEnumerator FireDelay()
    {
        
        yield return new WaitForSeconds(1f);
        Fire();
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.up;
        Debug.Log(direction);
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
