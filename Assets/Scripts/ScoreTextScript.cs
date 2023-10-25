using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreTextScript : MonoBehaviour
{
    [SerializeField] private TMP_Text misses;
    [SerializeField] private TMP_Text goodHits;
    [SerializeField] private TMP_Text perfectHits;

    private Player.Score playerScore;

    private void Start()
    {
        playerScore = Player.Instance.PlayerScore;
    }
    private void Update()
    {
        misses.text = $"Miss: {playerScore.misses}";
        goodHits.text = $"Good: {playerScore.goodHits}";
        perfectHits.text = $"Perfect: {playerScore.perfectHits}";
    }
}
