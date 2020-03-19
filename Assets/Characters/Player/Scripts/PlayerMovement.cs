using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    public Rigidbody2D rb;

    public float movementSpeed = 3f;

    Vector2 movement;
    bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        Flip();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    void Flip()
    {
        if ((movement.x < 0 && facingRight) 
            || (movement.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            rb.transform.Rotate(new Vector3(0, 180, 0));
        }
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
