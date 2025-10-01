using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class waypoint
{
    public GameObject way;
    public List<Transform> points;
}

public class StageManager : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;
    public List<Sprite> backgrounds;
    public List<Tilemap> tilemaps;
    public List<waypoint> waypoints;

    public GameObject enemyspawner;

    public TowerSpawner towerSpawner;

    public List<TowerSpot> towerSpots;

    private int currentStage = 0;

    void Start()
    {
        backgroundRenderer.sprite = backgrounds[currentStage];
        tilemaps[currentStage].gameObject.SetActive(true);  
        waypoints[currentStage].way.SetActive(true);        
        enemyspawner.gameObject.SetActive(false);
        towerSpots[currentStage].MarkBuildableTiles();
        towerSpawner.SetTowerSpot(towerSpots[currentStage]);
    }

    public void LoadStage(int stageIndex)
    {
        if(stageIndex <0) return;
        

        waypoints[currentStage].way.SetActive(false);                
        tilemaps[currentStage].gameObject.SetActive(false);

        ClearSpots(currentStage);

        backgroundRenderer.sprite = backgrounds[stageIndex];
        tilemaps[stageIndex].gameObject.SetActive(true);
        waypoints[stageIndex].way.SetActive(true);
        towerSpots[stageIndex].MarkBuildableTiles();
        towerSpawner.SetTowerSpot(towerSpots[stageIndex]);  


        if (stageIndex ==10)
        {
            enemyspawner.SetActive(true);
        }     

        currentStage = stageIndex;
    }

    private void ClearSpots(int index)
    {
        foreach (var spot in towerSpots[index].spotPoints)
        {
            if (spot.tower != null)
            {
                Debug.Log("타워 제거"); 
                Define.gold += spot.tower.GetComponent<Tower>().data.ResellPrice; //타워 매각시 골드 증가
                Destroy(spot.tower); // 스폿 위 타워 제거
            }
        }
        towerSpots[index].spotPoints.Clear();
    }
}
