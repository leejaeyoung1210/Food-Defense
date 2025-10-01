using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public enum TowerType
{
    None,
    Warrior,
    Arrow,
    Magic
}

public class TowerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TowerType Type { get; set; }
    public int Combo_type { get; set; }
    public int Level { get; set; }
    public int NextId { get; set; }
    public bool Upgradeable { get; set; }
    public int UpgradeCost { get; set; }
    public int Hp { get; set; }
    public int AttackPower { get; set; }    
    public float AttackSpeed { get; set; }
    public float Range { get; set; }
    

    public string Skill1 { get; set; }
    public int? SkillId { get; set; }

    public string Skill2 { get; set; }
    public int? Skill2Id { get; set; }

    public float Summonprobability { get; set; }
    public int ResellPrice { get; set; }

    public string Icon { get; set; }

    [CsvHelper.Configuration.Attributes.Ignore]
    public Sprite spriteIcon
    {
        get
        {
            string[] spriteSheets;                

            switch(Type)
            {
                case TowerType.Warrior:
                    spriteSheets = new []{ "Icon/warrior1", "Icon/warrior2", "Icon/warrior3" };
                    break;
                case TowerType.Arrow:
                    spriteSheets = new[] { "Icon/arrow1", "Icon/arrow2", "Icon/arrow3" };
                    break;
                case TowerType.Magic:
                    spriteSheets = new[] { "Icon/magic1", "Icon/magic2", "Icon/magic3" };
                    break;
                    default:
                     return null ;    
            }
            foreach (string sheet in spriteSheets)
            {
                var sprites = Resources.LoadAll<Sprite>(sheet);
                var found = System.Array.Find(sprites, s => s.name == Icon);
                if (found != null)
                    return found;
            }
            return null ;
        }
    }
}



public class TowerTable : DataTable
{
    private readonly Dictionary<int, TowerData> table = new Dictionary<int, TowerData>();

    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsst = Resources.Load<TextAsset>(path);
        var list = LoadCSV<TowerData>(textAsst.text);

        foreach (var tower in list)
        {
            if (!table.ContainsKey(tower.Id))
            {               
                table.Add(tower.Id, tower);
            }
            else
            {
                Debug.Log("아이디 중복오류");
            }
        }
    }
    public TowerData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.Log("Id 없음");
            return null;
        }
        return table[id];
    }

    public IEnumerable<TowerData> GetAll()
    {
        return table.Values;
    }
}
