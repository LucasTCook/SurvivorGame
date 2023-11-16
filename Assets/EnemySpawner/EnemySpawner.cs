using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    public GameObject WolfPrefab;
    public GameObject RatPrefab;
    public GameObject DuckPrefab;
    
    GameObject player; // The player game object
    SpriteRenderer SpriteRenderer;

    //WAVE DESIGN
    Dictionary<int, Dictionary<GameObject, double>> waves;
    
    public int currentEnemyTotal = 0;
    public float spawnDistance = 3.5f; // The maximum distance from the player to spawn the enemy
    // public float spawnInterval = .1f; // The interval between enemy spawns
    public float waveDuration = 60;  // Wave duration in seconds
    private float elapsedTime = 0;  // Elapsed time since the start of the current wave
    public int currentWave = 1;  // Current wave number

    public int enemyCap = 300;
    int[] waveMinimums = new int[30];

    void Start()
    {
        // Find the player game object
        player = GameObject.FindWithTag("Player");

        InitializeWaves();
        currentWave = 1; 

        //START FIRST WAVE
        Dictionary<GameObject, double> enemies = waves[1];

        // Iterate through the dictionary and spawn the enemies
        foreach (KeyValuePair<GameObject, double> entry in enemies)
        {
            GameObject enemyPrefab = entry.Key;
            print(entry.Key);
            double spawnVal = entry.Value;
            string[] arr = spawnVal.ToString().Split('.');
            int spawnCount = int.Parse(arr[0]);
            float spawnRate = (float.Parse(arr[1]) * 0.01f);
            if(waveDuration/spawnRate < spawnCount){
                spawnRate = waveDuration/spawnCount;
            }

            StartCoroutine(SpawnEnemies(enemyPrefab, spawnCount,spawnRate));
        }
    }

    void InitializeWaves(){
        // Initialize the waves dictionary
        waves = new Dictionary<int, Dictionary<GameObject, double>>();

        // Wave 1
        waves[1] = new Dictionary<GameObject, double>();
        waves[1][RatPrefab] = 10.025;
        waves[1][WolfPrefab] = 20.501;
        waveMinimums[1] = 5;

        waves[2] = new Dictionary<GameObject, double>();
        waves[2][WolfPrefab] = 15.05; 
        waveMinimums[2] = 10;

        waves[3] = new Dictionary<GameObject, double>();
        waves[3][DuckPrefab] = 10.02;
        waveMinimums[3] = 5;

    }

    void Update()
    {
        checkWave();
    }


void checkWave()
{
    elapsedTime += Time.deltaTime;  // Update the elapsed time

    if (elapsedTime >= waveDuration)  // Check if the wave duration has been reached
    {
        // Reset the elapsed time
        elapsedTime = 0;

        // Check if the current wave is the last wave in the dictionary
        if (currentWave != waves.Keys.Max())
        {
            // Increment the current wave number
            currentWave++;
        }else{
            currentWave = 1;
        }
        // else{
        //     Destroy(gameObject);
        // }

        Dictionary<GameObject, double> enemies = waves[currentWave];

        // Show the wave UI
        StartCoroutine(ShowWaveUI());

        // Iterate through the dictionary and spawn the enemies
        foreach (KeyValuePair<GameObject, double> entry in enemies)
        {
            GameObject enemyPrefab = entry.Key;
            // print(entry.Key);
            double spawnVal = entry.Value;
            string[] arr = spawnVal.ToString().Split('.');
            int spawnCount = int.Parse(arr[0]);
            float spawnRate = (float.Parse(arr[1]) * 0.01f);
            if(waveDuration/spawnRate < spawnCount){
                spawnRate = waveDuration/spawnCount;
            }
            
            StartCoroutine(SpawnEnemies(enemyPrefab, spawnCount,spawnRate));
        }


    }else{
        if(waveMinimums[currentWave] > currentEnemyTotal){
            //Spawn 1 of everything on this wave
            Dictionary<GameObject, double> enemies = waves[currentWave];

            // Iterate through the dictionary and spawn the enemies
            foreach (KeyValuePair<GameObject, double> entry in enemies)
            {
                GameObject enemyPrefab = entry.Key;
                // print(entry.Key);
                double spawnVal = entry.Value;
                string[] arr = spawnVal.ToString().Split('.');
                int spawnCount = 1;
                float spawnRate = (float.Parse(arr[1]) * 0.01f);
                if(waveDuration/spawnRate < spawnCount){
                    spawnRate = waveDuration/spawnCount;
                }
                StartCoroutine(SpawnEnemies(enemyPrefab, spawnCount,spawnRate));
            }
        }
    }

}

    IEnumerator ShowWaveUI()
    {
        // Get the Image component
        Image image = GetComponentInChildren<Image>();

        // Set the image to be visible
        image.enabled = true;

        // Wait for a specified duration
        yield return new WaitForSeconds(.5f);

        // Set the image to be invisible
        image.enabled = false;
    }


    IEnumerator SpawnEnemies(GameObject enemyPrefab,int spawnCount, float spawnRate)
    {
        print("SPAWN "+ spawnCount +  " ENEMIES @ "+ spawnRate.ToString());
        
            // Continuously spawn enemies until the maximum count is reached
            for (int i = 0; i < (spawnCount); i++)
            {
                currentEnemyTotal++;
                // Spawn the enemy prefab at a random position around the player
                float angle = Random.Range(0f, 360f);
                Vector3 spawnPosition = player.transform.position + spawnDistance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);

                if(spawnCount != 1){
                    // Wait for a specified delay before spawning the next enemy
                    yield return new WaitForSeconds(spawnRate);
                }
            }

    }




}