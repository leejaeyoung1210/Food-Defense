using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public TowerData data;
    private CircleCollider2D towerRange;
    private SpriteRenderer towerSptrite;

    private ObjectPooler pool;  

    public float attackIntaval = 1f;
    public float lastAttack;
    private List<GameObject> enemies = new List<GameObject>();

    private  GameObject effectObj;


    private void Awake()
    {
        towerRange = GetComponent<CircleCollider2D>();
        pool = GetComponent<ObjectPooler>();
        towerSptrite = GetComponentInChildren<SpriteRenderer>();        
    }

    public void Init(TowerData towerData)
    {
        data = towerData;
        if(towerData.spriteIcon ==null)
        {
            Debug.Log($"{towerData.Name},{towerData.Icon}");
        }
        towerSptrite.sprite = towerData.spriteIcon;
        towerRange.radius = data.Range;
        attackIntaval = data.AttackSpeed;
        var hp = GetComponent<TowerHealth>();
        hp.AddData(data.Hp);
        if (data.Type == TowerType.Warrior)
        {
            effectObj = transform.Find("Effect")?.gameObject;
        }

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
            switch (data.Type)
            {
                case TowerType.Warrior:
                    Attack(targetGo);
                    break;
                case TowerType.Arrow:
                    Shot(targetGo);
                    break;
                case TowerType.Magic:
                    MagicShot(targetGo);
                    //for(int i = 0; i < enemies.Count; i++) //범위공격
                    //{
                    //    var t = enemies[i].GetComponent<IDamagable>();
                    //    if (t != null)
                    //    {
                    //        t.OnDamage(data.AttackPower, transform.position);
                    //    }
                    //}
                    break;
            }
        }
    }

    private void Attack(GameObject target)
    {
        effectObj.SetActive(true);
        target.GetComponent<IDamagable>().OnDamage(data.AttackPower, transform.position);
        StartCoroutine(Damage());  
    }

    IEnumerator Damage()
    {
        var ani = effectObj.GetComponent<Animator>();
        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);
        effectObj.SetActive(false);
    }

    private void Shot(GameObject target)
    {
        GameObject arrow = pool.GetPoolobject();
        arrow.transform.position = transform.position;
        arrow.SetActive(true);
     
        Projectile projectile = arrow.GetComponent<Projectile>();   
        projectile.Set(target.transform,data.AttackPower); 
    }
    private void MagicShot(GameObject target)
    {
        Debug.Log("Fire");
        GameObject ball = pool.GetPoolobject();
        ball.transform.position = transform.position;
        ball.SetActive(true);

        MagicProjectile magicprojectile = ball.GetComponent<MagicProjectile>();
        magicprojectile.Set(target.transform, data.AttackPower);
    }

}
