﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPoweup : MonoBehaviour
{
    /// <summary>
    /// Check if trigger with player happened.
    /// </summary>
    /// <param name="otherCollider"></param>
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log(otherCollider.name);
        
        PlayerControl pc = otherCollider.GetComponentInParent<PlayerControl>();
        if (pc != null)
        {
            Debug.Log("Player picks up power-up");
            UsePowerup(pc);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Use power-up on given player.
    /// </summary>
    /// <param name="player">Player to use power-up on.</param>
    protected abstract void UsePowerup(PlayerControl player);
}