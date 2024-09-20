using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerDamagable : Damagable
{
    PlayerController controller;
    public UnityEvent<float, float> HealthChanged; // for UI
    protected override bool wasHit {get{
        return controller.wasHit;
    }set{
        controller.wasHit = value;
    }}
    public float currentSize = 1f;
    public override int HP {get{
        return _HP;
    } set{
        _HP = value;
        HealthChanged?.Invoke(_HP, _maxHP);  // UI update
        setSize();
        if(_HP <= 0){
            isAlive = false;
            animator.SetBool("isAlive", false);
            Debug.Log("dead");
            musicPlayer.mute = true;
            sfxPlayer.PlaySFX(6);
            // death animation
            // destroy game object when the enemy is defeated (will be updated)
            // temporary way to restart level
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }}
    AudioSource musicPlayer;
    UIScreenFade screenFade;


    private void Awake(){
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        sfxPlayer = GetComponent<SFXPlayer>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<AudioSource>();
        screenFade = GameObject.FindGameObjectWithTag("ScreenFade").GetComponent<UIScreenFade>();
    }


    public override void TakeDamage(int damage, Vector2 knockback){
        if(isAlive && !isInvincible){
            sfxPlayer.PlaySFX(5);
            // take half damage when ice state
            if(controller.currentState == 2){
                HP = Mathf.Max(HP - (damage/2), 0);
                Debug.Log(gameObject.name + " received " + damage/2 + " damage");
            }
            else{
                HP = Mathf.Max(HP - damage, 0);
                Debug.Log(gameObject.name + " received " + damage + " damage");
            }
            isInvincible = true; // temporary invincibility
            OnHitEvents?.Invoke(damage, knockback); // ? = check null
            wasHit = true;
        }
    }

    public void SelfDamage(int damage){
        if(isAlive){
            HP = Mathf.Max(HP - damage, 0);
            //Debug.Log("Reduced " + damage + " HP from attacking");
        }
    }

    public void Heal(int restoredHP){
        if(isAlive){
            HP = Mathf.Min(HP + restoredHP, _maxHP);
            Debug.Log("Restored " + restoredHP + " HP");
        }
    }


    // change player size according to HP
    private void setSize(){
        float newSize = 1f;
        if(_HP > 70){
            newSize = 1f;
        }else if(_HP >= 40 && _HP <= 70){
            newSize = 0.9f;
        }else if(_HP < 40){
            newSize = 0.8f;
        }
        if(newSize != currentSize){
            if(controller.isFacingRight){
                transform.localScale = new Vector3(newSize, newSize, 0);
            }
            else{
                transform.localScale = new Vector3(-newSize, newSize, 0);
            }
            currentSize = newSize;
        }
    }

    public void playerDeath(){
        screenFade.FadeToScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
