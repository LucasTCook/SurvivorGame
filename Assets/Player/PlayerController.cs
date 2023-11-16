using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{   
    public int currentHealth;
    public int maxHealth = 100;
    public int revival = 0; // extra lives
    public float heal = 0;
    public int armor = 0;
    public float speed = 1f;
    public float radialDistance  = .25f;
    public float cooldownReduction = 0; 
    public float might = 1; // 1 = 100% DMG, 1.5 = 150% DMG
    public float projScale = 0;
    public float projSpeed = 0;
    public float projRange = 0f;
    public int addProj = 0;
    public float addProjMod = .25f; // % of proj scale and damage

    public float magnetForce = 5f; // The force with which the player will pull in the experience orb objects
    public float pickupDistance = .08f;

    public float magnet;
    // public float luck = 1f;
    public float growth = 1; //1.05 for 5%
    // public float greed = 0;
    // public float curse = 0;
    // public int reroll = 0;
    // public int skip = 0;
    // public int banish = 0;
    
    // The prefab to instantiate
    // public GameObject lowExperiencePrefab;
    // public GameObject midExperiencePrefab;
    // public GameObject highExperiencePrefab;
    private GameObject game;
    private GameController gameController;
   
   

    Animator Animator;
    SpriteRenderer SpriteRenderer;
    Collider2D Collider2D;


    void Start()
    {

        game = GameObject.FindWithTag("GameController");
        gameController = game.GetComponent<GameController>();

        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();
       
        currentHealth = maxHealth;
        magnet = radialDistance;
        gameController.healthText.text = "Health: " + maxHealth;


        //Start Coroutines
        StartCoroutine(Heal());

    }
    
    void Update()
    {
        Move(); 
        // Get all the experience orb objects within the player's pull radius
        Collider2D[] experienceOrbs = Physics2D.OverlapCircleAll(transform.position, magnet, 1 << LayerMask.NameToLayer("Exp"));
        
        // Iterate over all the experience orb objects within the player's pull radius
        foreach (Collider2D orb in experienceOrbs)
        {
            // Calculate the direction from the player to the orb
            Vector3 direction = transform.position - orb.transform.position;
            direction = direction.normalized;
            
            // Apply the pull force to the orb
            orb.GetComponent<Rigidbody2D>().AddForce(direction * magnetForce);
            
            // Destroy the orb when it reaches the player
            if (Vector3.Distance(transform.position, orb.transform.position) < pickupDistance)
            {
                print("GAINING "+orb.GetComponent<Experience>().amount * (growth)+" XP!!");
                gameController.currentXP += (int)((float)orb.GetComponent<Experience>().amount * (growth));
                Destroy(orb.gameObject);
            }
        }
    }


    void Move(){
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the new position
        Vector3 newPosition = transform.position + new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;

        // Set the new position

        if(transform.position == newPosition){
            Animator.SetBool("isMoving",false);
        }else{
            Animator.SetBool("isMoving",true);
            transform.position = newPosition;

            if(horizontalInput > 0){
                Collider2D.offset = new Vector2(-.01f, Collider2D.offset.y);
                SpriteRenderer.flipX = false;  
            }

            if(horizontalInput < 0){
                Collider2D.offset = new Vector2(.01f, Collider2D.offset.y);
                SpriteRenderer.flipX = true;
            }

        }
    }


    IEnumerator Heal()
    {
        while (true)
        {
            
            // Heal the player
            if(currentHealth < maxHealth){

                float adjustedHealAmount = maxHealth * heal;
                // print(adjustedHealAmount);
                // print("HEALING: "+ (int)adjustedHealAmount);

                if((currentHealth + (int)adjustedHealAmount) > maxHealth){
                    currentHealth = maxHealth;
                }else{
                    currentHealth += (int)adjustedHealAmount;
                }
                updateHealthDisplay();
            }

            // Wait for 1 second before healing again
            yield return new WaitForSeconds(1);
        }
    }

    public void TakeDamage(int damage)
    {
        // print("TAKING " + damage + " DAMAGE!!");
        Animator.SetTrigger("TakeDamage");
        int postMitDmg = (damage-armor);

        if(postMitDmg > 0){
            currentHealth -= postMitDmg;
        }

        updateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void updateHealthDisplay(){
        gameController.healthText.text = "Health: " + currentHealth;
    }

    void Die()
    {
        if(revival > 0){
            revival -= 1;
            currentHealth = maxHealth/2;
        }else{
            Application.Quit();
            Destroy(gameObject);
        }
        // Destroy the player game object
    }



     void OnDrawGizmos()
    {
        // Set the color of the Gizmos to red
        Gizmos.color = Color.blue;

        // Draw a wireframe sphere representing the diameter of the projectile
        Gizmos.DrawWireSphere(transform.position, radialDistance*(1+projScale));
        Gizmos.DrawWireSphere(transform.position, magnet);
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }


}