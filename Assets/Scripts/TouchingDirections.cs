using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D col;
    Animator animator;
    public ContactFilter2D castFilter;
    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    public float groundDistance = 0.05f;
    [SerializeField] private bool _isGrounded = true; // whether the character is on ground or not
    public bool isGrounded {get{
        return _isGrounded;
    }private set{
        _isGrounded = value;
        animator.SetBool("isGrounded", value);
    }}
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    public float ceilingDistance = 0.05f;
    [SerializeField] private bool _isOnCeiling = false; // whether the character is touching the ceiling or not
    public bool isOnCeiling {get{
        return _isOnCeiling;
    }private set{
        _isOnCeiling = value;
        animator.SetBool("isOnCeiling", value);
    }}

    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    public float wallDistance = 0.2f;
    [SerializeField] private bool _isOnWall = false; // whether the character is touching the wall or not
    public bool isOnWall {get{
        return _isOnWall;
    }private set{
        _isOnWall = value;
        animator.SetBool("isOnWall", value);
    }}
    // returns left is local scale is -1, right if 1
    private Vector2 wallCheckDir => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    void Awake(){
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // returns true if touching ground
        isGrounded = col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        isOnCeiling = col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        isOnWall = col.Cast(wallCheckDir, castFilter, wallHits, wallDistance) > 0;
    }
}
