using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class TowerTypePrefab
{
    public TowerType type;
    public GameObject prefab;
}

public class TowerSpawner : MonoBehaviour
{
    public TowerTypePrefab[] towerPrefabs;

    private Dictionary<TowerType, GameObject> prefabs;

    private TowerSpot towerSpot;
      
    public UIManager manager;   

    public AudioSource audio;
    public AudioClip spawnClip;

    private void Awake()
    {
        prefabs = new Dictionary<TowerType, GameObject>();
        foreach (var entry in towerPrefabs)
        {
            prefabs[entry.type] = entry.prefab;            
        }
    }

    public void SetTowerSpot(TowerSpot newSpot)
    {
        towerSpot = newSpot;
    }   

    public void SpawnTower()
    {
        if (Define.gold < Define.spawnCost)
        {
            Debug.Log("골드 부족");
            return;
        }

        List<TowerData> towerList = new List<TowerData>(DataTableManager.TowerTableData.GetAll());
        if (towerList.Count == 0)
        {
            Debug.Log("비어있음");
            return;
        }

        float totalWeight = 0f; //확률
        foreach (var tower in towerList) //돌면서 다 넣어주고 
        {
            float w = Mathf.Max(0, tower.Summonprobability);
            totalWeight += w;
        }

        TowerData randomTower = null;

        float r = Random.Range(0f, totalWeight);
        float acc = 0f;
        foreach (var t in towerList) //범위를 다시 더하면서 구하고
        {
            acc += Mathf.Max(0f, t.Summonprobability);
            if (r <= acc) // 그 범위에 속하는 지 확인 후 할당 
            {
                randomTower = t;
                break;
            }
        }
        // 부동소수점 경계 보호 못찾았을경우 끝자리로 강제 
        if (randomTower == null) randomTower = towerList[towerList.Count - 1];

        if (!prefabs.TryGetValue(randomTower.Type, out var prefab))
        {
            return;
        }

        for (int i = 0; i < towerSpot.spotPoints.Count; i++)
        {
            if (towerSpot.spotPoints[i].isSpawning == false && towerSpot.spotPoints[i].tower ==null)
            {
                Define.gold = Mathf.Max(0, Define.gold -= Define.spawnCost);
                GameObject newtower = Instantiate(prefab, towerSpot.spotPoints[i].point, Quaternion.identity);
                towerSpot.spotPoints[i].isSpawning = true;
                towerSpot.spotPoints[i].tower = newtower;
                newtower.GetComponent<TowerHealth>().SetSpot(towerSpot.spotPoints[i]); // 타워가 자기 자리 알고있게
                newtower.GetComponent<Tower>().Init(randomTower);
                Define.spawnCost += 1;

                bool allFiled = towerSpot.AllSpot();
                manager.SetSpawnButton(!allFiled);  
                audio.PlayOneShot(spawnClip);   
                break;
            }
        }
        return;
    }
}
