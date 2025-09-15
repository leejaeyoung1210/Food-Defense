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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spotPoints[i].position, spotPoints[i].localScale);
        }
    }
}
