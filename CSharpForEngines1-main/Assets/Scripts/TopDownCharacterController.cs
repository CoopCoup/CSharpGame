using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class TopDownCharacterController : MonoBehaviour, I_Shot
{
    #region Framework Stuff

    //reference sprite renderer
    private SpriteRenderer spriteRen;

    //reference animator
    private Animator animator;
    
    //Reference to attached rigidbody 2D
    private Rigidbody2D rb;

    //The direction the player is moving in
    private Vector2 playerDirection;

    //The speed at which they're moving
    private float playerSpeed = 1f;

    //Health and damage
    [SerializeField] private int health = 3;
    private bool dead = false;
    private bool recovering = false;

    [Header("Movement parameters")]
    //The maximum speed the player can move
    [SerializeField] private float playerMaxSpeed = 100f;
    #endregion

    //getting the player's inputs for movements and recent inputs so they face diagonally correctly
    private float inputX;
    private float inputY;
    private float recentInputX;
    private float recentInputY;

    //Getting the player's direction for sprite orientation
    private Vector2 playerFacing = new Vector2(0, 1);
    private bool isDiagonal = false;
    private float angle;

     IEnumerator CheckDiagonal()
    {
        yield return new WaitForSeconds(0.03f);
        isDiagonal = false;
    }

    //ability setup
    private bool isGlitching = false;
    private bool canGlitch = true;

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameOver");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void LoadGameOver()
    {
        StartCoroutine(LoadAsyncScene());
    }

    //Ability coroutine
    IEnumerator GlitchCooldown()
    {
        yield return new WaitForSeconds(0.6f);
        canGlitch = true;
    }

    IEnumerator GlitchState()
    {
        isGlitching = true;
        animator.SetBool("isGlitching", true);
        yield return new WaitForSeconds(.3f);

        isGlitching = false;
        animator.SetBool("isGlitching", false);
    }
    
    
    //bullet setup
    public GameObject bulletGunPoint;
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    //firing setup
    private bool canFire = false;
    private int ammoCount = 0;

    //getting hit delay
    IEnumerator DamageRecover()
    {
        
        yield return new WaitForSeconds(.5f);
        recovering = false;
        canGlitch = true;
        if (!isGlitching)
        {
            animator.SetBool("isGlitching", false);
            spriteRen.color = Color.white;
        }
        
    }

    //firing projectile
    private void Fire()
    {
        GameObject bulletToSpawn = Instantiate(bulletPrefab, bulletGunPoint.transform.position, Quaternion.identity);


        if (bulletToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(playerFacing * bulletSpeed, ForceMode2D.Impulse);
        }

        
    }

    public void Hit(int damage)
    {
        if (!recovering)
        {
            if (!isGlitching)
            {
                health -= damage;
                Debug.Log(damage);
                recovering=true;
                canGlitch = false;
                if (health <= 0)
                {
                    dead = true;
                    Debug.Log("Dead!");
                    LoadGameOver();
                }
                spriteRen.color = Color.red;
                animator.SetBool("isGlitching", true);
                StartCoroutine(DamageRecover());
            }
            else
            {
                if (ammoCount >= 5)
                {
                    ammoCount = 5;
                }
                else
                {
                    ammoCount++;
                }
            }
        }
        

    }



    /// <summary>
    /// When the script first initialises this gets called, use this for grabbing componenets
    /// </summary>
    private void Awake()
    {
        //Get the attached components so we can use them later
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    /// <summary>
    /// Called after Awake(), and is used to initialize variables e.g. set values on the player
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc perfornamce so physics can be calculated properly
    /// </summary>
    private void FixedUpdate()
    {
        //Set the velocity to the direction they're moving in, multiplied
        //by the speed they're moving
        //making the player face the direction they move in
        

        rb.velocity = playerDirection * (playerSpeed * playerMaxSpeed) * Time.fixedDeltaTime;
        angle = Mathf.Atan2(playerFacing.x, playerFacing.y) * Mathf.Rad2Deg * -1;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    /// <summary>
    /// When the update loop is called, it runs every frame, ca run more or less frequently depending on performance. Used to catch changes in variables or input.
    /// </summary>
    private void Update()
    {
        // read input from WASD keys
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        //normalise the player direction so that diagonals move at the same speed as the regular cardinal movement 
        playerDirection = new Vector2 (inputX, inputY).normalized;
        playerDirection = Vector2.ClampMagnitude(playerDirection, 1f);

        // check if Diagonal
        if ((inputX != 0) && (inputY != 0))
        {
            isDiagonal = true;
            recentInputX = inputX;
            recentInputY = inputY;
        }
        else
        {
            if (isDiagonal)
            {
                StopCoroutine(CheckDiagonal());
                StartCoroutine(CheckDiagonal());
            }
        }

        // check if there is some movement direction, if there is something, then set animator flags and make speed = 1
        if (playerDirection.magnitude != 0)
        {
            if (isDiagonal)
            {
                playerFacing = new Vector2(recentInputX, recentInputY);
                playerSpeed = 1f;
            }
            else
            {
                playerFacing = new Vector2(inputX, inputY);
                //And set the speed to 1, so they move!
                playerSpeed = 1f;
            }
            
        }
        else
        {
            //Was the input just cancelled (released)? If so, set
            //speed to 0
            playerSpeed = 0f;

            //Update the animator too, and return
            if (isDiagonal)
            {
                playerFacing = new Vector2(recentInputX, recentInputY);
            }
        }

        // Was the fire button pressed (mapped to Left mouse button or gamepad trigger)
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoot 
            if (ammoCount > 0)
            {
                Fire();
                ammoCount--;
            }
            else
            {
                Debug.Log("Out of Ammo");
            }
        }

        //if alt-fire is pressed actiavte glitch ability
        if (Input.GetButtonDown("Fire2"))
        {
            if (canGlitch)
            {
                canGlitch = false;
                StartCoroutine(GlitchState());
                StartCoroutine(GlitchCooldown());
            }
        }
    }
}
