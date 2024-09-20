using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
//using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public UnityEvent<float,float> TimerChanged;
    public float moveSpeedLiquid = 5f;
    public float moveSpeedSolid = 15f;
    public float moveSpeedGas = 3f;
    public float flySpeedGas = 3.5f;
    public int selfDamage = 2;  // HP reduced when attacking
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    Vector2 moveInput; // player movement vector
    PlayerDamagable damagable;

    public bool wasHit = false;

    public bool isAlive {get{return animator.GetBool("isAlive");}}
    public bool isAttacking {get{return animator.GetBool("isAttacking");}}
    
    private int _currentState = 1; // 1 = Liquid, 2 = Solid, 3 = Gas
    public int currentState {get{
        return _currentState;
    }private set{
        _currentState = value;
        animator.SetInteger("currentState", value);
    }}

    public float stateTime = 10f;   // timer for state changes
    private float _stateTimer = 10f;
    public float stateTimer {get{
        return _stateTimer;
    }private set{
        _stateTimer = value;
        TimerChanged?.Invoke(_stateTimer, stateTime);
    }}
    public float rechargeRate = 1.5f;

    public float rechargeDelay = 0.2f;  // exploit fix
    private float rechargeTimer = 0f;
    public float flyDelay = 0.2f;   // exploit fix
    private float flyTimer = 0f;

    private bool _isMoving = false;
    public bool IsMoving {get{
        return _isMoving;
    }private set{
        _isMoving = value;
        animator.SetBool("isMoving", value);
    }}

    public bool canMove {get{return animator.GetBool("canMove");}}

    // for flipping the sprite according to player movement
    private bool _isFacingRight = true;
    public bool isFacingRight {get{
        return _isFacingRight;
    } private set{
        if(value != _isFacingRight){
            transform.localScale *= new Vector2(-1,1);  // flip local scale
        }
        _isFacingRight = value;
    }}

    public GameObject icicle;
    public GameObject shootPosition;
    SFXPlayer sfxPlayer;
    UIScreenFade screenFade;



    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damagable = GetComponent<PlayerDamagable>();
        sfxPlayer = GetComponent<SFXPlayer>();
        screenFade = GameObject.FindGameObjectWithTag("ScreenFade").GetComponent<UIScreenFade>();
        currentState = 1;
    }
    
    void Start(){
        gameObject.SetActive(false);
    }

    private void FixedUpdate(){
        // STATE TIMER
        if(isAlive){
            if(currentState > 1){
                if(stateTimer <= 0){
                    // revert back to liquid state
                    ChangeState1();
                    stateTimer = 0;
                    //Debug.Log("Energy depleted.");
                }
                else{
                    // count down timer
                    stateTimer -= Time.deltaTime;
                    //Debug.Log("Energy Remaining: " + Mathf.RoundToInt(stateTimer));
                }
            }
            else if(currentState < 2 && stateTimer < stateTime){
                // Delay recharge
                if(rechargeTimer < rechargeDelay){
                    rechargeTimer += Time.deltaTime;
                    //Debug.Log("Delay " + Time.deltaTime);
                }
                else{
                    // recharge timer
                    stateTimer = Mathf.Min(stateTimer + rechargeRate * Time.deltaTime, 10);
                    //Debug.Log("charging");
                }
                //Debug.Log("Energy recharging: " + Mathf.RoundToInt(stateTimer));
            }
        }

        // MOVEMENT
        if(!wasHit){
            if(!touchingDirections.isOnWall && currentState < 3 && canMove && isAlive){
                switch(_currentState){
                case 1:
                    rb.velocity = new Vector2(moveInput.x * moveSpeedLiquid, rb.velocity.y);
                    break;
                case 2:
                    if(IsMoving){
                        rb.velocity = new Vector2(Mathf.Min(Mathf.Max(moveInput.x + rb.velocity.x, -moveSpeedSolid), moveSpeedSolid), rb.velocity.y);
                    }
                    break;

                }
            }
            else if(currentState == 3){
                if(isAlive){
                    if(flyTimer > flyDelay){
                        if(canMove && !touchingDirections.isOnWall){
                        rb.velocity = new Vector2(moveInput.x * moveSpeedGas, flySpeedGas);
                        }
                        else{
                            rb.velocity = new Vector2(0, flySpeedGas);
                        }
                    }
                    else{
                        flyTimer += Time.deltaTime;
                    }
                }
                else{
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }


    // OnMove event in the player object
    public void OnMove(InputAction.CallbackContext context){
        if(isAlive){
            moveInput = context.ReadValue<Vector2>(); // get movement vector from input
            IsMoving = moveInput != Vector2.zero; // moving or not
            SetFacingDirection();
        }
        else{
            IsMoving = false;
        }
    }

    // OnAttack event in player object
    public void OnAttack(InputAction.CallbackContext context){
        if(context.started && canMove && isAlive && !isAttacking){
            if(currentState == 1 || currentState == 2){
                animator.SetBool("isAttacking", true);
                damagable.SelfDamage(selfDamage);
            }
        }
    }

    public void ShootIcicle(){
        GameObject p = Instantiate(icicle, shootPosition.transform.position, transform.rotation);
        p.transform.localScale = transform.localScale;
    }

    public void RestoreTime(float timeRestored){
        if(stateTimer < stateTime){
            stateTimer = Mathf.Min(stateTimer + timeRestored, stateTime);
            Debug.Log("Restored " + timeRestored + " EP");
        }
    }

    // change to liquid state
    public void OnState1(InputAction.CallbackContext context){
        if(context.started){
            ChangeState1();
        }
    }

    private void ChangeState1(){
        if(_currentState != 1 && isAlive && !isAttacking){
            currentState = 1;
            rb.gravityScale = 2; //2
            this.gameObject.layer = LayerMask.NameToLayer("Liquid");
            rechargeTimer = 0;
            flyTimer = 0;
            sfxPlayer.PlaySFX(0);
        }
    }
    // change to solid state
    public void OnState2(InputAction.CallbackContext context){
        if(context.started && _currentState != 2 && isAlive && !isAttacking && stateTimer >= 1){
            currentState = 2;
            rb.gravityScale = 3; //3
            this.gameObject.layer = LayerMask.NameToLayer("Solid");
            flyTimer = 0;
            sfxPlayer.PlaySFX(2);
        }
    }
     // change to gas state
    public void OnState3(InputAction.CallbackContext context){
        if(context.started && currentState != 3 && isAlive && !isAttacking && stateTimer >= 1){
            currentState = 3;
            rb.gravityScale = 0f;
            this.gameObject.layer = LayerMask.NameToLayer("Gas");
            sfxPlayer.PlaySFX(4);
        }
    }

    public void OnHit(int damage, Vector2 knockback){
        rb.velocity = knockback;
    }


    // temporary method for restarting game
    public void RestartTemp(InputAction.CallbackContext context){
        if(context.started){
            screenFade.FadeToScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    // flip sprite according to facing direction
    public void SetFacingDirection()
    {
        if(canMove && isAlive){
            if(moveInput.x > 0 && !isFacingRight){
            isFacingRight = true;
            }
            else if(moveInput.x < 0 && isFacingRight){
                isFacingRight = false;
            }
        }
    }
}
