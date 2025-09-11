using Unity.VisualScripting;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public GameObject towerPrefab;
    private TowerSpot towerSpot;


    //private GameObject[] towers;

    private int spotIndex;

    private void Awake()
    {        
        towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();
        
    }

    public void SpawnTower()
    {
        if (spotIndex >= towerSpot.Count)
        {
            Debug.Log("Full");
            return;
        }

        Vector3 pos = towerSpot.GetSpotPoint(spotIndex);
        Instantiate(towerPrefab, pos, Quaternion.identity);
        spotIndex++;

        
    }
}
