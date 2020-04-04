using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movements and holds player's HP.
/// </summary>
public class PlayerControl : AbstractCharacter
{

    public BoxCollider2D boxCollider;

    /// <summary>
    /// Prefab of the dead player object that is to be spawned when the player dies.
    /// </summary>
    public GameObject deadPlayer;

    public GameObject restartDialog;

    protected override void Init()
    {
        
    }

    protected override void OnAfterMoved()
    {
    }

    protected override void OnDying()
    {
        // spawn the dead player
        GameObject child = Instantiate(deadPlayer, transform.position, transform.rotation);

        // display restart dialog
        restartDialog.SetActive(true);
    }

    protected override bool ShouldMove()
    {
        // player should move anytime
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");
    }

    /// <summary>
    /// Draw the box colider.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
        {
            return;
        }

        Gizmos.DrawWireCube(boxCollider.transform.position, boxCollider.offset);
    }
}
