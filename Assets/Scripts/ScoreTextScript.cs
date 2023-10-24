using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreTextScript : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    TextMeshProUGUI gui;
    private void Start()
    {
        gui = GetComponent<TextMeshProUGUI>();
        gui.text = "0";
        player.OnScoreChange += Player_OnScoreChange;
    }

    private void Player_OnScoreChange(object sender, int score)
    {
        gui.text = score.ToString();
    }
}
