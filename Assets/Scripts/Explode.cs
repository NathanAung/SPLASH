using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public int damage = 10;
    public Vector2 knockback = Vector2.zero;


    private void OnTriggerEnter2D(Collider2D collision){
        // get the collided target
        Damagable target = collision.GetComponent<Damagable>();
        // deal damage
        if(target != null){
            // flip knockback direction
            Vector2 knockbackDir = collision.transform.position.x < transform.position.x ? new Vector2(knockback.x * -1, knockback.y) : knockback;
            target.TakeDamage(damage, knockbackDir);
            //Debug.Log("Dealt " + damage + " to " + collision.name);
        }
    }
}
