using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using CsvHelper.Configuration.Attributes;

public class WaveData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float waveTime { get; set; }
    public int waveRewardGold { get; set; }

    [CsvHelper.Configuration.Attributes.Ignore]
    public float spawnInterval = 1f;

    [CsvHelper.Configuration.Attributes.Ignore]
    public List<WaveSlot> slots = new List<WaveSlot>();
}

public class WaveTable : DataTable
{
    private readonly Dictionary<int, WaveData> waves = new Dictionary<int, WaveData>();
    private List<WaveData> ordered = new List<WaveData>();
    public WaveSlotTable slotTable { get; set; }
    
    public override void Load(string filename)
    {
        waves.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<WaveData>(textAsset.text);

        foreach (var wave in list)
        {
            if (!waves.ContainsKey(wave.Id))
            {
                Debug.Log($" {wave.Id}, {wave.Name}");
                wave.slots = SlotsLoad(wave.Id);
                waves.Add(wave.Id, wave);

            }
            else { Debug.Log("ม฿บน"); }
        }
        ordered = waves.Values.OrderBy(w => w.Id).ToList();
    }

    private List<WaveSlot> SlotsLoad(int waveid)
    {
        return slotTable.Get(waveid);
    }

    public int Count => waves.Count;

    public WaveData GetByIndex(int index)
    {
        if (ordered == null || index < 0 || index >= ordered.Count) return null;
        return ordered[index];
    }

    public WaveData Get(int waveid) => waves.TryGetValue(waveid, out var wave) ? wave : null;

}
