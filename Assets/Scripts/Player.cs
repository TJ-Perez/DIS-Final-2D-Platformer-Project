using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    //Game Object Components
    private Rigidbody2D rb;
    private Animator m_Animator;
    //private CircleCollider2D p_Colider;
    public Transform AttackPoint;

    //Player Stats
    public int lives;
    public int stamina;
    public float speed;
    public float jumpForce;
    public float rollforce;
    public float attackRange;
    private string direction = "right";
    private float rolltimer = 0f;
    private float attacktimer = 0f;
    public float knockBack;
    private bool waitingForJump;
    

    //Hitboxes


    //booleans
    private bool hasDoubleJump;
    private bool isGrounded = true;
    private bool isRolling;
    private bool isAttacking;
    private bool enabled = true;
    private bool groundCheck1;
    private bool groundCheck2;
    private bool groundCheck3;

    public AudioClip jumpNoise;
    public AudioClip walkNoise;
    public AudioClip rollNoise;
    public AudioClip hurtNoise;
    public AudioClip dieNoise;
    public AudioClip hitNoise;

    [SerializeField] AudioSource soundEffectPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    public void hurt(bool toLeft)
    {
        lives -= 1;
        rb.velocity = new Vector2(0f, 0f);
        if (toLeft)
        {
            rb.AddForce(Vector2.right * 250);
        }
        else
        {
            rb.AddForce(Vector2.left * 250);
        }


        StartCoroutine(gettingHit());
    }

    IEnumerator gettingHit()
    {
        m_Animator.SetBool("tookDmg", true);
        enabled = false;

        yield return new WaitForSeconds(.5f);

        enabled = true;
        m_Animator.SetBool("tookDmg", false);

    }



void Update()
    {

        //Checks to see if player is grounded 
        check_grounded();


        if (!isRolling && !isAttacking  && enabled)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        //death Mechanic
        if (lives <= 0)
        {
            rb.velocity = new Vector2(0f, 0f);
            enabled = false;
            m_Animator.SetBool("isDead", true);
            soundEffectPlayer.PlayOneShot(dieNoise);
        }


        if (enabled == true)
        {

            //right and left movement on ground
            if (Input.GetKey(KeyCode.A) && !isRolling && !isAttacking)
            {
                //makes sure that the character is facing the right way
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = -1;
                gameObject.transform.localScale = newScale;

                m_Animator.SetBool("isWalking", true);
                direction = "left";
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.D) && !isRolling && !isAttacking)
            {

                //makes sure that the character is facing the right way
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = 1;
                gameObject.transform.localScale = newScale;


                m_Animator.SetBool("isWalking", true);
                direction = "right";
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                m_Animator.SetBool("isWalking", false);
            }


            //roll timer should probably use coroutines
            rolltimer -= Time.deltaTime;

            //Roll mechanics

            if (rolltimer <= 0)
            {
                //Activates Enemy collisions
                Physics2D.IgnoreLayerCollision(7, 6, false);
                isRolling = false;
                m_Animator.SetBool("isRolling", false);
            }

            if (Input.GetKey(KeyCode.LeftShift) && !isRolling && !isAttacking && stamina > 0)
            {
                stamina -= 1;

                StartCoroutine(RollRestore());

                Physics2D.IgnoreLayerCollision(7, 6, true);
                rollforce = 300;

                if (Mathf.Abs(rb.velocity.x) > 3)
                {
                    rollforce = rollforce / 4;
                }

                m_Animator.SetBool("isRolling", true);
                isRolling = true;

                if (direction == "right")
                {
                    rb.AddForce(Vector2.right * rollforce);
                }
                else
                {
                    rb.AddForce(Vector2.left * rollforce);
                }
                rolltimer = 1f;
            }



            // Jumping mechanics

            if (!isGrounded)
            {
                m_Animator.SetBool("isGrounded", false);
            }


            if (isGrounded && rb.velocity.y == 0 && waitingForJump == false)
            {
                m_Animator.SetBool("isGrounded", true);
                m_Animator.SetBool("isDoubleJumping", false);
                m_Animator.SetBool("isJumping", false);
                hasDoubleJump = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isRolling && !isAttacking)
            {
                StartCoroutine(Jumptimer()); 
                m_Animator.SetBool("isJumping", true);
                rb.AddForce(Vector2.up * jumpForce);
                hasDoubleJump = true;

            }


            if (Input.GetKeyDown(KeyCode.Space) && hasDoubleJump && !isGrounded && !isRolling && !isAttacking)
            {
                m_Animator.SetBool("isJumping", false);
                m_Animator.SetBool("isDoubleJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                hasDoubleJump = false;
                
            }


            //should be using coroutines
            attacktimer -= Time.deltaTime;

            if (attacktimer <= 0)
            {
                isAttacking = false;
                m_Animator.SetBool("isAttacking", false);
            }


            //Attacking
            if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
            {
                StartCoroutine(Attack());
            }

            //falling
            if (rb.velocity.y < 0 && !isGrounded)
            {
                m_Animator.SetBool("isJumping", true);
            }

            if (Input.GetKeyDown(KeyCode.Space)) 
            {

                soundEffectPlayer.PlayOneShot(jumpNoise);
            }

            if (Input.GetKey(KeyCode.LeftShift) && !isRolling)
            {

                soundEffectPlayer.PlayOneShot(rollNoise);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {

                soundEffectPlayer.PlayOneShot(hitNoise);
            }

        }

    }


    private void OnDrawGizmosSelected()
    {
            Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
            Debug.DrawRay(transform.position + new Vector3(.2f, 0, 0), Vector2.down, Color.green);
            Debug.DrawRay(transform.position + new Vector3(-.2f, 0, 0), Vector2.down, Color.green);
            Debug.DrawRay(transform.position + new Vector3(0, 0, 0), Vector2.down, Color.green);
    
    
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        hasDoubleJump = false;
        attacktimer = 0.6f;
        m_Animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(.285f); 

        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange);
        foreach (Collider2D enemies in hitEnemys)
        {
            if (enemies.gameObject.tag == "Enemy")
            {
                Debug.LogError("Player hit skelenem(ies)");

                enemies.gameObject.GetComponent<Enemy>().gotHit();
            }
        }

        yield return new WaitForSeconds(.285f);

    }

    IEnumerator Jumptimer()
    {
        waitingForJump = true;
        yield return new WaitForSeconds(.2f);
        waitingForJump = false;

    }

    IEnumerator RollRestore()
    {
        yield return new WaitForSeconds(10f);
        stamina += 1;
    }

    private bool check_grounded()
    {
        groundCheck1 = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Floor"));
        groundCheck2 = Physics2D.Raycast(transform.position + new Vector3(.2f, 0, 0), Vector2.down, .90f, LayerMask.GetMask("Floor"));
        groundCheck3 = Physics2D.Raycast(transform.position + new Vector3(-.2f, 0, 0), Vector2.down, .90f, LayerMask.GetMask("Floor"));


        return isGrounded = groundCheck1 || groundCheck2 || groundCheck3;
    }



}