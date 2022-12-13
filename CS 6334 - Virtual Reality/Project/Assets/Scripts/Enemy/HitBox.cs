using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    public void OnRaycastHit(PlayerFire playerFire, Vector3 direction)
    {
        enemyHealth.TakeDamage(playerFire.damage, direction);
    }
}
