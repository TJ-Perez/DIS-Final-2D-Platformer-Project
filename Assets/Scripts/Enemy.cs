using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject bones;

    private GameObject player;
    private Animator playerAnim;
    private Animator anim;
    private Rigidbody2D rb;

    private Player playerScript;

    private List<Collider2D> colliders = new List<Collider2D>();

    [SerializeField]
    private float walkSpeed;
    private float playerDistance;
    [SerializeField]
    private float attackRange;

    private bool playerToRight;

    [SerializeField]
    private int health;

    private bool isAttackingTimer;

    [SerializeField] private GameControler gameControler;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerAnim = player.GetComponent<Animator>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        playerDistance = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);

        if (player.transform.position.x < gameObject.transform.position.x)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x = -Mathf.Abs(gameObject.transform.localScale.x);
            gameObject.transform.localScale = newScale;

            walkSpeed = Mathf.Abs(walkSpeed) * -1;

            playerToRight = false;
        }
        else
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x = Mathf.Abs(gameObject.transform.localScale.x);
            gameObject.transform.localScale = newScale;

            walkSpeed = Mathf.Abs(walkSpeed) * 1;

            playerToRight = true;
        }

        if (playerDistance >= attackRange && canForward() && !isAttackingTimer && !anim.GetBool("isHit"))
        {
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            anim.SetBool("canWalk", true);
            anim.SetBool("isAttacking", false);

        }
        else if ((playerDistance < attackRange) && !isAttackingTimer && !playerAnim.GetBool("isDead") && !anim.GetBool("isHit"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            StartCoroutine(Attack());

        }
        else if (!isAttackingTimer && !anim.GetBool("isHit"))
        {
            anim.SetBool("canWalk", false);
        }
    }
  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!colliders.Contains(other)) { colliders.Add(other); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        colliders.Remove(other);
    }

    private bool canForward()
    {
        Vector2 groundOrigin = transform.position;
        Vector2 enemyOrigin = transform.position;

        float enemySpacing = Mathf.Abs(1.9f * transform.localScale.y);
        float groundSpacing = Mathf.Abs(.8f * transform.localScale.y);


        if (playerToRight)
        {
            groundOrigin.x += groundSpacing;
            enemyOrigin.x += enemySpacing;

        }
        else
        {
            groundOrigin.x -= groundSpacing;
            enemyOrigin.x -= enemySpacing;
        }

        Debug.DrawRay(groundOrigin, Vector2.down, Color.green);

        if (!Physics2D.Raycast(groundOrigin, Vector2.down, 2 * transform.localScale.y, LayerMask.GetMask("Floor"))){
            return false;
        }
        if (Physics2D.Raycast(enemyOrigin, Vector2.right, .1f, LayerMask.GetMask("Enemy")))
        {
            return false;
        }
        if (Physics2D.Raycast(enemyOrigin, Vector2.left, .1f, LayerMask.GetMask("Enemy")))
        {
            return false;
        }
        
        return true;
    }

    IEnumerator Attack()
    {
        isAttackingTimer = true;

        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(.75f);

        if (!anim.GetBool("isHit"))
        {
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.tag == "Player")
                {
                    Debug.LogError("HIT BY SKELETON");
                    playerScript.hurt(playerToRight);
                }
            }
        }
        
        yield return new WaitForSeconds(.75f);
        anim.SetBool("isAttacking", false);

        isAttackingTimer = false;
    }

    public void gotHit()
    {
        StartCoroutine(gettingHit());
    }

    public IEnumerator gettingHit()
    {

        health -= 1;

        if (playerToRight)
        {
            rb.AddForce(Vector2.left * (30000 * gameObject.transform.localScale.y));
        }
        else
        {
            rb.AddForce(Vector2.right * (30000 * gameObject.transform.localScale.y));
        }

        anim.SetBool("isHit", true);
        anim.SetBool("isAttacking", false);


        if (health <= 0)
        {
            anim.SetBool("isDead", true);
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());


            yield return new WaitForSeconds(1.25f);
            gameControler.addPoint();
            GameObject tmp = Instantiate(bones, gameObject.transform.position, gameObject.transform.rotation);

            if (!playerToRight)
            {
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = -Mathf.Abs(gameObject.transform.localScale.x);
                tmp.transform.localScale = newScale;

            }
            DestroyImmediate(gameObject);
            yield break;
        }

        yield return new WaitForSeconds(.75f);
        anim.SetBool("isHit", false);

    }
}
