using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 1f;
    
    private Vector3 targetposition;

    public WayPoint currentPath;
    private int currentPoint;

    public RectTransform bar;

    private void Awake()
    {
        currentPath = GameObject.FindWithTag("Spawn").GetComponent<WayPoint>(); 
    }
    private void OnEnable()
    {
        currentPoint = 0;
        targetposition = currentPath.GetWayPoint(currentPoint);
    }

    private void Update()
    {

        Vector3 screenpos = Camera.main.WorldToScreenPoint(transform.position);
        bar.position = screenpos;

        transform.position = Vector3.MoveTowards(transform.position, targetposition, enemySpeed * Time.deltaTime);

        float distanceToTarget = Vector3.Distance(transform.position, targetposition);  

        if (distanceToTarget<0.1f)
        {              
            if (currentPoint == currentPath.wayindex)
            {
                currentPoint = 0;
            }
            targetposition = currentPath.GetWayPoint(currentPoint);
            currentPoint++;
        }
           
    }


}
