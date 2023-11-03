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


    private float measureTimer;

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
        musicStartDelay = (candySpawnXPosition / candyMoveSpeed);
        measureTimer = 0f;

        // Setup Audio
        // songFinished = false;
        currentSong = BeatHolder.Instance.GetSong();

        songAudioSource = GetComponent<AudioSource>();
        songAudioSource.clip = currentSong.songAudio;

        // Start Coroutines
        StartCoroutine(StartMusic());

        StartCoroutine(SpawnCandies());
    }

    public void Update()
    {
        measureTimer -= Time.deltaTime;

        // Right now this makes it only work for 4/4
        // todo: change this to work with all time signatures

        // float timeUntilNextMeasure = (4 * 60) / currentSong.BPM;

        // if (measureTimer <= 0 && BeatHolder.Instance.GetNextMeasure(out BeatHolder.Measure measure))
        // {
        //     StartCoroutine(SpawnMeasureOfCandies(measure));
        //     measureTimer = timeUntilNextMeasure;
        // }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            songAudioSource.Pause();
            resumeButton.onClick.AddListener(Resume);
        }
    }

    List<BeatHolder.Note> CreateNotes()
    {
        var result = new List<BeatHolder.Note>();

        foreach (BeatHolder.Measure measure in currentSong.measures)
        {
            foreach (BeatHolder.Beat beat in measure.beats)
            {
                foreach (BeatHolder.Note note in beat.notes)
                {
                    result.Add(note);
                }
            }
        }


        return result;
    }

    private void Resume()
    {
        songAudioSource.UnPause();
    }

    IEnumerator SpawnCandies()
    {
        // Loop Measures
        var notes = CreateNotes();
        foreach (BeatHolder.Note note in notes)
        {
            float timeBetweenNotes = 1 / 8f;
            if (note.playNote)
            {
                SpawnCandy(note.isGrounded);
            }


            yield return new WaitForSeconds(timeBetweenNotes);

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
