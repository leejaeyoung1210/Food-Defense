//using Unity.VisualScripting;
//using UnityEngine;
//using System.Collections.Generic;

//public class TowerSpawner : MonoBehaviour
//{
//    //public List<GameObject> towerPrefabs;
//    public TowerSpot towerSpot;

//    public void GoldCheat()
//    {
//        Define.gold += 100;
//    }

//    public void SpawnTower()
//    {
        
//        if (Define.gold < Define.spawnCost)
//        {
//            Debug.Log("골드 부족");
//            return;
//        }

//        List<TowerData> towerList = new List<TowerData>(DataTableManager.TowerTableData.GetAll());
//        if (towerList.Count == 0)
//        {
//            Debug.Log("비어있음");
//            return;
//        }

//        float totalWeight = 0f;
//        foreach (var tower in towerList) //돌면서 다 넣어주고 
//        {
//            float w = Mathf.Max(0, tower.Summonprobability);
//            totalWeight += w;
//        }

//        TowerData randomTower = null;
//        if (totalWeight <= 0f)
//        {
//            randomTower = towerList[Random.Range(0, towerList.Count)];
//        }
//        else
//        {
         
//            float r = Random.Range(0f, totalWeight);
//            float acc = 0f;
//            foreach (var t in towerList)
//            {
//                acc += Mathf.Max(0f, t.Summonprobability);
//                if (r <= acc)
//                {
//                    randomTower = t;
//                    break;
//                }
//            }
//            // 부동소수점 경계 보호
//            if (randomTower == null) randomTower = towerList[towerList.Count - 1];
//        }


//        for (int i = 0; i < towerSpot.spotPoints.Count; i++)
//        {
//            if (towerSpot.spotPoints[i].isSpawning == false)
//            {
//                Define.gold = Mathf.Max(0, Define.gold -= Define.spawnCost);
//                GameObject newtower = Instantiate(randomTower.prefab, towerSpot.spotPoints[i].point, Quaternion.identity);
//                towerSpot.spotPoints[i].isSpawning = true;
//                towerSpot.spotPoints[i].tower = newtower;
//                newtower.GetComponent<TowerHealth>().SetSpot(towerSpot.spotPoints[i]); // 타워가 자기 자리 알고있게
//                newtower.GetComponent<Tower>().Init(randomTower);                                                                                   
//                Define.spawnCost += 1;
//                break;
//            }
//        }

//        return;
//    }
//}
