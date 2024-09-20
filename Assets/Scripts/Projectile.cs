using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 knockback = Vector2.zero;
    public Vector2 moveDir = Vector2.right;
    public float moveSpeed = 5f;
    private Vector2 originalPos;
    public float maxDistance = 10f;
    protected bool wasHit = false;
    Rigidbody2D rb;
    protected Animator animator;
    CircleCollider2D col;
    public AudioClip hitSFX;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        moveDir = transform.localScale;
        originalPos = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if(!wasHit && Vector2.Distance(transform.position, originalPos) < maxDistance){
            rb.velocity = new Vector2(moveSpeed * moveDir.x, rb.velocity.y);
        }
        else{
            col.enabled = false;
            animator.SetTrigger("fade");
            wasHit = true;
            rb.velocity = Vector2.zero;
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision){
        // get the collided target
        Damagable target = collision.GetComponent<Damagable>();
        // deal damage
        if(target != null){
            // flip knockback direction
            Vector2 knockbackDir = transform.localScale.x > 0 ? knockback : new Vector2(knockback.x * -1, knockback.y);
            target.TakeDamage(damage, knockbackDir);
            //Debug.Log("Dealt " + damage + " to " + collision.name);
        }
        animator.SetTrigger("wasHit");
        wasHit = true;
        AudioSource.PlayClipAtPoint(hitSFX, transform.position + new Vector3(0,0,-10), 0.5f);
    }

    public void DestroyProjectile(){
        Destroy(gameObject);
    }
}
