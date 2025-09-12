using UnityEngine;

[CreateAssetMenu(fileName = "TpwerData", menuName = "Scriptable Objects/TpwerData")]
public class TowerData : ScriptableObject
{
    public float range;
    public float MaxHp;
    public float shootInterval;
    public float shootSpeed;
    public float damage;
}
