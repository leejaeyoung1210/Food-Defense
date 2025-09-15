using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 1f;
    
    private Vector3 targetposition;

    public WayPoint currentPath;
    private int currentPoint;
    public EnemyData data;


    private void Awake()
    {
        currentPath = GameObject.FindWithTag("Spawn").GetComponent<WayPoint>(); 
    }
    private void OnEnable()
    {
        currentPoint = 0;
        //enemySpeed = data.moveSpeed;    
        targetposition = currentPath.GetWayPoint(currentPoint);
    }

    private void Update()
    {
        Vector2 oldpos = transform.position;    
        transform.position = Vector3.MoveTowards(transform.position, targetposition, enemySpeed * Time.deltaTime);

        float xscale = transform.position.x - oldpos.x; 

        transform.localScale = new Vector3(Mathf.Sign(xscale), 1, 1);


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
