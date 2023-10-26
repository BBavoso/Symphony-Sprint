using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatHolder : MonoBehaviour
{

    // ***********************************************
    // Fields
    // ***********************************************
    [SerializeField] private SongSO songSO;

    private Queue<Measure> measures;



    // ***********************************************
    // Beat Holder Functionality
    // ***********************************************

    private void Start()
    {
        measures = new Queue<Measure>(songSO.measures);
    }





    // ***********************************************
    // Measure -> Beat -> Note
    // Definitions and Utility Functions
    // ***********************************************


    public bool HasNextMeasure(out Measure measure)
    {
        if (measures.Count == 0)
        {
            measure = new Measure { };
            return false;
        }
        measure = measures.Dequeue();
        return true;
    }

    public static Measure CreateNewMeasureOfFour()
    {
        return new Measure { beats = CreateNBeatList(4) };
    }

    public static List<Beat> CreateNBeatList(int n)
    {
        List<Beat> beats = new();
        for (int i = 0; i < n; i++)
        {
            beats.Add(CreateNewSixteenthBeat());
        }
        return beats;
    }

    public static Beat CreateNewSixteenthBeat()
    {
        return new Beat
        {
            notes = CreateNNoteList(4)
        };
    }

    public static List<Note> CreateNNoteList(int n)
    {
        List<Note> notes = new();
        for (int i = 0; i < n; i++)
        {
            notes.Add(CreateDefaultNote());
        }
        return notes;
    }

    public static Note CreateDefaultNote()
    {
        return new Note
        {
            playNote = false,
            isGrounded = true
        };
    }

    public static Note CreateLaneNote(bool topLane, bool bottomLane)
    {
        Note note = CreateDefaultNote();

        if (topLane || bottomLane)
        {
            note.playNote = true;
        }
        if (topLane)
        {
            note.isGrounded = false;
        }

        return note;
    }

    [System.Serializable]
    public struct Measure
    {
        public List<Beat> beats;
    }
    [System.Serializable]
    public struct Beat
    {
        public List<Note> notes;
    }

    [System.Serializable]
    public struct Note
    {
        public bool playNote;
        public bool isGrounded;
    }

}
