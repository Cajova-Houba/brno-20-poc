using Assets.Scripts.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for character objects. Those have HP, can take damage
/// and may have their own AI (or can be controlled by input).
/// </summary>
public abstract class AbstractCharacter : MonoBehaviour
{
    /// <summary>
    /// Movement speed parameter set in animator.
    /// </summary>
    public const string IS_MOVING_ANIM_NAME = "isMoving";

    public const string HIT_1_ANIM_NAME = "hit1";
    public const string HIT_2_ANIM_NAME = "hit2";

    /// <summary>
    /// Animator used for walking/idle animations.
    /// 
    /// Sets speed parameter.
    /// 
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Max HP of this character.
    /// </summary>
    public int maxHP = 100;

    /// <summary>
    /// Character's energy. Used for skills etc.
    /// </summary>
    public int maxEnergy = 100;

    /// <summary>
    /// Dependes on the orientation of character sprite.
    /// </summary>
    public bool facingRight = true;

    /// <summary>
    /// How fast this character moves.
    /// </summary>
    public float movementSpeed = 3f;

    public Rigidbody2D rb;

    public HealthBar healthBar;

    public HealthBar energyBar;

    public GameObject sprite;

    /// <summary>
    /// Stiff body used to calculate correct z-axis position.
    /// </summary>
    public GameObject stiffBody;

    /// <summary>
    /// Attacks available to this character.
    /// </summary>
    public AbstractAttack[] attacks;

    /// <summary>
    /// How long should the character be stunned when hit.
    /// </summary>
    public float stunDuration;

    /// <summary>
    /// Direction of the character's movement. Usually set in Update() method.
    /// </summary>
    protected Vector2 movementDirection;

    protected bool stunned = false;

    protected int currentHP;

    protected int currentEnergy;

    /// <summary>
    /// Flag used to govern stun types
    /// true = 1st stun 
    /// false = 2nd stun
    /// </summary>
    private bool stunType = true;

    /// <summary>
    /// Method that can be used by other scripts to damage this character.
    /// Also updates health bar.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>True if this character was killed by the damage.</returns>
    public bool TakeDamage(int damage)
    {
        Debug.Log(name + " taking " + damage + " damage.");
        currentHP -= damage;
        UpdateHealthBar(currentHP);
        bool dead = currentHP <= 0;
        if (dead)
        {
            Die();
        } else if (IsStunnable() && !IsStunned())
        {
            // stun the character only if it's stunnable and is not stunned already
            StartCoroutine(Stun());
        }

        return dead;
    }

    /// <summary>
    /// Returns true if this character is stunned.
    /// </summary>
    /// <returns></returns>
    public bool IsStunned()
    {
        return stunned;
    }

    /// <summary>
    /// Returns true if this character can be stunned by attack.
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsStunnable()
    {
        return true;
    }

    /// <summary>
    /// Coroutine which sets the stunned flag, yields for the stun duration and the re-sets the stunned flag.
    /// Should be called when a stunnable character is hit.
    /// </summary>
    private IEnumerator Stun()
    {
        stunned = true;
        PlayStunnedAnimation();
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
    }

