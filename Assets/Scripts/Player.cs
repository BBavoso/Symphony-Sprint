using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public int Score { private set; get; }

    [SerializeField] private GameObject hitTextPrefab;

    public static Player Instance { private set; get; }

    private bool isGrounded;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Player Instance Already Exists!!!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        isGrounded = true;
    }

    private void Update()
    {
        HandleHitSystem();
    }

    private bool CheckForHits(out CandyScript candyScript)
    {
        GameObject closestCandy = CandySpawner.Instance.FindNearestCandyToPlayerInLane(isGrounded);
        if (closestCandy == null)
        {
            candyScript = null;
            return false;
        };
        candyScript = closestCandy.GetComponent<CandyScript>();
        // Debug.Log($"Closest candy is {closestCandy} and is {xDistance} units away");
        return true;
    }

    private void HandleHitSystem()
    {
        if (!Input.GetKeyDown(KeyCode.F) && !Input.GetKeyDown(KeyCode.J))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = new Vector2(0, -3.5f);
            isGrounded = true;
            if (CheckForHits(out CandyScript candyScript))
            {
                DestroyOnHit(candyScript);
            }
            return;

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            transform.position = new Vector2(0, 0);
            isGrounded = false;
            if (CheckForHits(out CandyScript candyScript))
            {
                DestroyOnHit(candyScript);
            }
        }
    }

    private void DestroyOnHit(CandyScript candyScript)
    {
        float xDistanceToPlayer = candyScript.XDistanceToPlayer();
        CandyScript.CandyHitType candyHitType = CandyScript.DistanceToHitType(xDistanceToPlayer);
        // Debug.Log($"hit was {xDistanceToPlayer} away and was a {candyHitType}");
        CreateHitText(candyHitType);
        CandySpawner.Instance.DestroyCandy(candyScript.gameObject);
    }

    private void CreateHitText(CandyScript.CandyHitType candyHitType)
    {
        GameObject hitText = Instantiate(hitTextPrefab);
        hitText.transform.position = new Vector2(0, 2);
        HitTextScript hitTextScript = hitText.GetComponent<HitTextScript>();
        hitTextScript.StartHitText(candyHitType);
    }
}