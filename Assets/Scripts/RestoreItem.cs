using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreItem : MonoBehaviour
{
    Animator animator;
    public bool isHP = true;
    public int HPRestore = 15;
    public float EPRestore = 3;
    public bool collected {get{
        return animator.GetBool("collected");
    }private set{
        animator.SetBool("collected", value);
    }}
    public float respawnTime = 5f;
    private float respawnTimer = 0f;
    public AudioClip collectSFX;


    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Update(){
        if(!isHP && collected){
            if(respawnTimer < respawnTime){
                respawnTimer += Time.deltaTime;
            }
            else{
                respawnTimer = 0;
                collected = false;
                animator.SetTrigger("respawn");
                gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision){
        if(!collected){
            if(isHP){
                PlayerDamagable damagable = collision.GetComponent<PlayerDamagable>();
                if(damagable){
                    damagable.Heal(HPRestore);
                    collected = true;
                    //Destroy(gameObject);
                }
            }
            else{
                PlayerController controller = collision.GetComponent<PlayerController>();
                if(controller){
                    controller.RestoreTime(EPRestore);
                    collected = true;
                    //Destroy(gameObject);
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                }
            }
            AudioSource.PlayClipAtPoint(collectSFX, transform.position + new Vector3(0,0,-10), 1f);
        }
    }
}
