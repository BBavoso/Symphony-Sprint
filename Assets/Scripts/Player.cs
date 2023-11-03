using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [HideInInspector] public Score PlayerScore { private set; get; }

    [SerializeField] private GameObject hitTextPrefab;

    [SerializeField] public Animator anim;

    public static Player Instance { private set; get; }

    private bool isGrounded;

    public static KeyCode hitKey = KeyCode.F;

    public static KeyCode jumpKey = KeyCode.J;

    public class Score
    {
        public int misses;
        public int goodHits;
        public int perfectHits;

        public Score()
        {
            this.misses = 0;
            this.goodHits = 0;
            this.perfectHits = 0;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Player Instance Already Exists!!!");
            return;
        }
        Instance = this;
        PlayerScore = new Score();
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
        // GameObject closestCandy = CandySpawner.Instance.FindNearestCandyToPlayerInLane(isGrounded);

        GameObject[] candies = GameObject.FindGameObjectsWithTag("Item");
        CandyScript closestCandy = null;
        foreach (var candy in candies)
        {
            var candyGameObjectScript = candy.GetComponent<CandyScript>();

            if (candyGameObjectScript.IsGrounded == isGrounded)
            {
                if (closestCandy == null)
                {
                    closestCandy = candyGameObjectScript;
                }
                else
                {
                    closestCandy = closestCandy.XDistanceToPlayer() > candyGameObjectScript.XDistanceToPlayer() ? candyGameObjectScript : closestCandy;
                }
            }
        }

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
        if (!Input.GetKeyDown(hitKey) && !Input.GetKeyDown(jumpKey))
        {
            return;
        }
        if (Input.GetKeyDown(hitKey))
        {
            transform.position = new Vector2(0, -3.5f);
            isGrounded = true;
            if (CheckForHits(out CandyScript candyScript))
            {
                HandleSuccessfulHit(candyScript);
            }
        
        }
        if (Input.GetKeyDown(jumpKey))
        {
            transform.position = new Vector2(0, 0);
            isGrounded = false;
            if (CheckForHits(out CandyScript candyScript))
            {
                HandleSuccessfulHit(candyScript);
            }
        
        }
    }


    private void HandleSuccessfulHit(CandyScript candyScript)
    {
        float xDistanceToPlayer = candyScript.XDistanceToPlayer();
        CandyScript.CandyHitType candyHitType = CandyScript.DistanceToHitType(xDistanceToPlayer);
        UpdateScoreBoard(candyHitType);
        // Debug.Log($"hit was {xDistanceToPlayer} away and was a {candyHitType}");
        CreateHitText(candyHitType);
        Destroy(candyScript.gameObject);
        // CandySpawner.Instance.DestroyCandy(candyScript.gameObject);
    }

    public void UpdateScoreBoard(CandyScript.CandyHitType candyHitType)
    {
        switch (candyHitType)
        {
            case CandyScript.CandyHitType.Perfect:
                PlayerScore.perfectHits += 1;
                break;
            case CandyScript.CandyHitType.Good:
                PlayerScore.goodHits += 1;
                break;
            case CandyScript.CandyHitType.Miss:
                PlayerScore.misses += 1;
                break;
        }
    }

    private void CreateHitText(CandyScript.CandyHitType candyHitType)
    {
        GameObject hitText = Instantiate(hitTextPrefab);
        float hitTextYPosition = isGrounded ? -2.5f : 1.5f;
        hitText.transform.position = new Vector2(0, hitTextYPosition);
        HitTextScript hitTextScript = hitText.GetComponent<HitTextScript>();
        hitTextScript.StartHitText(candyHitType);
    }

    public static KeyCode[] GetControls()
    {
        KeyCode[] controls = { hitKey, jumpKey };
        return controls;
    }
}