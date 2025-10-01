using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Threading;

public class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        Init();
    }

    private static void Init()
    {
        var enemyTable = new EnemyTable();
        enemyTable.Load(DataTableIds.Enemy);
        tables.Add(DataTableIds.Enemy, enemyTable);
        

        var waveSlotTable = new WaveSlotTable();
        waveSlotTable.enemyTable = enemyTable;
        waveSlotTable.Load(DataTableIds.Slot);
        tables.Add(DataTableIds.Slot, waveSlotTable);    

        var waveTable = new WaveTable();
        waveTable.slotTable = waveSlotTable;
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);

        var towerTable = new TowerTable();
        towerTable.Load(DataTableIds.Tower);
        tables.Add(DataTableIds.Tower, towerTable);

        var synergyTable = new SynergryTable();
        synergyTable.Load(DataTableIds.Synergy);
        tables.Add(DataTableIds.Synergy, synergyTable);

        //var skillTable = new SkillTable();  
        //skillTable.Load(DataTableIds.Skill);
        //tables.Add(DataTableIds.Tower, skillTable);
    }

    public static EnemyTable EnemyTableData
    {
        get
        {
            return Get<EnemyTable>(DataTableIds.Enemy);
        }
    }

    public static WaveSlotTable WaveSlotTableData
    {
        get
        {
            return Get<WaveSlotTable>(DataTableIds.Slot);
        }
    }
    public static WaveTable WaveTableData
    {
        get
        {
            return Get<WaveTable>(DataTableIds.Wave);
        }
    }
    public static TowerTable TowerTableData
    {
        get 
        {
            return Get<TowerTable>(DataTableIds.Tower);
        }
    }
    public static SynergryTable SynergyTableData
    {
        get
        {
            return Get<SynergryTable>(DataTableIds.Synergy);
        }
    }   

    //public static SkillTable SkillTableData
    //{
    //    get
    //    {
    //        return Get<SkillTable>(DataTableIds.Skill);
    //    }
    //}   

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.Log("id ¿À·ù");
            return null;
        }
        return tables[id] as T;
    }
}
