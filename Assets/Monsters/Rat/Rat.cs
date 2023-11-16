using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    protected override void Start()
    {
        // Call the base Start method
        base.Start();

        // Set the values of the variables
        health = 1;
        attackRange = .15f;
        attackDamage = 10;
        attackCooldown = 1;
        movementSpeed = .7f;
        // expLevel = "low";
        // spawnRate = .01f;
        // maxCount = 10;
    }
}
