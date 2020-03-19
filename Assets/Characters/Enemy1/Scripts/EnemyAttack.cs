using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform attackPoint;

    /// <summary>
    /// Attack range around attack point.
    /// </summary>
    public float meleeAttackRange = 0.5f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Draw small circle around attack point - good for debugging.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRange);
    }
}
