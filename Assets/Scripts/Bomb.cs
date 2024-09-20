using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explodeTime = 0f;
    private float explodeTimer = 0f;
    private bool exploded = false;
    protected bool hitGround {get{
        return animator.GetBool("hitGround");
    } private set{
        animator.SetBool("hitGround", value);
    }}
    Rigidbody2D rb;
    Animator animator;
    public Collider2D explosionCol;
    public AudioClip explodeSFX;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    private void Update()
    {
        if(hitGround){
            if(explodeTimer < explodeTime){
                explodeTimer += Time.deltaTime;
            }
            else if(!exploded){
                explosionCol.enabled = true;
                exploded = true;
                animator.SetTrigger("explode");
                AudioSource.PlayClipAtPoint(explodeSFX, transform.position + new Vector3(0,0,-10), 0.5f);
            }
        }
    }


    void OnCollisionEnter2D(Collision2D col){
        if(!hitGround){
            hitGround = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }


    public void DestroyProjectile(){
        Destroy(gameObject);
    }
}
