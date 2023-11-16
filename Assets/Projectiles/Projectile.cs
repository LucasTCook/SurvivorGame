using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;  // The damage dealt by the projectile
    public float knockbackDistance = .1f;
    public float knockbackDuration = 1f;
    public int enemyPassThru = 1;
    public bool isAddProj = false;


    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider is an enemy
        if (collider.tag == "Enemy")
        {
            // Decrement the enemyPassThru value
            enemyPassThru--;

            // Destroy the projectile if the enemyPassThru value has reached 0
            if (enemyPassThru <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
