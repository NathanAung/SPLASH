using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    public GameObject projectile;
    public GameObject shootPosition;


    public void ShootProjectile(){
        GameObject p = Instantiate(projectile, shootPosition.transform.position, transform.rotation);
        p.transform.localScale = transform.localScale;
    }
}
