using Assets.Scripts.Generic;
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

    bool attack1Pressed = false;

    /// <summary>
    /// Flag set for the duration of attack.
    /// </summary>
    bool isAttacking = false;

    /// <summary>
    /// Number of enemies killed by player
    /// </summary>
    int killedEnemies = 0;

    public int GetKilledEnemiesCount()
    {
        return killedEnemies;
    }

    protected override float GetMovementSpeed()
    {
        if (isAttacking)
        {
            // attacking = cant move
            return 0f;
        } else
        {
            return base.GetMovementSpeed();
        }
    }

    protected override bool IsStunnable()
    {
        // player is not stunnable rn
        return false;
    }

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

        attack1Pressed = Input.GetKeyDown(KeyCode.J);

        HandleAttack();
    }

    private void HandleAttack()
    {
        if (attacks == null || attacks.Length == 0)
        {
            return;
        }

        if (isAttacking)
        {
            // player is already attacking, nothing to do
            return;
        }

        AbstractAttack attack = attacks[0];
        if (attack1Pressed && attack.CanUseAttack())
        {
            StartCoroutine(DoAttack(attack));
        }
    }

    /// <summary>
    /// Coroutine which will raise attacking flag, executes actual attack
    /// and after its finished, resets the attacking flag again.
    /// </summary>
    /// <param name="attack">Attack to execute.</param>
    /// <returns></returns>
    private IEnumerator DoAttack(AbstractAttack attack)
    {
        isAttacking = true;
        yield return StartCoroutine(attack.UseAttack());
        Debug.Log("Player attack finished.");
        isAttacking = false;
        killedEnemies += attack.GetTargetKillCount();
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
