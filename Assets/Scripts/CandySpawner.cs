using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] private GameObject candyPrefab;
    [SerializeField] private float candyHightLow;
    [SerializeField] private float candyHightHigh;
    [SerializeField] private float candySpawnXPosition;
    [SerializeField] public float candyMoveSpeed;

    [SerializeField] public Button resumeButton;
    public static CandySpawner Instance;


    private List<GameObject> candies;

    private AudioSource songAudioSource;
    private SongSO currentSong;

    // private bool songFinished;


    private float musicStartDelay;

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
        musicStartDelay = candySpawnXPosition / candyMoveSpeed;

        // Setup Audio
        // songFinished = false;
        currentSong = BeatHolder.Instance.GetSong();

        songAudioSource = GetComponent<AudioSource>();
        songAudioSource.clip = currentSong.songAudio;

        // Start Coroutines
        StartCoroutine(SpawnCandies());
        StartCoroutine(StartMusic());


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            songAudioSource.Pause();
            resumeButton.onClick.AddListener(Resume);
        }
    }

    private void Resume() {
        songAudioSource.UnPause();
    }

    IEnumerator SpawnCandies()
    {
        // Loop Measures
        while (BeatHolder.Instance.GetNextMeasure(out BeatHolder.Measure measure))
        {
            Queue<BeatHolder.Beat> beats = new(measure.beats);
            // Loop Beats
            while (beats.Count > 0)
            {
                BeatHolder.Beat beat = beats.Dequeue();
                Queue<BeatHolder.Note> notes = new(beat.notes);
                float timeBetweenNotes = 60f / (currentSong.BPM * notes.Count);
                // Loop Notes
                while (notes.Count > 0)
                {
                    BeatHolder.Note note = notes.Dequeue();
                    if (note.playNote)
                    {
                        SpawnCandy(note.isGrounded);
                    }
                    yield return new WaitForSeconds(timeBetweenNotes);
                }
            }
        }
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(musicStartDelay);
        songAudioSource.Play();

    }
    private void SpawnCandy(bool isGrounded)
    {
        GameObject candy = Instantiate(candyPrefab);
        float y = isGrounded ? candyHightLow : candyHightHigh;
        candy.transform.position = new Vector2(candySpawnXPosition, y);
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
