using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    public float moveSpeedD = 3f;
    // dash attack
    public float dashSpeed = 50f;
    public bool dashing {get{
        return animator.GetBool("dashing");
    } set{
        animator.SetBool("dashing", value);
    }}
    public bool dashDelayAni {get{
        return animator.GetBool("dashDelay");
    } set{
        animator.SetBool("dashDelay", value);
    }}
    public float dashDelay = 3f;
    private float dashDelayTimer = 0f;
    public BoxCollider2D hitbox;
    // jump attack
    public float jumpSpeed = 20f;
    public float jumpHeight = 7f;
    public bool jumping {get{
        return animator.GetBool("jumping");
    } set{
        animator.SetBool("jumping", value);
    }}
    private bool falling = false;
    public BoxCollider2D jumpCol;
    private Vector2 jumpPos;
    // for spawning minion
    public GameObject meleeEnemy;
    public GameObject spawnPos;
    public GameObject player;
    public DetectionZone shootZone;
    public GameObject projectile;
    private bool _hasTargetR = false;
    public bool hasTargetR {get{
        return _hasTargetR;
    }private set{
        _hasTargetR = value;
        animator.SetBool("hasTargetR", value);
    }}
    public bool canSpecial {get{
        return animator.GetBool("canSpecial");
    } set{
        animator.SetBool("canSpecial", value);
    }}
    public float specialCD = 15f;
    private float specialTimer = 0f;
    SFXPlayer sfxPlayer;

    public List<EnemyDamagable> summonedEnemies;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damagable = GetComponent<Boss1Damagable>();
        player = GameObject.FindGameObjectWithTag("Player");
        sfxPlayer = GetComponent<SFXPlayer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = moveSpeedD;
        this.enabled = false;
    }


    void Update()
    {
        hasTarget = attackZone.detectedCol.Count > 0;
        hasTargetR = shootZone.detectedCol.Count > 0;

        if(dashing){
            // dash after delay
            if(dashDelayTimer >= dashDelay){
                dashDelayAni = false;
                moveSpeed = dashSpeed;
                canMove = true;
                dashDelayTimer = 0;
                sfxPlayer.PlaySFX(3);
                //Debug.Log(dashDelayTimer);
            }
            else if(!canMove){
                dashDelayTimer += Time.deltaTime;
            }
        }
        else if(canAttack && !dashing && !jumping){
            if(hasTargetR && Random.Range(0, 10) == 0){
                animator.SetTrigger("rangedAttack");
            }
            else if(hasTarget){
                animator.SetTrigger("meleeAttack");
            }
        }
        if(!canSpecial){
            if(specialTimer >= specialCD){
                canSpecial = true;
                specialTimer = 0;
            }
            else{
                specialTimer += Time.deltaTime;
            }
        }

        if(!canAttack){
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
                Debug.Log(dashDelayTimer);
                //CANCEL SPECIAL
                if(dashing && dashDelayTimer == 0){
                    moveSpeed = moveSpeedD;
                    //hitbox.enabled = false;
                    dashing = false;
                    Debug.Log("NOT DASHING");
                }
                //SPECIAL ATTACKS
                else if(canSpecial){
                    canAttack = false;
                    if(Random.Range(0, 5) == 0){
                        animator.SetTrigger("summon");
                    }
                    else if(Random.Range(0, 2) == 0){
                        DashWait();
                    }
                    else{
                        JumpAttack();
                    }
                    //animator.SetTrigger("summon");
                }
            }
            // JUMP ATTACK
            if(jumping){
                if(falling && touchingDirections.isGrounded){
                    jumpCol.enabled = false;
                    rb.gravityScale = 2f;
                    canMove = true;
                    falling = false;
                    jumping = false;
                    Debug.Log("fell");
                }
                else if(falling){
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else if(Vector2.Distance(transform.position, jumpPos) <= 1){
                    Debug.Log("reached");
                    jumpCol.enabled = true;
                    rb.gravityScale = 5f;
                    falling = true;
                    sfxPlayer.PlaySFX(5);

                }
                else{
                    transform.position = Vector2.MoveTowards(transform.position, jumpPos, jumpSpeed * Time.deltaTime);
                }
            }
            // MOVE
            else if(!wasHit && !touchingDirections.isOnWall){
                if(canMove){
                    rb.velocity = new Vector2(moveSpeed * walkDirVector.x, rb.velocity.y);
                }
                else{
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }

    private void LateUpdate(){
        if(dashing && canMove && !hitbox.enabled){
            hitbox.enabled = true;
        }
    }


    public void ShootProjectile(){
        GameObject p = Instantiate(projectile, spawnPos.transform.position, transform.rotation);
        //p.transform.localScale = transform.localScale;
        if(transform.localScale.x < 0){
            p.transform.localScale = new Vector3(-p.transform.localScale.x, p.transform.localScale.x, transform.localScale.z);
        }
    }


    public void DashWait(){
        Debug.Log("DASH");
        dashDelayAni = true;
        dashing = true;
        canMove = false;
        sfxPlayer.PlaySFX(7);
    }


    public void JumpAttack(){
        Debug.Log("JUMP");
        jumping = true;
        rb.gravityScale = 0;
        jumpPos = new Vector3(player.transform.position.x, transform.position.y + jumpHeight, 0.005f);
        canMove = false;
        sfxPlayer.PlaySFX(4);
    }


    public void Summon(){
        Debug.Log("SUMMON");
        GameObject e = Instantiate(meleeEnemy, spawnPos.transform.position + new Vector3(0, 0, 0.003f), transform.rotation);
        summonedEnemies.Add(e.GetComponent<EnemyDamagable>());
        if(_walkDir == WalkableDir.Left){
            e.GetComponent<Enemy>().FlipDir();
        }
        sfxPlayer.PlaySFX(6);
    }


    public override void OnHit(int damage, Vector2 knockback){
        if(!dashing && !jumping){
            rb.velocity = knockback;
            Debug.Log("KNOCKBACK");
        }
        else{
            Debug.Log("NO KNOCKBACK");
        }

    }
}
