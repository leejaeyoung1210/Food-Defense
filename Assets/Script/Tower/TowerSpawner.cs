using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;   

public class TowerSpawner : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    private TowerSpot towerSpot;
    private int randomType;    


    private void Awake()
    {        
        towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();        
    }

    public void SpawnTower()
    {
        randomType = Random.Range(0, towerPrefabs.Count);
        
        foreach(var spot in towerSpot.spotPoints)
        {
           if(spot.isSpawning == false)
           {
                var newtower = Instantiate(towerPrefabs[randomType], spot.point.position, Quaternion.identity);
                spot.isSpawning = true; // 자리 참 
                newtower.GetComponent<TowerHealth>().SetSpot(spot); // 타워가 자기 자리 알고있게
                Debug.Log("타워 생성됨");
                break;
            }
        }
    }
}
