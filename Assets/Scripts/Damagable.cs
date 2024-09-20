using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// abstract class which will be inherited by PlayerDamagable and EnemyDamagable
public abstract class Damagable : MonoBehaviour
{
    protected Animator animator;
    public UnityEvent<int, Vector2> OnHitEvents;    // events to invoke when received damage
    //public UnityEvent<float, float> HealthChanged; // for UI
    protected abstract bool wasHit {get; set;}
    [SerializeField] private bool _isAlive = true;
    public bool isAlive {get{
        return _isAlive;
    }protected set{
        _isAlive = value;
    }}
    [SerializeField] protected int _maxHP = 100;
    public int maxHP {get{
        return _maxHP;
    }protected set{
        _maxHP = value;
    }}
    [SerializeField] protected int _HP = 100;  //current HP
    public abstract int HP {get; set;}
    [SerializeField] protected bool isInvincible = false;
    public float invTime = 0.5f; // invincibility time
    private float invTimer = 0f; // timer
    protected SFXPlayer sfxPlayer;


    private void Awake(){
        animator = GetComponent<Animator>();
        sfxPlayer = GetComponent<SFXPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInvincible){
            if(invTimer >= invTime){
                isInvincible = false; // reset invincibility
                invTimer = 0;
                if(wasHit){
                    wasHit = false;
                }
            }
            else{
                invTimer += Time.deltaTime; // add time to timer
            }
        }
        //Hit(10);
    }

    // take damage
    public virtual void TakeDamage(int damage, Vector2 knockback){
        if(isAlive && !isInvincible){
            HP -= damage;
            isInvincible = true; // temporary invincibility
            OnHitEvents?.Invoke(damage, knockback); // ? = check null
            wasHit = true;
            Debug.Log(gameObject.name + " received " + damage + " damage");
        }
    }
}
