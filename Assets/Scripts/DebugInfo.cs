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
        tramSpawnRateText = GameObject.Find("SalinaSpawnRateText").GetComponent<UnityEngine.UI.Text>();
        enemyCountText = GameObject.Find("EnemyCountText").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
