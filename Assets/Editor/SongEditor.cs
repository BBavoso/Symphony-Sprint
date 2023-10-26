using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class SongEditor : EditorWindow
{
    private SongSO song;
    private List<BeatHolder.Measure> measures;
    private IMGUIContainer songFields;

    private BeatHolder.Measure currentMeasure;


    public VisualTreeAsset SongEditor_UXML;
    public VisualTreeAsset BeatEditor_UXML;
    public VisualTreeAsset NoteEditor_UXML;

    private bool decreaseAttatched;
    private bool increaseAttatched;

    private void OnEnable()
    {


        TemplateContainer treeAsset = SongEditor_UXML.CloneTree();
        rootVisualElement.Add(treeAsset);

        songFields = rootVisualElement.Query<IMGUIContainer>("SongFields").First();
        songFields.visible = false;
        decreaseAttatched = false;
        increaseAttatched = false;
    }
    [MenuItem("Tools/SongEditor")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<SongEditor>();
        window.titleContent = new GUIContent("Song Editor");
    }
    private void CreateGUI()
    {
        CreateSongList();
    }

    private void CreateSongList()
    {
        FindAllSongs(out SongSO[] songs);

        ListView songList = rootVisualElement.Query<ListView>("SongList").First();
        songList.makeItem = () => new Label();
        songList.bindItem = (element, i) => (element as Label).text = songs[i].name;

        songList.itemsSource = songs;
        // songList.itemHeight = 16;
        songList.selectionType = SelectionType.Single;

        songList.selectionChanged += OnSongSelectionChanged;
    }

    private void OnSongSelectionChanged(IEnumerable<object> enumerable)
    {

        ListView measureList = rootVisualElement.Query<ListView>("MeasureList").First();
        measureList.selectionChanged -= OnMeasureSelectionChanged;
        measureList.Clear();

        song = enumerable.First() as SongSO;
        EditorUtility.SetDirty(song);
        measures = song.measures;

        measureList.makeItem = () => new Label();
        measureList.bindItem = (element, i) => (element as Label).text = $"Measure {i + 1}";

        measureList.itemsSource = measures;
        measureList.selectionType = SelectionType.Single;


        // Enable and Change Fields
        if (!songFields.visible) { songFields.visible = true; }
        // BPM
        IntegerField BPMIntField = rootVisualElement.Query<IntegerField>("BeatsPerMinute").First();
        BPMIntField.value = song.BPM;

        SerializedObject SongSerialiezedObject = new SerializedObject(song);
        SerializedProperty BPMProperty = SongSerialiezedObject.FindProperty("BPM");

        BPMIntField.BindProperty(BPMProperty);

        // Total Measures
        IntegerField MeasureCountIntField = rootVisualElement.Query<IntegerField>("TotalMeasures").First();
        MeasureCountIntField.value = song.measures.Count;

        MeasureCountIntField.bindingPath = "arraySize";

        // SerializedProperty songMeasuresProperty = SongSerialiezedObject.FindProperty("measures");

        if (!decreaseAttatched)
        {
            Button measuresDecrease = rootVisualElement.Query<Button>("MeasuresDecrease");
            measuresDecrease.clicked += DecreaseMeasuresClicked;
            decreaseAttatched = true;
        }

        void DecreaseMeasuresClicked()
        {
            if (measures.Count == 0) { return; }
            measures.RemoveAt(measures.Count - 1);
            MeasureCountIntField.value = song.measures.Count;
            measureList.Rebuild();
        }

        if (!increaseAttatched)
        {
            Button measuresIncrease = rootVisualElement.Query<Button>("MeasuresIncrease");
            measuresIncrease.clicked += IncreaseMeasuresClicked;
            increaseAttatched = true;
        }

        void IncreaseMeasuresClicked()
        {
            measures.Add(BeatHolder.CreateNewMeasureOfFour());
            MeasureCountIntField.value = song.measures.Count;
            measureList.Rebuild();
        };


        measureList.selectionChanged += OnMeasureSelectionChanged;
    }


    private void OnMeasureSelectionChanged(IEnumerable<object> enumerable)
    {
        ScrollView measureEditor = rootVisualElement.Query<ScrollView>("MeasureEditor").First();
        // measureEditor.Clear();
        BeatHolder.Measure measure = (BeatHolder.Measure)enumerable.First();
        currentMeasure = measure;

        List<BeatHolder.Beat> beats = measure.beats;
        IMGUIContainer beatsContainer = measureEditor.Query<IMGUIContainer>("BeatsContainer");
        beatsContainer.Clear();
        int i = 1;
        foreach (BeatHolder.Beat beat in beats)
        {
            TemplateContainer beatEditor = BeatEditor_UXML.CloneTree();

            Label beatLabel = beatEditor.Query<Label>("BeatLabel");
            beatLabel.text = $"Beat {i}";

            IMGUIContainer notesContainer = beatEditor.Query<IMGUIContainer>("NotesContainer");
            notesContainer.Clear();

            List<BeatHolder.Note> notes = beat.notes;
            for (int j = 0; j < notes.Count; j++)
            {
                BeatHolder.Note note = notes[j];

                TemplateContainer noteEditor = NoteEditor_UXML.CloneTree();

                GroupBox noteHolder = noteEditor.Query<GroupBox>("NoteHolder");
                Button clearNoteButton = noteEditor.Query<Button>("ClearNote");

                RadioButton noteTopLane = noteEditor.Query<RadioButton>("NoteTopLane");
                RadioButton noteBottomLane = noteEditor.Query<RadioButton>("NoteBottomLane");

                if (note.playNote)
                {
                    if (note.isGrounded)
                    {
                        noteBottomLane.value = true;
                    }
                    else
                    {
                        noteTopLane.value = true;
                    }
                }

                noteHolder.RegisterCallback<ChangeEvent<bool>>((evt) =>
                {
                    UpdateAllNotesInCurrentMeasure();
                });

                clearNoteButton.clicked += () =>
                {
                    noteTopLane.value = false;
                    noteBottomLane.value = false;
                };

                notesContainer.Add(noteEditor);
            }


            beatsContainer.Add(beatEditor);

            i++;
        }
    }

    private void UpdateAllNotesInCurrentMeasure()
    {
        List<VisualElement> beatEditors = rootVisualElement.Query<VisualElement>("BeatContainer").ToList();
        int i = 0;
        foreach (BeatHolder.Beat beat in currentMeasure.beats)
        {
            VisualElement selectedBeatEditor = beatEditors[i];


            List<GroupBox> noteContainers = selectedBeatEditor.Query<GroupBox>("NoteHolder").ToList();
            for (int j = 0; j < beat.notes.Count; j++)
            {
                BeatHolder.Note note = beat.notes[j];
                GroupBox currentNote = noteContainers[j];

                bool topLane = currentNote.Query<RadioButton>("NoteTopLane").First().value;
                bool bottomLane = currentNote.Query<RadioButton>("NoteBottomLane").First().value;

                BeatHolder.Note newNote = BeatHolder.CreateLaneNote(topLane, bottomLane);

                currentMeasure.beats[i].notes[j] = newNote;
                // note = newNote;
            }

            i++;
        }
    }

    private void FindAllSongs(out SongSO[] songs)
    {
        string[] guids = AssetDatabase.FindAssets("t:SongSO");

        songs = new SongSO[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            songs[i] = AssetDatabase.LoadAssetAtPath<SongSO>(assetPath);
        }
    }


}