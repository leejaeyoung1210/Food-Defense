using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    
    public Transform[] spotPoints;
    public bool[] isSpawning;

    public bool isOn = false;

    public int Count => spotPoints?.Length ?? 0;

    private void Awake()
    {
        isSpawning = new bool[spotPoints.Length];
        for(int i = 0; i < isSpawning.Length; i++)
        {
            isSpawning[i] = false;
        }   
    }   
    public Vector2 GetSpotPoint(int count)
    {
        return spotPoints[count].transform.position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spotPoints[i].position, spotPoints[i].localScale);
        }
    }
}
