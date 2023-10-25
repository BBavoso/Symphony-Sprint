using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CandyScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public bool IsGrounded { set; get; }
    void Update()
    {
        transform.position = (Vector2)transform.position + moveSpeed * Time.deltaTime * Vector2.left;

        CheckDesroy();
    }

    private void CheckDesroy()
    {
        if (transform.position.x <= -5)
        {
            Player.Instance.UpdateScoreBoard(CandyHitType.Miss);
            CandySpawner.Instance.DestroyCandy(gameObject);
        }
    }



    public float XDistanceToPlayer()
    {
        return Math.Abs(Player.Instance.transform.position.x - transform.position.x);
    }

    public enum CandyHitType
    {
        Perfect,
        Good,
        Miss
    }

    public static CandyHitType DistanceToHitType(float distance)
    {
        if (distance > 1)
        {
            return CandyHitType.Miss;
        }
        else if (distance > .5)
        {
            return CandyHitType.Good;
        }
        else
        {
            return CandyHitType.Perfect;
        }
    }
}
