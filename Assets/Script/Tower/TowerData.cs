using UnityEngine;

public enum TowerType
{    
    Warrior,
    Arrow,    
    Magic
}


[CreateAssetMenu(fileName = "TpwerData", menuName = "Scriptable Objects/TpwerData")]
public class TowerData : ScriptableObject
{
    public TowerType towerType;  
    public float range;
    public float MaxHp;
    public float shootInterval;    
    public float damage;
}
