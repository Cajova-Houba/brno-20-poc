using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;


    Transform player;

    Vector2 playerDirection;

    bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = player.position - transform.position;

        Flip();
    }

    void Flip()
    {
        if ((playerDirection.x < 0 && facingRight)
            || (playerDirection.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            rb.transform.Rotate(new Vector3(0, 180, 0));
        }
    }


    /// <summary>
    /// Draw are of the enemy's box colider.
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
