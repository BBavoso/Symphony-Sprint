using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    [SerializeField] private float songWaitTime;

    private void Start()
    {
        StartCoroutine(StartMusicAfterDelay());
    }
    IEnumerator StartMusicAfterDelay()
    {
        yield return new WaitForSeconds(songWaitTime);
        GetComponent<AudioSource>().Play();
    }
}
