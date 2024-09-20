using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
//using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected TouchingDirections touchingDirections;
    protected EnemyDamagable damagable;
    public bool wasHit = false;
    protected Vector2 walkDirVector = Vector2.right;
    public enum WalkableDir {Left, Right}; // for walking direction
    protected WalkableDir _walkDir = WalkableDir.Right;
    public WalkableDir walkDir{get{
        return _walkDir;
    }protected set{
        if(_walkDir != value){
            // flip sprite
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            // set walking direction vector
            if(value == WalkableDir.Left){
                walkDirVector = Vector2.left;
            }else if(value == WalkableDir.Right){
                walkDirVector = Vector2.right;
            }
            _walkDir = value;
        }
    }}
    public bool canMove {get{
        return animator.GetBool("canMove");
    } protected set{
        animator.SetBool("canMove", value);
    }}
    public DetectionZone cliffDetection; // for detecting cliffs for flipping direction
    public DetectionZone attackZone; // collider that detects player for attacking
    private bool _hasTarget = false;
    public bool hasTarget {get{
        return _hasTarget;
    }protected set{
        _hasTarget = value;
        animator.SetBool("hasTarget", value);
    }}

    public float attackCD = 1f; // attack cooldown
    protected float attackTimer = 0f;
    public bool canAttack {get{
        return animator.GetBool("canAttack");
    } set{
        animator.SetBool("canAttack", value);
    }}


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damagable = GetComponent<EnemyDamagable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hasTarget = attackZone.detectedCol.Count > 0;
        if(!canAttack && damagable.isAlive){
            if(attackTimer >= attackCD){
                canAttack = true;
                attackTimer = 0;
            }
            else{
                attackTimer += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate(){
        if(damagable.isAlive){
            if(touchingDirections.isGrounded && touchingDirections.isOnWall){
                FlipDir();
                //Debug.Log("Flipped");
            }
            if(!wasHit && !touchingDirections.isOnWall){
                if(canMove && !hasTarget){
                    rb.velocity = new Vector2(moveSpeed * walkDirVector.x, rb.velocity.y);
                }
                else{
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }

    public void FlipDir()
    {
       if(walkDir == WalkableDir.Left){
            walkDir = WalkableDir.Right;
       }
       else if(walkDir == WalkableDir.Right){
            walkDir = WalkableDir.Left;
       }
       else{
            Debug.LogError("Cannot find walkable dir");
       }
    }

    public virtual void OnHit(int damage, Vector2 knockback){
        rb.velocity = knockback;
    }

    // Flip direction when a cliff is detected
    public void OnCliffDetected(){
        if(touchingDirections.isGrounded){
            FlipDir();
        }
    }
}
