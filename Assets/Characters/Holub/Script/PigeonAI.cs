using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonAI : AbstractCharacter
{
    /// <summary>
    /// Change direction every x seconds.
    /// </summary>
    public float directionChangeRate = 5;

    float nextDirChangeTime = 0;

    System.Random random;

    protected override void Init()
    {
        random = new System.Random();
    }

    protected override void OnAfterMoved()
    {
    }

    protected override void OnDying()
    {
    }

    protected override bool ShouldMove()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDirectionChangeTime())
        {
            ChangeDirection();
            CalculateNewDirectionChangeTime();
        }
    }

    private void CalculateNewDirectionChangeTime()
    {
        nextDirChangeTime = Time.time + directionChangeRate;
    }

    private void ChangeDirection()
    {
        // 2/3 = choose random direction
        // 1/3 = stay still
        int r = random.Next(3);
        switch(r)
        {
            case 0:
            case 1:
                movementDirection.x = 1 - random.Next(3);
                movementDirection.y = 1 - random.Next(3);
                break;
            case 2:
                movementDirection.x = 0;
                movementDirection.y = 0;
                break;
        }

        Debug.Log("Pidgeon dir: " + movementDirection);
    }

    private bool IsDirectionChangeTime()
    {
        return Time.time >= nextDirChangeTime;
    }
}
