using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // public float DespawnDistance = 3f; // The maximum distance from the player to spawn the enemy
    public float separationForce = .01f; // Adjust this value to change the strength of the separation force
    public bool isKnockedBack;

    protected int health;
    protected float attackRange;
    protected int attackDamage;
    protected float attackCooldown;
    protected float movementSpeed;
    protected string expLevel;
    // public int maxCount;

    // public GameObject damageText;
    // public GameObject damageTextPrefab;
    // public GameObject canvasPrefab;

    Text dmgText;

    GameObject player;
    GameObject spawner;
    GameObject game;
    public Experience ExperiencePrefab;
    
    GameController gameController;
    PlayerController playerController;
    EnemySpawner enemySpawner;


    private SpriteRenderer spriteRenderer;
    float lastAttackTime;

    protected virtual void Start()
    {
        game = GameObject.FindWithTag("GameController");
        gameController = game.GetComponent<GameController>();

        // Find the player game object and player controller script
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();


        spriteRenderer = this.GetComponent<SpriteRenderer>(); 
        spawner = GameObject.FindWithTag("Spawner");
        enemySpawner = spawner.GetComponent<EnemySpawner>();
        gameObject.layer = 6;
        gameObject.tag = "Untagged";
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Exp"));

    }


    void Update()
    {
        spritePosition();
        AdjustVelocity();
        Movement();
    }

    void AdjustVelocity(){
        // // Calculate the direction from the enemy to the player
        // Vector3 directionToPlayer = player.transform.position - transform.position;

        // // Check if the enemy is moving away from the player
        // if (Vector3.Dot(directionToPlayer, GetComponent<Rigidbody2D>().velocity) < 0)
        // {
        //     // Calculate the force needed to push the enemy towards the player
        //     Vector3 force = directionToPlayer.normalized * movementSpeed - (Vector3)GetComponent<Rigidbody2D>().velocity;

        //     // Apply the force to the enemy
        //     GetComponent<Rigidbody2D>().AddForce(force);
        // }

        if(!isKnockedBack){
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Set the velocity of the enemy to zero
            rb.velocity = Vector2.zero;
        }
    }

    void Movement(){
        // Get the center of the enemy's collider
        Vector2 enemyCenter = GetComponent<Collider2D>().bounds.center;

        // Get the closest point on the player's collider to the enemy's center
        Vector2 closestPoint = player.GetComponent<Collider2D>().ClosestPoint(enemyCenter);

        // Calculate the distance between the enemy's center and the closest point on the player's collider
        float distanceToPlayer = Vector2.Distance(enemyCenter, closestPoint);

        // Check if the distance exceeds the maximum spawn distance
        if (distanceToPlayer > enemySpawner.spawnDistance)
        {

            Vector3 playerDirection = player.transform.position - transform.position;
            playerDirection = Vector3.Normalize(playerDirection);
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            float newDistance = enemySpawner.spawnDistance;
            transform.position = player.transform.position + newDistance * playerDirection;



        }
        else if (distanceToPlayer < attackRange && Time.time > lastAttackTime + attackCooldown){
            // If the enemy is within the attack range, attack the player if the cooldown has expired
            AttackPlayer();
        }else{
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
    }
    

    void spritePosition(){
        spriteRenderer.flipX = player.transform.position.x > this.transform.position.x;
        
        // Compare the Y-positions of the enemy and the player
        if (transform.position.y < player.transform.position.y)
        {
            // Set the "Order in Layer" value to a higher value than the player's "Order in Layer" value to draw the enemy on top of the player
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            // Set the "Order in Layer" value to a lower value than the player's "Order in Layer" value to draw the enemy behind the player
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }

        // Set the z angle of the enemy's transform to 0 to prevent rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    void Despawn(){
        print("DESPAWN");
        Destroy(gameObject);
    }


    void AttackPlayer()
    {
        // Deal damage to the player
        playerController.TakeDamage(attackDamage);

        // Update the last attack time
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        // DisplayDamageText(damage);
        // print("REMAINING HEALTH: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die(){
        // print("Enemy Destroyed----DROP XP");
        gameController.enemiesKilled += 1;
        gameController.enemiesKilledText.text = "Enemies Killed: " + gameController.enemiesKilled;
        enemySpawner.currentEnemyTotal--;
        Destroy(gameObject);

        int expCount = GameObject.FindGameObjectsWithTag("Exp").Length;
        if(expCount < 20){
            //Stack up the XP Chest
            Instantiate(ExperiencePrefab, transform.position, Quaternion.identity);
        }



    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider is a projectile
        if (collider.tag == "Projectile")
        {
            // Check if the projectile game object has a Projectile component
            Projectile projectile = collider.GetComponent<Projectile>();
            if (projectile != null)
            {
                print("STRARTING KNOCKBACK");
                // Get the Rigidbody2D component of the enemy and projectile
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                Rigidbody2D projectileRB = collider.GetComponent<Rigidbody2D>();


                // Calculate the distance that the enemy should be knocked back
                float knockbackDistance = projectile.knockbackDistance;  // The distance that the enemy should be knocked back (in meters)
                float knockbackDuration = projectile.knockbackDuration;  // The duration of the knockback (in seconds)

                // Calculate the knockback force
                //In the direction the proj is going
                ////// Vector2 knockbackForce = projectileRB.velocity;
                ////// print(knockbackForce);


                // Get the position of the collision point
                Vector2 collisionPoint = collider.transform.position;

                // Get the position of the player's center
                Vector2 playerCenter = playerController.transform.position;

                // Calculate the vector between the collision point and the player's center
                Vector2 knockbackForce = collisionPoint - playerCenter;


                //In the direction of colider angles
                // Vector2 knockbackForce = rb.position - (Vector2)collider.transform.position;
                knockbackForce.Normalize();
                knockbackForce *= knockbackDistance / knockbackDuration;
                print(knockbackForce);
                // Start the Knockback coroutine


                StartCoroutine(Knockback(knockbackDuration, knockbackForce));

                IEnumerator Knockback(float duration, Vector2 force)
                {
                    // force = new Vector2(5, 10);
                    //
                    //Pause knockbacks while being knocked back
                    //
                    // Check if the enemy is already being knocked back
                    if (!isKnockedBack){
                        // Apply the knockback force for the specified duration
                        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
                        isKnockedBack = true;
                        yield return new WaitForSeconds(duration);
                        isKnockedBack = false;
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    }

                    // Set the isKnockedBack flag to false
                    // yield return new WaitForSeconds(duration);

                    // Stop the enemy's movement
                }
                float adjustedDamage = projectile.damage * playerController.might;
                if(projectile.isAddProj){
                    adjustedDamage *=playerController.addProjMod;
                }
                // print("DEALING "+ (int)adjustedDamage + " DAMAGE");
                // Deal the damage to the enemy
                TakeDamage((int)adjustedDamage);
            }
        }
    }




    void OnCollisionEnter2D  (Collision2D collision)
    {
        // Check if the collision is with another enemy
        if (collision.gameObject.tag == "Enemy")
        {
            // Calculate the direction to separate the enemies
            Vector2 separationDirection = (collision.transform.position - transform.position).normalized;

            // Apply a larger separation force to the enemies
            float separationForce = 50f;  // The separation force to apply to the enemies (in Newtons)
            GetComponent<Rigidbody2D>().AddForce(-separationDirection * separationForce * Time.deltaTime);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(separationDirection * separationForce * Time.deltaTime);

            // Apply a damping force to the enemies to slow them down
            float dampingForce = 10f;  // The damping force to apply to the enemies (in Newtons)
            GetComponent<Rigidbody2D>().AddForce(-GetComponent<Rigidbody2D>().velocity * dampingForce * Time.deltaTime);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.gameObject.GetComponent<Rigidbody2D>().velocity * dampingForce * Time.deltaTime);
        }
    }


    void OnDrawGizmos()
    {
        // Set the color of the Gizmos to red
        Gizmos.color = Color.blue;

        // Draw a wireframe sphere representing the diameter of the projectile
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}