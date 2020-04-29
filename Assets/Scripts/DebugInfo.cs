using Assets.Scripts.Generic;
using Assets.Scripts.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{
    UnityEngine.UI.Text enemiesKilledText,
        tramSpawnRateText,
        enemyCountText;

    public AbstractSpawner enemySpawner,
        tramSpawner;

    public AbstractLevel levelInfo;

    void Start()
    {
        tramSpawnRateText = GameObject.Find("SalinaSpawnRateText").GetComponent<UnityEngine.UI.Text>();
        enemyCountText = GameObject.Find("EnemyCountText").GetComponent<UnityEngine.UI.Text>();
        enemiesKilledText = GameObject.Find("EnemiesKilledText").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        PrintTramSpawnRate();
        PrintEnemyCount();
    }

    private void PrintEnemyCount()
    {
        if (enemyCountText == null)
        {
            return;
        }
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCountText.text = "Enemy count: " + enemyCount;
    }

    private void PrintTramSpawnRate()
    {
        if (tramSpawner == null)
        {
            return;
        }
        float tramSpawnRateInverse = tramSpawner.GetUsedSpawnRate();
        string isEnabledText = tramSpawner.spawningEnabled ? " (enabled)" : " (disabled)";
        tramSpawnRateText.text = "Salina spawn rate: " + tramSpawnRateInverse + "/s"+isEnabledText;
        enemiesKilledText.text = "Enemies killed: " + levelInfo.GetEnemiesKilled();
    }
}
