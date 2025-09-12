using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    private float SpawnTime;
    private float SpawnIntaval=1.5f;
    public GameObject preafab;

    private void Update()
    {
        SpawnTime += Time.deltaTime;  
        if(SpawnTime> SpawnIntaval)
        {
            SpawnTime = 0f;
            Spawn();            
        }        
    }

    private void Spawn()
    {
        GameObject enemy = Instantiate(preafab);
        enemy.transform.position = transform.position;
    }

   }
