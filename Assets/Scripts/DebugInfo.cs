using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{
    UnityEngine.UI.Text enemySpawnRateText,
        tramSpawnRateText,
        enemyCountText;

    public AbstractSpawner enemySpawner,
        tramSpawner;

    void Start()
    {
        enemySpawnRateText = GameObject.Find("EnemySpawnRateText").GetComponent<UnityEngine.UI.Text>();
        tramSpawnRateText = GameObject.Find("SalinaSpawnRateText").GetComponent<UnityEngine.UI.Text>();
        enemyCountText = GameObject.Find("EnemyCountText").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        PrintEnemySpawnRate();
        PrintTramSpawnRate();
        PrintEnemyCount();
    }

    private void PrintEnemyCount()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCountText.text = "Enemy count: " + enemyCount;
    }

    private void PrintTramSpawnRate()
    {
        float tramSpawnRateInverse = tramSpawner.GetUsedSpawnRate();
        string isEnabledText = tramSpawner.spawningEnabled ? " (enabled)" : " (disabled)";
        tramSpawnRateText.text = "Salina spawn rate: " + tramSpawnRateInverse + "/s"+isEnabledText;
    }

    private void PrintEnemySpawnRate()
    {
        float enemySpawnRateInverse = enemySpawner.GetUsedSpawnRate();
        string isEnabledText = enemySpawner.spawningEnabled ? " (enabled)" : " (disabled)";
        enemySpawnRateText.text = "Enemy spawn rate: " + enemySpawnRateInverse + "/s"+isEnabledText;
    }
}
