using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public class Spot
{
    [HideInInspector]
    public Vector2 point;
    [HideInInspector]
    public bool isSpawning;
    [HideInInspector]
    public GameObject tower;
        
}

public class TowerSpot : MonoBehaviour
{
    public Tilemap tilemap; //타일맵 가져옴 사이즈 알아야하니까
    public TileBase tileBase; //설치 가능한 타일 검사할거임

    public List<Spot> spotPoints = new List<Spot>();

    private UIManager uiManager;

    //public int Count => spotPoints?.Count ?? 0;
    private void Awake()
    {
        uiManager = GameObject.FindWithTag("UiMgr").GetComponent<UIManager>();   
        //tilemap = GameObject.FindWithTag("Ground").GetComponent<Tilemap>();    
        MarkBuildableTiles(); //타일맵에서 설치 가능한 타일 셋팅 
    }

    public void MarkBuildableTiles()
    {
        if (tileBase == null) return;

        spotPoints.Clear(); //리스트 초기화

        var bounds = tilemap.cellBounds;

        for (int y = bounds.yMax - 1; y >= bounds.yMin; y--)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                var cellPos = new Vector3Int(x, y,0);
                var tile = tilemap.GetTile(cellPos);
                if (tile == tileBase)
                {
                    var world = tilemap.GetCellCenterWorld(cellPos);
                    spotPoints.Add(new Spot { point = world, isSpawning = false });
                }
            }
        }
    }

    public Vector2 GetSpotPoint(int index)
    {
        if (index < 0 || index >= spotPoints.Count) return Vector2.zero;
        return spotPoints[index].point;
    }



    public Spot FindAvailableSpot(Vector2 worldpos) //
    {
        for (int i = 0; i < spotPoints.Count; i++)
        {
            if (Vector2.Distance(worldpos, spotPoints[i].point) < 0.25f) //&& spotPoints[i].isSpawning == false
            {
                Debug.Log("여기 스폿이에요");
                return spotPoints[i]; //선택자리의 정보전달
            }
        }
        Debug.Log("여기는 스폿이아니에요");
        return null;  
    }

    public bool AllSpot()
    {
        foreach (var spot in spotPoints)
        {
            if(!spot.isSpawning)
            {
                return false;
            }            
        }
        return true;
    }

    public void TrySpawn()
    {
        if(!AllSpot())
        {
            uiManager.SetSpawnButton(true);
        }
    }
}
