using System.Collections.Generic;
using UnityEngine;


public enum EnemyTypes
{
    Knight = 1,
    Archer,
    Wizard,
}

//public enum AttackType
//{
//    Melee = 1,
//    Ranged,
//    Magic,
//}
public class EnemyData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EnemyTypes Type { get; set; }
    public int Level { get; set; }
    public int Hp { get; set; }
    public int AttackPower { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }  
    public float Range {  get; set; }
    public string Skill1 { get; set; }
    public int? Skill1Id { get; set; }

    public string Skill2 { get; set; }
    public int? Skill2Id { get; set; }
}
public class EnemyTable : DataTable
{
    private readonly Dictionary<int, EnemyData> table = new Dictionary<int, EnemyData>();

    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsst = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EnemyData>(textAsst.text);

        foreach (var enemy in list)
        {
            if (!table.ContainsKey(enemy.Id))
            {
                Debug.Log($" {enemy.Id}, {enemy.Name}");
                table.Add(enemy.Id, enemy);
            }
            else
            {
                Debug.Log("아이디 중복오류");
            }
        }
    }

    public EnemyData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.Log("Id 없음");
            return null;
        }
        return table[id];
    }


}