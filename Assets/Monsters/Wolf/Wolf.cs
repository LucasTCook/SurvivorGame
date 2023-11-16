using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    protected override void Start()
    {
        // Call the base Start method
        base.Start();

        // Set the values of the variables
        health = 3;
        attackRange = 0.12f;
        attackDamage = 20;
        attackCooldown = 1.2f;
        movementSpeed = .5f;
        // expLevel = "high";
        // spawnRate = 2f;
        // maxCount = 5;
    }
}
