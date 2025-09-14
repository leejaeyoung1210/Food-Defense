using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;   

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public float waveTime;

    public List<WaveSlot> slots = new List<WaveSlot>(); 
}
