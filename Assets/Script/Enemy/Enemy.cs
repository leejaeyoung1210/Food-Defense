using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 1f;
    
    private Vector3 targetposition;

    public WayPoint currentPath;
    private int currentPoint;


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
        transform.position = Vector3.MoveTowards(transform.position, targetposition, enemySpeed * Time.deltaTime);

        float distanceToTarget = Vector3.Distance(transform.position, targetposition);  

        if (distanceToTarget<0.01f)
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
