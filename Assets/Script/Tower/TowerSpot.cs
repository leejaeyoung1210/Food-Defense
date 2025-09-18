using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


[System.Serializable]
public class Spot
{
    [HideInInspector]
    public Vector3 point;
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

    //public int Count => spotPoints?.Count ?? 0;
    private void Awake()
    {
        //tilemap = GameObject.FindWithTag("Ground").GetComponent<Tilemap>();    
        MarkBuildableTiles(); //타일맵에서 설치 가능한 타일 셋팅 
    }

    public void MarkBuildableTiles()
    {
        if (tilemap == null) return;
        if( tileBase == null) return;   

        spotPoints.Clear(); //리스트 초기화
                
        var bounds = tilemap.cellBounds;

        for (int y = bounds.yMax - 1; y >= bounds.yMin; y--)
        {   
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                var cellPos = new Vector3Int(x, y, 0);
                var tile = tilemap.GetTile(cellPos);
                if (tile == tileBase)
                {
                    var world = tilemap.GetCellCenterWorld(cellPos);  
                    spotPoints.Add(new Spot { point = world, isSpawning = false });
                }                
            }
        }
    }

    public Vector3 GetSpotPoint(int count)
    {
        if (count < 0 || count >= spotPoints.Count) return Vector3.zero;
        return spotPoints[count].point;
    }

    public int FindAvailableSpot(Vector3 worldpos)
    {
        for (int i = 0; i < spotPoints.Count; i++)
        {
            if (Vector3.Distance(worldpos, spotPoints[i].point) < 0.3f && spotPoints[i].isSpawning == false)
            {
                return i; //비어있는 자리 인덱스 반환 
            }   

        }
        return -1; //없음 
    }   
}
