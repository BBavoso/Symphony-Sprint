using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] private GameObject candyPrefab;
    [SerializeField] private float candyHightLow;
    [SerializeField] private float candyHightHigh;
    [SerializeField] private float spawnWaitTime;

    [SerializeField] private List<GameObject> candies;

    public static CandySpawner Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("CandySpawner Instance Already Exists!!!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        candies = new List<GameObject>();
        StartCoroutine(SpawnCandies());
    }
    IEnumerator SpawnCandies()
    {
        while (true)
        {
            SpawnCandy();
            yield return new WaitForSeconds(spawnWaitTime);
        }
    }
    private void SpawnCandy()
    {
        GameObject candy = Instantiate(candyPrefab);
        int lineNumber = Random.Range(0, 2);
        bool isGrounded = lineNumber == 0;
        float y = isGrounded ? candyHightLow : candyHightHigh;
        candy.transform.position = new Vector2(12, y);
        CandyScript candyScript = candy.GetComponent<CandyScript>();
        candyScript.IsGrounded = isGrounded;
        candies.Add(candy);
    }

    public void DestroyCandy(GameObject candyToDestroy)
    {
        candies.Remove(candyToDestroy);
        Destroy(candyToDestroy);
    }

    public GameObject FindNearestCandyToPlayerInLane(bool isGrounded)
    {
        if (candies.Count == 0)
        {
            return null;
        }
        if (candies.Count == 1 && candies[0].GetComponent<CandyScript>().IsGrounded == isGrounded)
        {
            return candies[0];
        }

        GameObject closest = null;
        float closestDist = float.MaxValue;

        // This a bad solution
        // Check if there is at leat one in lane
        bool atLeastOneInLane = false;
        foreach (GameObject candy in candies)
        {
            CandyScript candyScript = candy.GetComponent<CandyScript>();
            if (candyScript.IsGrounded == isGrounded)
            {
                atLeastOneInLane = true;
                break;
            }
        }
        if (!atLeastOneInLane) return null;

        foreach (GameObject candy in candies)
        {
            CandyScript candyScript = candy.GetComponent<CandyScript>();
            if (candyScript.IsGrounded != isGrounded) { continue; }
            float tempDist = Mathf.Abs(candy.transform.position.x - Player.Instance.transform.position.x);
            if (closest == null)
            {
                closest = candy;
                closestDist = tempDist;
                continue;
            }
            if (tempDist < closestDist)
            {
                closest = candy;
                closestDist = tempDist;
            }
        }
        return closest;
    }
}
