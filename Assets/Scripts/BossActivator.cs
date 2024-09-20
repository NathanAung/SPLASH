using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    GameObject boss;
    MusicPlayer musicPlayer;
    public Animator doorAnim;

    private void Awake(){
        boss = GameObject.FindGameObjectWithTag("Boss");
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        Animator doorAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        boss.GetComponent<EnemyBoss>().enabled = true;
        musicPlayer.ChangeTrack(1);
        doorAnim.ResetTrigger("pressed");
        doorAnim.SetTrigger("close");
        Destroy(gameObject);
    }
}
