using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    private float SpawnTime;
    private float SpawnIntaval=1.5f;
    public GameObject preafab;
    public static int count = 0;
    private void Update()
    {
        SpawnTime += Time.deltaTime;  
        if(SpawnTime> SpawnIntaval && count < 5)
        {
            SpawnTime = 0f;
            Spawn();
            count++;
        }        
    }

    private void Spawn()
    {
        GameObject enemy = Instantiate(preafab);
        enemy.transform.position = transform.position;
    }

   }
