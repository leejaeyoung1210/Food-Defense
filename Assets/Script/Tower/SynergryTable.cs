using System.Collections.Generic;
using UnityEngine;

public class SynergryData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Combo_typeA { get; set; }
    public int Combo_typeB { get; set; }
    public int ValueType1 { get; set; } 
    public float Value1 { get; set; }
    public int ValueType2 { get; set; }
    public float Value2 { get; set; }
}   


public class SynergryTable : DataTable
{
    private readonly Dictionary<int, SynergryData> table = new Dictionary<int, SynergryData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsst = Resources.Load<TextAsset>(path);
        var list = LoadCSV<SynergryData>(textAsst.text);

        foreach (var synergry in list)
        {
            if (!table.ContainsKey(synergry.Id))
            {
                table.Add(synergry.Id, synergry);
            }
            else
            {
                Debug.Log("아이디 중복오류");
            }
        }
    }

    public SynergryData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.Log("Id 없음");
            return null;
        }
        return table[id];
    }

    public IEnumerable<SynergryData> GetAll()
    {
        return table.Values;
    }


}
