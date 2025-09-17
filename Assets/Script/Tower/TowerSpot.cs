using UnityEngine;

[System.Serializable]
public class Spot
{
    public Transform point;
    [HideInInspector]
    public bool isSpawning;
}

public class TowerSpot : MonoBehaviour
{
    public Spot[] spotPoints;

    public int Count => spotPoints?.Length ?? 0;

    public Transform GetSpotPoint(int count) // 
    {
        return spotPoints[count].point;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spotPoints[i].point.position, spotPoints[i].point.localScale);
        }
    }
}
