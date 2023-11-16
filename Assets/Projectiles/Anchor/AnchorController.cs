using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorController : MonoBehaviour
{
    
    GameObject player; // The player game object
    PlayerController playerController;
    public GameObject projectilePrefab; // The prefab for the projectile
    public float cooldown = .5f; // The interval between shots (in seconds)
    public float projectileSpeed = 1; // The speed of the projectile
    public int projectileAmount = 1;
    public float projectileScale = .1f; 

    Transform anchorTransform;


    void Start()
    {
        // Find the player game object
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        StartCoroutine(SpawnAnchor());
    }

    IEnumerator SpawnAnchor()
    {
        yield return new WaitForSeconds(cooldown);
        // Calculate the initial position of the anchor
        Vector3 spawnPosition = playerController.transform.position + new Vector3((playerController.radialDistance*(1+playerController.projScale)), 0, 0);

        // Spawn the anchor at the calculated position
        GameObject anchor = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Get the anchor's transform component
        anchorTransform = anchor.GetComponent<Transform>();

        // Set the anchor's parent to the player
        anchorTransform.SetParent(playerController.transform);
    }

    void Update()
    {
        // Change the scale of the projectile
        float adjustedProjectileScale = projectileScale + playerController.projScale;

        anchorTransform.localScale = new Vector3(adjustedProjectileScale, adjustedProjectileScale, 1f);
        
        // Calculate the angle to rotate based on the elapsed time
        float angle = 360.0f * (Time.time / (projectileSpeed-playerController.projSpeed));

        // Convert the angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the new position of the anchor
        Vector3 newPosition = playerController.transform.position + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * playerController.radialDistance;

        // Set the position and rotation of the anchor
        anchorTransform.position = newPosition;
        anchorTransform.rotation = Quaternion.Euler(0, 0, angle+90);
        // anchorTransform.LookAt(playerController.transform);
    }


}
