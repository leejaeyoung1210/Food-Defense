using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class TowerSpawner : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    public TowerSpot towerSpot;
    private int randomType;   

    //private void Awake()
    //{
    //    towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();
    //}

    public void GoldCheat()
    {
        Define.gold += 100;
    }

    public void SpawnTower()
    {
        randomType = Random.Range(0, towerPrefabs.Count);

        if (Define.gold < Define.spawnCost)
        {
            Debug.Log("골드 부족");
            return;
        }
        else
        {
            //foreach (var spot in towerSpot.spotPoints)
            //{
            //    if (spot.isSpawning == false)
            //    {
            //        Mathf.Max(0, Define.gold -= Define.spawnCost);
            //        Debug.Log("타워 생성");
            //        var newtower = Instantiate(towerPrefabs[randomType], spot.point, Quaternion.identity);
            //        spot.isSpawning = true;
            //        Debug.Log(newtower.name);
            //        spot.tower = newtower; // 스폿이 타워 알고있게
            //        newtower.GetComponent<TowerHealth>().SetSpot(spot); // 타워가 자기 자리 알고있게                    
            //        Define.spawnCost += 1;
            //        break;
            //    }
            //}

            for (int i = 0; i < towerSpot.spotPoints.Count; i++)
            {
                if (towerSpot.spotPoints[i].isSpawning == false)
                {                   
                    Define.gold = Mathf.Max(0, Define.gold -= Define.spawnCost);                    
                    GameObject newtower = Instantiate(towerPrefabs[randomType], towerSpot.spotPoints[i].point, Quaternion.identity);
                    towerSpot.spotPoints[i].isSpawning = true;                    
                    towerSpot.spotPoints[i].tower = newtower;                    
                    newtower.GetComponent<TowerHealth>().SetSpot(towerSpot.spotPoints[i]); // 타워가 자기 자리 알고있게                    
                    Define.spawnCost += 1;
                    break;
                }
            }

            return;
        }
    }
}
