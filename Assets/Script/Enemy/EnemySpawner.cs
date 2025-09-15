using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //private ObjectPooler pool;
    private void Update()
    {

    }

    public void Spawn(EnemyData e, Vector2 towerpos)
    {
        GameObject enemy = Instantiate(e.pre, towerpos, Quaternion.identity);
        //enemy.SetActive(true);
    }

}
