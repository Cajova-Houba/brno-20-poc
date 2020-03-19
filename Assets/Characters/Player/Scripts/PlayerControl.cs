﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int maxHealth = 100;

    public HealthBar healthBar;

    /// <summary>
    /// Prefab of the dead player object that is to be spawned when the player dies.
    /// </summary>
    public GameObject deadPlayer;

    int currentHealth;

    public void TakeDamage(int damage)
    {
        Debug.Log("Player taking " + damage + " damage.");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        Debug.Log(transform.position);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("The player died.");

        // spawn the dead player
        GameObject child = Instantiate(deadPlayer, transform.position, transform.rotation);

        // destroy the player object
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            TakeDamage(currentHealth);
        }
    }
}