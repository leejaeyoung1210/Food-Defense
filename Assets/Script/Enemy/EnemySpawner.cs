using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyTypePrefab
{
    public EnemyTypes type;
    public GameObject prefab;
}

public class EnemySpawner : MonoBehaviour
{
    public EnemyTypePrefab[] enemyPrefabs;

    private Dictionary<EnemyTypes, GameObject> prefabs;

    private void Awake()
    {
        prefabs = new Dictionary<EnemyTypes, GameObject>();
        foreach(var entry in enemyPrefabs)
        {
            prefabs[entry.type] = entry.prefab;
        }
    }

    public void Spawn(EnemyData enemyData, Vector2 towerpos,bool secondSpawner)
    {

        if(!prefabs.TryGetValue(enemyData.Type,out var prefab))
        {
            return;
        }

        var enemy = Instantiate(prefab, towerpos, Quaternion.identity);
        enemy.GetComponent<Enemy>().Init(enemyData);        
        if(secondSpawner)
        {
            var enemySecond = enemy.GetComponent<Enemy>();
            enemySecond.currentPath = GameObject.FindWithTag("Spawn2").GetComponent<WayPoint>();
            enemySecond.targetposition = enemySecond.currentPath.GetWayPoint(0);
            enemySecond.transform.localScale = new Vector2(-1,1);
        }
    }

}
