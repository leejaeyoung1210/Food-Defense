using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Search;

public class Tower : MonoBehaviour
{
    public TowerData data;
    private CircleCollider2D towerRange;
    //private int enemyLayer;


    public float attackIntaval = 0.5f;
    public float lastAttack;
    private List<GameObject> enemies = new List<GameObject>();


    private void Awake()
    {
        //enemyLayer = LayerMask.NameToLayer("Enemy");
        towerRange = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        towerRange.radius = data.range;
    }

    private void Update()
    {
          
    }

    private void OnTriggerEnter2D(Collider2D other)
    {  
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
            EnemyHealth.OnAnyEnemyRemoved += EnemyRemoved;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (lastAttack + attackIntaval < Time.time)
        {
            lastAttack = Time.time;
            var target = enemies[0].GetComponent<IDamagable>();
            if (target != null)
            {
                target.OnDamage(data.damage, transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
            EnemyHealth.OnAnyEnemyRemoved -= EnemyRemoved;
        }
    }


    private void EnemyRemoved(GameObject e)
    {
        enemies.Remove(e);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.range * transform.lossyScale.x);
    }


}
