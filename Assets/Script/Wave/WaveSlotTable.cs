using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class WaveSlot
{
    public int waveId { get; set; }
    public int enemyId { get; set; } // 어떤 적
    public int count { get; set; } // 몇 마리
    [CsvHelper.Configuration.Attributes.Ignore]
    public EnemyData enemyData { get; set; }

  
}

public class WaveSlotTable : DataTable
{
    private readonly Dictionary<int, List<WaveSlot>> slotTable = new Dictionary<int, List<WaveSlot>>();
    public  EnemyTable enemyTable { get; set; }  
    public override void Load(string filename)
    {
        slotTable.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsst = Resources.Load<TextAsset>(path);
        var list = LoadCSV<WaveSlot>(textAsst.text);

        foreach (var slot in list)
        {
            //if (!slotTable.ContainsKey(slot.waveId))
            //{
            //    slotTable[slot.waveId] = new List<WaveSlot>();

            //    Debug.Log($" {slot.waveId}, {slot.enemyId}");
            //    slotTable[slot.waveId].Add(new WaveSlot
            //    {
            //        waveId = slot.waveId,
            //        enemyId = slot.enemyId,
            //        count = slot.count,
            //        enemyData = enemyData.Get(slot.enemyId)  
            //    });
            //}
            //else
            //{
            //    Debug.Log("아이디 중복오류");
            //}
            if (!slotTable.TryGetValue(slot.waveId, out var slots))
            {
                slots = new List<WaveSlot>();
                slotTable[slot.waveId] = slots;
            }

            slots.Add(new WaveSlot
            {
                waveId = slot.waveId,
                enemyId = slot.enemyId,
                count = slot.count,
                enemyData = enemyTable?.Get(slot.enemyId) // null-safe
            });
        }
    }
    public List<WaveSlot> Get(int waveId)
    {
        return slotTable.TryGetValue(waveId, out var slots)
         ? slots: new List<WaveSlot>();
    }
}
