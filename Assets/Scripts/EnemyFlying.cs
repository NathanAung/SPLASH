using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : Enemy
{
    public GameObject bomb;
    public GameObject dropPosition;


    private void FixedUpdate(){
        if(damagable.isAlive){
            if(touchingDirections.isOnWall){
                FlipDir();
                //Debug.Log("Flipped");
            }
            if(!wasHit && !touchingDirections.isOnWall){
                if(canMove){
                    rb.velocity = new Vector2(moveSpeed * walkDirVector.x, rb.velocity.y);
                }
                else{
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }


    public void DropBomb(){
        GameObject b = Instantiate(bomb, dropPosition.transform.position, transform.rotation);
    }
}
