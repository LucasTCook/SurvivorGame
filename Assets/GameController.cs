using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentXP = 0;
    public int nextLevelXP;
    
    public int enemiesKilled = 0;
    public Text enemiesKilledText;
    public Text healthText;
    public Image levelUpUI;

    public Text expText;
    // Start is called before the first frame update
    void Start()
    {
        //initialize game setup
        currentLevel = 1;
        nextLevelXP = CalculateXpForNextLevel(currentLevel);
        enemiesKilledText.text = "Enemies Killed: 0"; 
        expText.text = "EXP: 0";
    }

    // Update is called once per frame
    void Update()
    {
        expText.text = "Current XP: " + currentXP;
        CheckLevelUp();
    }

    void CheckLevelUp(){
        if(currentXP >= nextLevelXP){
            StartCoroutine(ShowLevelUpUI());
            currentLevel++;
            nextLevelXP = CalculateXpForNextLevel(currentLevel);
            CheckLevelUp();
        }
    }

    public int CalculateXpForNextLevel(int currentLevel)
    {
        if (currentLevel < 2)
        {
            return 5;
        }
        else if (currentLevel < 21)
        {
            return 10 * (currentLevel - 1) + 5;
        }
        else if (currentLevel < 41)
        {
            return 13 * (currentLevel - 1) + 85;
        }
        else
        {
            return 16 * (currentLevel - 1) + 205;
        }
    }

    IEnumerator ShowLevelUpUI()
    {
        // Set the image to be visible
        levelUpUI.enabled = true;

        // Wait for a specified duration
        yield return new WaitForSeconds(.5f);

        // Set the image to be invisible
        levelUpUI.enabled = false;
    }
}
