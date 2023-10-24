using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private LineNumber currentLine;
    private int score;

    public event EventHandler<int> OnScoreChange;

    public enum LineNumber
    {
        Line1,
        Line2,
        Line3,
        Line4,
        Line5
    }

    private void Start()
    {
        currentLine = LineNumber.Line3;
        transform.position = new Vector2(0f, LineNumberToYPosition(LineNumber.Line3));
        score = 0;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        score += 1;
        OnScoreChange?.Invoke(this, score);
        Destroy(other.gameObject);
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentLine == LineNumber.Line5) { return; }
            MoveUpOrDown(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentLine == LineNumber.Line1) { return; }
            MoveUpOrDown(false);
        }
    }

    private void MoveUpOrDown(bool moveUp)
    {
        int lineNumberAsInt = (int)currentLine + (Convert.ToInt32(moveUp) * 2 - 1);
        currentLine = IntToLineNumber(lineNumberAsInt);
        MoveCharacterToLineNumber(currentLine);
    }
    /// <summary>
    /// Transforms a line number into the line absolute y level
    /// </summary>
    /// <param name="lineNumber">LineNumber Object</param>
    /// <returns>Global y position (float) of Line</returns>
    public static float LineNumberToYPosition(LineNumber lineNumber)
    {
        return lineNumber switch
        {
            LineNumber.Line1 => -4f,
            LineNumber.Line2 => -2f,
            LineNumber.Line3 => -0f,
            LineNumber.Line4 => 2f,
            LineNumber.Line5 => 4f,
            _ => -1,
        };
    }

    private void MoveCharacterToLineNumber(LineNumber lineNumber)
    {
        transform.position = new Vector2(0, LineNumberToYPosition(lineNumber));
    }
    /// <summary>
    /// Takes an integer between 0 and 4 and returns a matching LineNumber Object
    /// </summary>
    /// <param name="number">and integer between one and 5</param>
    /// <returns>Returns LineNumber</returns>
    public static LineNumber IntToLineNumber(int number)
    {
        return (LineNumber)number;
    }
}
