using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;   

public class TowerSpawner : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    private TowerSpot towerSpot;
    private int randomType;
    private int randomSpot;


    //private GameObject[] towers;

    //private int spotIndex;

    private void Awake()
    {        
        towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();
        
    }

    public void SpawnTower()
    {
        randomType = Random.Range(0, towerPrefabs.Count);
        randomSpot = Random.Range(0, towerSpot.spotPoints.Length);

        //if (spotIndex >= towerSpot.Count)
        //{
        //    Debug.Log("Full");
        //    return;
        //}


        
        Vector3 pos = towerSpot.GetSpotPoint(randomSpot);
        //towerSpot.spotPoints[randomSpot] =   
        Instantiate(towerPrefabs[randomType], pos, Quaternion.identity);
       

        
    }
}
