using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalinatorAI : MonoBehaviour
{

    public bool facingRight = false;
    public float speed = 10;

    public float minX = -50;
    public float maxX = 60;

    /// <summary>
    /// Stiff body used to correctly set the z-axis position.
    /// </summary>
    public GameObject stiffBody;

    float direction;

    // Start is called before the first frame update
    void Start()
    {
        SetZPosition();

        // flip to player direction
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Transform player = playerObj.transform;
            direction = (player.position - transform.position).normalized.x;
            Flip();
        } else
        {
            Destroy(gameObject);
        }
    }

    private void SetZPosition()
    {
        Vector3 pos = stiffBody.transform.position;
        Vector3 gameObjPos = gameObject.transform.position;
        gameObjPos.z = pos.y - SettingsHolder.zRange;
        gameObject.transform.position = gameObjPos;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + new Vector2(direction, 0) * speed * Time.fixedDeltaTime);

        if (rb.position.x > maxX || rb.position.x < minX)
        {
            Destroy(gameObject);
        }
    }

    void Flip()
    {
        if ((direction < 0 && facingRight)
            || (direction > 0 && !facingRight))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            facingRight = !facingRight;
            rb.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log(otherCollider.name);

        // must make sure that we hit player's hit box
        AbstractCharacter hitCharacter = otherCollider.GetComponentInParent<AbstractCharacter>();

        if (hitCharacter != null && otherCollider.gameObject.layer == LayerMask.NameToLayer("SalinaHitbox"))
        {
            Debug.Log("Salina hits " + hitCharacter.name);
            hitCharacter.Kill();
        }
    }
}
