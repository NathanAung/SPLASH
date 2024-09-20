using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    Vector2 knockback = new Vector2(3f,3f);
    Vector2 targetPos;
    public int damage = 1;
    private void OnTriggerStay2D(Collider2D collision)
    {
        Damagable target = collision.GetComponent<Damagable>();
        if (target != null)
        {
            if (collision.transform.position.x <= this.transform.position.x)
                knockback.x *= -1;
            target.TakeDamage(damage, knockback);
        }
    }
}
