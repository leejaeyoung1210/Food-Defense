using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Search;

public class Tower : MonoBehaviour
{
    public TowerData data;
    private CircleCollider2D towerRange;


    public float attackIntaval = 1f;
    public float lastAttack;
    private List<GameObject> enemies = new List<GameObject>();


    private void Awake()
    {
        towerRange = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        towerRange.radius = data.range;
    }

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyRemoved += EnemyRemoved;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (lastAttack + attackIntaval < Time.time)
        {
            lastAttack = Time.time;
            if (enemies.Count > 0)
            {
                var target = enemies[0].GetComponent<IDamagable>();
                if (target != null)
                {
                    target.OnDamage(data.damage, transform.position);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
    }
    private void OnDisable()
    {
        EnemyHealth.OnAnyEnemyRemoved -= EnemyRemoved;
    }

    private void EnemyRemoved(GameObject e)
    {
        enemies.Remove(e);
        e.gameObject.SetActive(false);  
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.range * transform.lossyScale.x);
    }


}
