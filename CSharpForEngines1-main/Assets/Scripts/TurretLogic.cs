using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class TurretLogic : MonoBehaviour, I_Shot
{

    //bullet setup
    [SerializeField] private GameObject bulletGunPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    public static event Action OnEnemyDamaged;
    private bool canFire = true;
    private bool newPlayerInSight = false;
    private GameObject newPlayerRef;
    public float rotationModifier;
    public float rotateSpeed;

    //get animator
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public int damage = 1;

    private Vector2 direction;
    [SerializeField] private float firingDelay;
    IEnumerator TurretDeath()
    {
        canFire = false;
        anim.SetBool("IsGlitching", true);
        yield return new WaitForSeconds(.2f);
        OnEnemyDamaged?.Invoke();
        Destroy(gameObject);
    }

    IEnumerator FacingPlayer()
    {
        if (newPlayerInSight == true)
        {
            Vector3 vectorToTarget = newPlayerRef.transform.position - transform.position;
            float angle = MathF.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

            direction = (new Vector2(newPlayerRef.transform.position.x, newPlayerRef.transform.position.y) - new Vector2(bulletGunPoint.transform.position.x, bulletGunPoint.transform.position.y));
            direction = direction.normalized;
            yield return new WaitForSeconds(.01f);
            StartCoroutine(FacingPlayer());
            
        }
        else
        {
            StopCoroutine(FacingPlayer());
        }
        
    }

    public void FacePlayer(bool playerInSight, GameObject playerRef)
    {
        newPlayerInSight = true;
        newPlayerRef = playerRef;
        StartCoroutine(FacingPlayer());
    }

    public void PlayerGone()
    {
        newPlayerInSight = false;
        StopCoroutine(FacingPlayer());
        
    }


    public void Hit(int damage)
    {
        StartCoroutine(TurretDeath());
    }


    //firing projectile
    private void Fire()
    {
        if (canFire == true)
        {
            
            GameObject bulletToSpawn = Instantiate(bulletPrefab, bulletGunPoint.transform.position, Quaternion.identity);


            if (bulletToSpawn.GetComponent<Rigidbody2D>() != null)
            {
                bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
            }

            StartCoroutine(FireDelay());
        }
        

    }


    IEnumerator FireDelay()
    {
        
        yield return new WaitForSeconds(firingDelay);
        Fire();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = transform.right;
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
