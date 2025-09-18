using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;   

public class TowerSpawner : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    private TowerSpot towerSpot;
    private int randomType;

    GameObject towerPrefab;
 
    private TowerSpot towerSpottest;   // 선택

    private void Awake()
    {        
       towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();        
    }

    public void SpawnTower()
    {
        randomType = Random.Range(0, towerPrefabs.Count);
        //if (towerPrefab == null) { Debug.LogError("[TowerSpawner] towerPrefab 미할당", this); return; }
        //if (towerSpottest == null) { Debug.LogError("[TowerSpawner] towerSpot 미할당", this); return; }

        foreach (var spot in towerSpot.spotPoints)
        {
           if(spot.isSpawning == false)
           {
                Debug.Log("타워 생성");
                var newtower = Instantiate(towerPrefabs[randomType], spot.point, Quaternion.identity);
                spot.isSpawning = true; // 자리 참 
                spot.tower = newtower; // 스폿이 타워 알고있게
                newtower.GetComponent<TowerHealth>().SetSpot(spot); // 타워가 자기 자리 알고있게                
                break;
            }
        }
    }
}
