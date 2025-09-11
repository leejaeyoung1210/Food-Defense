using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    [SerializeField]
    private Transform[] spotPoints;

    public int Count => spotPoints?.Length ?? 0;
    
    public Vector3 GetSpotPoint(int count)
    {
        return spotPoints[count].transform.position;
    }
}
