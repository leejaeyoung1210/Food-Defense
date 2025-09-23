using UnityEngine;


public static class DataTableIds
{
    public static readonly string Enemy = "EnemyTable";
    public static readonly string Wave = "WaveTable";
    public static readonly string Slot = "WaveSlotTable";
    public static readonly string Tower = "TowerTable";

}
public static class Define
{
    public static int waveCount { get; set; } = 0;
    public static int gold { get; set; } = 300;    
    public static int spawnCost { get; set; } = 10;

}
