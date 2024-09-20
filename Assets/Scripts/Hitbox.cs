using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;
    public Vector2 knockback = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        // get the collided target
        Damagable target = collision.GetComponent<Damagable>();
        // deal damage
        if(target != null){
            // flip knockback direction
            Vector2 knockbackDir = transform.parent.localScale.x > 0 ? knockback : new Vector2(knockback.x * -1, knockback.y);
            target.TakeDamage(damage, knockbackDir);
            //Debug.Log("Dealt " + damage + " to " + collision.name);
        }
    }
}
