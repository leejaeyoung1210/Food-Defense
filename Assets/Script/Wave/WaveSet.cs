using UnityEngine;
using System.Collections.Generic;   

[CreateAssetMenu(fileName = "WaveSet", menuName = "Scriptable Objects/WaveSet")]
public class WaveSet : ScriptableObject
{
    public List<WaveData> waves = new List<WaveData>(); 
}
