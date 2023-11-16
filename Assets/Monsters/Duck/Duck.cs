using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Enemy
{
    protected override void Start()
    {
        // Call the base Start method
        base.Start();

        // Set the values of the variables
        health = 1;
        attackRange = .01f;
        attackDamage = 50;
        attackCooldown = 1.2f;
        movementSpeed = .3f;
        // expLevel = "mid";
        // spawnRate = 2f;
        // maxCount = 5;
    }

}
