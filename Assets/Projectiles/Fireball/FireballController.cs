using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    
    GameObject player; // The player game object
    PlayerController playerController;
    public GameObject projectilePrefab; // The prefab for the projectile
    public float cooldown = .5f; // The interval between shots (in seconds)
    public float projectileSpeed = 5; // The speed of the projectile
    public float projectileScale = .1f; // The scale of the projectile
    public float projectileRange = 2f;
    public int projectileAmount = 1;
    float lastFireTime; // The time of the last shot
    Vector3 direction; // The direction of the projectile

    void Start()
    {
        // Set the initial direction of the projectile to the right
        direction = Vector3.right;
        // Find the player game object
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // void Update()
    // {
    //     // Check if the fire rate interval has elapsed
    //     if (Time.time > lastFireTime + (cooldown - playerController.cooldownReduction))
    //     {
    //         // Find all enemies in the scene
    //         GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //         // Store the distance to the closest enemy and the closest enemy game object
    //         float closestEnemyDistance = float.MaxValue;
    //         GameObject closestEnemy = null;

    //         // Find the closest enemy
    //         foreach (GameObject enemy in enemies)
    //         {
    //             float distance = Vector3.Distance(transform.position, enemy.transform.position);
    //             if (distance < closestEnemyDistance)
    //             {
    //                 closestEnemyDistance = distance;
    //                 closestEnemy = enemy;
    //             }
    //         }

    //         //Calculate adjusted Range of attack
    //         float adjustedProjectileRange = projectileRange + playerController.projRange;
            

    //         // Calculate the direction to the closest enemy
    //         // Vector3 direction;
    //         if (closestEnemy != null && closestEnemyDistance <= adjustedProjectileRange)
    //         {
    //             // print("COOLDOWN SHOT");
    //             // Set the last fire time to the current time
    //             lastFireTime = Time.time;
    //             StartCoroutine(ShootEnemy(closestEnemy));
    //         }

    //     }
    // }

    void Update()
    {
        // Check if the fire rate interval has elapsed
        if (Time.time > lastFireTime + (cooldown - playerController.cooldownReduction))
        {
            // Get the layer mask for the "Enemies" layer
            int enemyLayer = LayerMask.NameToLayer("Enemies");
            LayerMask enemyMask = 1 << enemyLayer;

            // Get all colliders in the "Enemies" layer within the specified radius
            float radius = projectileRange + playerController.projRange;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);

            // Store the distance to the closest enemy and the closest enemy game object
            float closestEnemyDistance = float.MaxValue;
            GameObject closestEnemy = null;

            // Find the closest enemy
            foreach (Collider2D enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestEnemyDistance)
                {
                    closestEnemyDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }

            // Calculate the direction to the closest enemy
            if (closestEnemy != null)
            {
                // Set the last fire time to the current time
                lastFireTime = Time.time;
                StartCoroutine(ShootEnemy(closestEnemy));
            }
        }
    }



    IEnumerator ShootEnemy(GameObject closestEnemy){
        // Calculate the direction to the closest enemy
        Vector3 direction = closestEnemy.transform.position - transform.position;

        




        int numberOfProjectiles = playerController.addProj + projectileAmount;

        print("Number of proj: " + numberOfProjectiles);
        for(int i=0;i<numberOfProjectiles;i++){
            
            // Get the velocity of the player
            Vector3 playerVelocity = GetComponent<Rigidbody2D>().velocity;

            // Calculate the initial position of the projectile by adding the velocity of the player
            Vector3 initialPosition = transform.position;
            // Spawn a new projectile
            GameObject projectile = Instantiate(projectilePrefab, initialPosition + playerVelocity, Quaternion.identity);
            Projectile projectileObj = projectile.GetComponent<Projectile>();

            
            // Change the scale of the projectile
            float adjustedProjectileScale = projectileScale + playerController.projScale;

            if (i > 0) {
                projectileObj.isAddProj = true;
                //Added Projectiles have different effects
                adjustedProjectileScale = adjustedProjectileScale * playerController.addProjMod;
            }

            projectile.transform.localScale = new Vector3(adjustedProjectileScale, adjustedProjectileScale, 1f);

            // Calculate the angle between the direction of the projectile and the direction to the closest enemy
            float angle = Vector2.SignedAngle(Vector3.right, direction);

            // Create a quaternion that represents this angle
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Calculate adjusted Speed and range of attack
            float adjustedProjectileSpeed = projectileSpeed + playerController.projSpeed;
            float adjustedProjectileRange = projectileRange + playerController.projRange;


            projectile.GetComponent<Rigidbody2D>().velocity = rotation * Vector3.right * adjustedProjectileSpeed + playerVelocity;

            // Calculate the time it will take for the projectile to reach its maximum distance
            float timeToReachMaxDistance = adjustedProjectileRange / adjustedProjectileSpeed;

            // Destroy the projectile after it has reached its maximum distance
            Destroy(projectile, timeToReachMaxDistance);
            // yield return new WaitForSeconds(.05f);

            if(numberOfProjectiles > 1){
                // print("ADDITIONAL SHOT");
                float timeToSeparateProj = (cooldown - playerController.cooldownReduction) / numberOfProjectiles;
                yield return new WaitForSeconds(timeToSeparateProj);
            }
        }


        
    }

    // void OnDrawGizmos()
    // {
    //     // Set the color of the Gizmos to red
    //     Gizmos.color = Color.red;

    //     // Calculate the projected position of the projectile at the desired length
    //     Vector3 projectedPosition = transform.position + (direction * projectileRange);

    //     // Draw a line from the current position to the projected position
    //     Gizmos.DrawLine(transform.position, projectedPosition);

    //     // Draw a line representing the velocity of the projectile
    //     Gizmos.DrawRay(transform.position, direction.normalized * projectileSpeed);

    //     // Draw a wireframe sphere representing the diameter of the projectile
    //     Gizmos.DrawWireSphere(projectedPosition, projectileScale);
    // }
}
