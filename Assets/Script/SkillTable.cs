//using System.Collections.Generic;
//using UnityEngine;

//public class SkillData
//{
//    public int Id { get; set; }
//    public string Name { get; set; }

//    public int Type { get; set; }
//    public int Target { get; set; }
//    public float Value { get; set; }
//    public float Duration { get; set; }
//    public float Cooldown { get; set; }
//    public string ResourcePath { get; set; }
//}


//public class SkillTable : DataTable
//{
//    private readonly Dictionary<int, SkillData> table = new Dictionary<int, SkillData>();

//    public override void Load(string filename)
//    {
//        //table.Clear();

//        //var path = string.Format(FormatPath, filename);
//        //var textAsst = Resources.Load<TextAsset>(path);
//        //var list = LoadCSV<SkillData>(textAsst.text);

//        //foreach (var skill in list)
//        //{
//        //    if (!table.ContainsKey(skill.Id))
//        //    {
//        //        Debug.Log($" {skill.Id}, {skill.Name}");
//        //        table.Add(skill.Id, skill);
//        //    }
//        //    else
//        //    {
//        //        Debug.Log("아이디 중복오류");
//        //    }
//        //}
//    }
//    //public SkillData Get(int id)
//    //{
//    //    if (!table.ContainsKey(id))
//    //    {
//    //        Debug.Log("Id 없음");
//    //        return null;
//    //    }
//    //    return table[id];
//    //}

//}