    private void PlayStunnedAnimation()
    {
        string triggerName = stunType ? HIT_1_ANIM_NAME : HIT_2_ANIM_NAME;
        stunType = !stunType;
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    /// <summary>
    /// Kills this characters (takes damage equal to its current HP).
    /// </summary>
    public void Kill()
    {
        TakeDamage(currentHP);
    }

    /// <summary>
    /// Heals this character by given hp (health is still capped at maxHealth).
    /// </summary>
    /// <param name="hp"></param>
    public void Heal(int hp)
    {
        currentHP = Math.Min(maxHP, currentHP + hp);
        UpdateHealthBar(currentHP);
    }

    /// <summary>
    /// Heals this character's energy by given amount up to the max energy.
    /// </summary>
    /// <param name="energy"></param>
    public void HealEnergy(int energy)
    {
        currentEnergy = Math.Min(maxEnergy, currentEnergy + energy);
        UpdateEnergyBar(currentEnergy);
    }

    /// <summary>
    /// Checks if the character has enough energy.
    /// </summary>
    /// <param name="requiredEnergy">Energy required by some action.</param>
    /// <returns>True if currentEnergy >= requiredEnergy</returns>
    public bool HasEnoughEnergy(int requiredEnergy)
    {
        return currentEnergy >= requiredEnergy;
    }

    /// <summary>
    /// Uses given amount of energy.
    /// </summary>
    /// <param name="energy"></param>
    public void UseEnergy(int energy)
    {
        currentEnergy = Math.Max(0, currentEnergy - energy);
        UpdateEnergyBar(currentEnergy);
    }

    /// <summary>
    /// Called when character dies just before its game obejct is destroyed.
    /// </summary>
    protected abstract void OnDying();

    /// <summary>
    /// Called in FixedUpate() to decide whether the character should move or not.
    /// </summary>
    /// <returns></returns>
    protected abstract bool ShouldMove();

    /// <summary>
    /// Called in FixedUpdate() after MovePosition() is called.
    /// </summary>
    protected abstract void OnAfterMoved();

    /// <summary>
    /// Init method called in Start().
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// Movement speed to be used during movement. Returns movementSpeed by default
    /// but can be overriden to set different speed when e.g. following player.
    /// </summary>
    /// <returns></returns>
    protected virtual float GetMovementSpeed()
    {
        return movementSpeed;
    }

    /// <summary>
    /// Flips character object so that it's facing the same direction as his movement.
    /// </summary>
    protected void Flip()
    {
        if ((movementDirection.x < 0 && facingRight)
            || (movementDirection.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            rb.transform.Rotate(new Vector3(0, 180, 0));

            //set sprite position to z = -z to 'reverse' the flip rotation
            Vector3 p = sprite.transform.position;
            p.z = -p.z;
            sprite.transform.position = p;
        }
    }

    protected void SetAnimatorWalkingAnimation(float speed)
    {
        if (animator != null)
        {
            //Debug.Log("Setting " + IS_MOVING_ANIM_NAME + " to: " + ((Math.Abs(speed) - 0) > 0.001));
            animator.SetBool(IS_MOVING_ANIM_NAME, (Math.Abs(speed) - 0) > 0.001);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        currentEnergy = maxEnergy;
        SetHealthBarMax(currentHP);
        SetEnergyBarMax(currentEnergy);
        Init();
    }

    protected void UpdateEnergyBar(int newEnergy)
    {
        if (energyBar != null)
        {
            energyBar.SetHealth(newEnergy);
        }
    }

    protected void UpdateHealthBar(int newHP)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(newHP);
        }
    }

    private void SetEnergyBarMax(int newMaxEnergy)
    {
        if (energyBar != null)
        {
            energyBar.SetMaxHealth(newMaxEnergy);
        }
    }

    private void SetHealthBarMax(int newMaxHP)
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(newMaxHP);
        }
    }

    void FixedUpdate()
    {
        if (ShouldMove())
        {
            Debug.Log(name + " is moving with speed " + GetMovementSpeed());
            rb.MovePosition(rb.position + movementDirection.normalized * GetMovementSpeed() * Time.fixedDeltaTime);
            UpdateSpriteZAxis();

            SetAnimatorWalkingAnimation(movementDirection.normalized.magnitude*GetMovementSpeed());
            Flip();
            OnAfterMoved();
        }
        else
        {
            SetAnimatorWalkingAnimation(0);
            // don't move if we're near the player
            rb.MovePosition(rb.position);
        }
    }
    

    /// <summary>
    /// Sets the position.z value of the sprite so that it overlaps correctly
    /// with sprites of other objects.
    /// 
    /// The z-axis position is calculated relative to the position of 
    /// character's stiff body, not game object.
    /// 
    /// Changing the z position of sprite does not affect physics or collisions so it should be ok.
    /// </summary>
    private void UpdateSpriteZAxis()
    {
        Vector3 pos = stiffBody.transform.position;
        Vector3 spritePos = sprite.transform.position;
        spritePos.z = pos.y - 6.2f;
        sprite.transform.position = spritePos;
    }

    private void Die()
    {
        Debug.Log(name + " is dying");
        OnDying();
        Destroy(gameObject);
    }

}
