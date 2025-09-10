using UnityEditor;
using UnityEngine;


public class WayPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waypoints;

   
   
    private void OnDrawGizmos()
    {

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;        
        style.fontSize = 18;

      
        for (int i = 0; i < waypoints.Length; i++)
        {
            Handles.Label(waypoints[i].transform.position + Vector3.up * 0.3f+Vector3.right *0.3f, $"{i + 1}", style);
            if (i < waypoints.Length - 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }
        }        
        Gizmos.DrawLine(waypoints[waypoints.Length - 1].transform.position, waypoints[0].transform.position);
       
    }

   
}
