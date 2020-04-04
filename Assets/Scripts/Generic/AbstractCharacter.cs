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

    /// <summary>
    /// Direction of the character's movement. Usually set in Update() method.
    /// </summary>
    protected Vector2 movementDirection;

    protected int currentHP;

    protected int currentEnergy;

    /// <summary>
    /// Method that can be used by other scripts to damage this character.
    /// Also updates health bar.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        Debug.Log(name + " taking " + damage + " damage.");
        currentHP -= damage;
        UpdateHealthBar(currentHP);
        if (currentHP <= 0)
        {
            Die();
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
    /// Flips character object so that it's facing the same direction as his movement.
    /// </summary>
    protected void Flip()
    {
        if ((movementDirection.x < 0 && facingRight)
            || (movementDirection.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            rb.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    protected void SetAnimatorSpeedParameter(float speed)
    {
        if (animator != null)
        {
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
            rb.MovePosition(rb.position + movementDirection.normalized * movementSpeed * Time.fixedDeltaTime);
            SetAnimatorSpeedParameter(movementDirection.normalized.magnitude);
            Flip();
            OnAfterMoved();
        }
        else
        {
            // don't move if we're near the player
            rb.MovePosition(rb.position);
        }
    }

    private void Die()
    {
        Debug.Log(name + " is dying");
        OnDying();
        Destroy(gameObject);
    }

}
