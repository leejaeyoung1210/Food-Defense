using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float range;
    public float MaxHp;
    public float attackInterval;
    public float attackSpeed;
    public float damage;
}
