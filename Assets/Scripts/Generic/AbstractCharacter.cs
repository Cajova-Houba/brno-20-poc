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
    /// Max HP of this character.
    /// </summary>
    public int maxHP = 100;

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

    /// <summary>
    /// Direction of the character's movement. Usually set in Update() method.
    /// </summary>
    protected Vector2 movementDirection;

    protected int currentHP;

    /// <summary>
    /// Method that can be used by other scripts to damage this character.
    /// Also updates health bar.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        Debug.Log(name + " taking " + damage + " damage.");
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
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
        healthBar.SetHealth(currentHP);
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

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(currentHP);
        Init();
    }

    void FixedUpdate()
    {
        if (ShouldMove())
        {
            rb.MovePosition(rb.position + movementDirection.normalized * movementSpeed * Time.fixedDeltaTime);
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
