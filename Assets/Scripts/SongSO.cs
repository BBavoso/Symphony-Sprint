using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongNameSO", menuName = "ScriptableObjects/SongSO")]
public class SongSO : ScriptableObject
{
    public List<BeatHolder.Measure> measures;
    public int BPM;
}
