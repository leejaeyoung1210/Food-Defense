using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    //private float SpawnTime;
    //private float SpawnIntaval=1.5f;
    //public GameObject enemyType;
    //public static int count = 0;
    private void Update()
    {
        //SpawnTime += Time.deltaTime;  
        //if(SpawnTime> SpawnIntaval && count < 5)
        //{
        //    SpawnTime = 0f;
        //    Spawn();
        //    count++;
        //}        
    }

    public void Spawn(EnemyData e,Vector2 towerpos)
    {
        GameObject enemy = Instantiate(e.pre,towerpos,Quaternion.identity);      
    }

   }
