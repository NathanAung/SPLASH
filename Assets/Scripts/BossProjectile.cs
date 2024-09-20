using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : Projectile
{
    public GameObject dropItem;

    private void OnTriggerEnter2D(Collider2D collision){
        // get the collided target
        Damagable target = collision.GetComponent<Damagable>();
        // deal damage
        if(target != null){
            // flip knockback direction
            Vector2 knockbackDir = transform.localScale.x > 0 ? knockback : new Vector2(knockback.x * -1, knockback.y);
            target.TakeDamage(damage, knockbackDir);
            //Debug.Log("Dealt " + damage + " to " + collision.name);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox")){
            Instantiate(dropItem, transform.position, transform.rotation);
        }
        animator.SetTrigger("wasHit");
        wasHit = true;
        AudioSource.PlayClipAtPoint(hitSFX, transform.position + new Vector3(0,0,-10), 0.5f);
    }
}
