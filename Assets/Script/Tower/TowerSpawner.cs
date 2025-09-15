using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;   

public class TowerSpawner : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    private TowerSpot towerSpot;
    private int random;


    //private GameObject[] towers;

    private int spotIndex;

    private void Awake()
    {        
        towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();
        
    }

    public void SpawnTower()
    {
        random = Random.Range(0, towerPrefabs.Count);   

        if (spotIndex >= towerSpot.Count)
        {
            Debug.Log("Full");
            return;
        }

        Vector3 pos = towerSpot.GetSpotPoint(spotIndex);
        Instantiate(towerPrefabs[random], pos, Quaternion.identity);
        spotIndex++;

        
    }
}
