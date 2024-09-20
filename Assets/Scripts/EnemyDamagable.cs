using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagable : Damagable
{
    public Enemy enemy;
    public GameObject dropItem;
    protected override bool wasHit {get{
        return enemy.wasHit;
    }set{
        enemy.wasHit = value;
    }}
    public override int HP { get{
        return _HP;
    } set{
        _HP = value;
        if(_HP <= 0){
            isAlive = false;
            animator.SetBool("isAlive", false);
            if(dropItem != null){
                Instantiate(dropItem, transform.position, transform.rotation);
            }
        }
    }}


    private void Awake(){
        animator = GetComponent<Animator>();
        sfxPlayer = GetComponent<SFXPlayer>();
    }


    public override void TakeDamage(int damage, Vector2 knockback){
        if(isAlive && !isInvincible){
            sfxPlayer.PlaySFX(1);
            HP -= damage;
            isInvincible = true; // temporary invincibility
            OnHitEvents?.Invoke(damage, knockback); // ? = check null
            wasHit = true;
            Debug.Log(gameObject.name + " received " + damage + " damage");
            // Flip enemy if being attacked from behind
            if(Math.Sign(knockback.x) == Math.Sign(gameObject.transform.localScale.x)){
                enemy.canAttack = false;
                enemy.FlipDir();
            }
        }
    }

    public void EnemyDeath(){
        Destroy(gameObject);
    }
}
