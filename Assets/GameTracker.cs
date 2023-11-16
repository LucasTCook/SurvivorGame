using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker instance;  // Singleton instance of the game tracker

    // Dictionary to store the count of each enemy prefab
    public Dictionary<GameObject, int> enemyCount = new Dictionary<GameObject, int>();

    void Awake()
    {
        // Check if there is already an instance of the game tracker
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this;
        }
        else if (instance != this)
        {
            // If there is already an instance, destroy this instance to prevent multiple instances
            Destroy(gameObject);
        }
    }

    public void UpdateEnemyCount(GameObject enemyPrefab, int count)
{
    // If the enemy prefab is not in the dictionary, add it with the given count
    if (!enemyCount.ContainsKey(enemyPrefab))
    {
        enemyCount[enemyPrefab] = count;
    }
    // If the enemy prefab is already in the dictionary, update the count
    else
    {
        enemyCount[enemyPrefab] = count;
    }
}
}