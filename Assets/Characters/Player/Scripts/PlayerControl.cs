using Assets.Characters.Player.Scripts;
using Assets.Powerups;
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
    /// <summary>
    /// Prefab of the dead player object that is to be spawned when the player dies.
    /// </summary>
    public GameObject deadPlayer;

    /// <summary>
    /// Guiding point for enemies. Instead of following the player's position, the will try to follow this point.
    /// </summary>
    public GameObject frontTargetPoint;

    /// <summary>
    /// Same as front target but this one is behind the player.
    /// </summary>
    public GameObject backTargetPoint;

    /// <summary>
    /// Object used to control animation displayed when a power up is pocked.
    /// </summary>
    public PickedUpPowerup pickedUpPowerup;

    bool attack1Pressed = false;
    bool attack2Pressed = false;
    bool superAttackPressed = false;

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

    /// <summary>
    /// Uses triggerName to play pickedup animation using PickedUpPowerup object.
    /// </summary>
    /// <param name="triggerName">Name of the animation trigger.</param>
    public void PlayPowerupPickedAnimation(string triggerName)
    {
        pickedUpPowerup.TriggerAnimation(triggerName);
    }

    protected override float GetMovementSpeed()
    {
        if (isAttacking || IsStunned())
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
        return true;
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
        GameObject child = Instantiate(deadPlayer.gameObject, sprite.transform.position, transform.rotation);
    }

    protected override bool ShouldMove()
    {
        // player should move anytime
        return true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        attack1Pressed = Input.GetKeyDown(KeyCode.J);
        attack2Pressed = Input.GetKeyDown(KeyCode.K);
        superAttackPressed = Input.GetKeyDown(KeyCode.L);

        HandleAttack();
    }

    private void HandleAttack()
    {
        if (attacks == null || attacks.Length == 0)
        {
            return;
        }

        if (isAttacking || IsStunned())
        {
            // player is already attacking or is stunned, nothing to do
            return;
        }

        AbstractAttack attack = null;
        if (superAttackPressed && attacks[2].CanUseAttack())
        {
            attack = attacks[2];
        } else if (attack1Pressed && attacks[0].CanUseAttack())
        {
            attack = attacks[0];
        } else if (attack2Pressed && attacks[1].CanUseAttack())
        {
            attack = attacks[1];
        }

        if (attack != null)
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
        yield return StartCoroutine(attack.UseAttack(this));
        Debug.Log("Player attack finished.");
        isAttacking = false;
        killedEnemies += attack.GetTargetKillCount();
        Debug.Log("Killed enemies:" + killedEnemies);
    }
}
