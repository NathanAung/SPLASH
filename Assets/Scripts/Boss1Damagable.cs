using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Boss1Damagable : EnemyDamagable
{
    AudioSource audioSource;
    MusicPlayer musicPlayer;
    UIScreenFade screenFade;
    public Animator doorAnim;
    public override int HP { get{
        return _HP;
    } set{
        _HP = value;
        if(_HP <= 0){
            isAlive = false;
            animator.SetBool("isAlive", false);
            musicPlayer.ChangeTrack(2);
            // destroy summoned enemies
            List<EnemyDamagable> summons = gameObject.GetComponent<EnemyBoss>().summonedEnemies;
            for(int i = 0; i < summons.Count; i++){
                if(summons[i] != null){
                    summons[i].HP = 0;
                }
            }
            doorAnim.ResetTrigger("close");
            doorAnim.SetTrigger("pressed");
            screenFade.PlayOutro();
        }
    }}


    private void Awake(){
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sfxPlayer = GetComponent<SFXPlayer>();
        Animator doorAnim = GetComponent<Animator>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        screenFade = GameObject.FindGameObjectWithTag("ScreenFade").GetComponent<UIScreenFade>();
    }


    public override void TakeDamage(int damage, Vector2 knockback){
        if(isAlive && !isInvincible){
            if(!audioSource.isPlaying){
                sfxPlayer.PlaySFX(2);
            }
            HP -= damage;
            isInvincible = true; // temporary invincibility
            OnHitEvents?.Invoke(damage,knockback); // ? = check null
            wasHit = true;
            Debug.Log(gameObject.name + " received " + damage + " damage");
            // Flip enemy if being attacked from behind
            // if(Math.Sign(knockback.x) == Math.Sign(gameObject.transform.localScale.x)){
            //     enemy.canAttack = false;
            //     enemy.FlipDir();
            // }
        }
    }
}
