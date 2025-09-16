using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Search;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData data;
    private CircleCollider2D towerRange;
    

    private ObjectPooler pool;  

    public float attackIntaval = 1f;
    public float lastAttack;
    private List<GameObject> enemies = new List<GameObject>();


    private void Awake()
    {
        towerRange = GetComponent<CircleCollider2D>();
        pool = GetComponent<ObjectPooler>();    
    }

    private void Start()
    {
        towerRange.radius = data.range;
        attackIntaval = data.shootInterval;
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
                TypeAttack();
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

    private void TypeAttack()
    {
        GameObject targetGo = enemies[0];   
        var target = enemies[0].GetComponent<IDamagable>();
        if (target != null)
        {
            switch (data.towerType)
            {
                case TowerType.Warrior:
                    //타겟한테 애니메이션 당하느것도 제작 필요?
                    target.OnDamage(data.damage, transform.position);
                    break;
                case TowerType.Arrow:
                    Shot(targetGo);
                    break;
                case TowerType.Magic:
                    for(int i = 0; i < enemies.Count; i++)
                    {
                        var t = enemies[i].GetComponent<IDamagable>();
                        if (t != null)
                        {
                            t.OnDamage(data.damage, transform.position);
                        }
                    }
                    break;
            }
        }
    }

    private void Shot(GameObject target)
    {
        GameObject arrow = pool.GetPoolobject();
        arrow.transform.position = transform.position;
        arrow.SetActive(true);
     
        Projectile projectile = arrow.GetComponent<Projectile>();   
        projectile.Set(target.transform,data.damage); 


    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, data.range * transform.lossyScale.x);
    //}


}
