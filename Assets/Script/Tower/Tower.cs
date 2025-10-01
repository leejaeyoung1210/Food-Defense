using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class Tower : MonoBehaviour
{
    public TowerData data;
    private CircleCollider2D towerRange;
    private SpriteRenderer towerSptrite;

    private ObjectPooler pool;  

    public float baseAttackInterval = 1f;   
    public float attackIntaval = 1f;
    public float lastAttack;
    private List<GameObject> enemies = new List<GameObject>();

    private  GameObject effectObj;

    public Tilemap tilemap;

    private float baseDamage = 0f;  
    private float damage = 0f; 

    private void Awake()
    {
        tilemap = GameObject.FindWithTag("Spot").GetComponent<Tilemap>(); 
        towerRange = GetComponent<CircleCollider2D>();
        pool = GetComponent<ObjectPooler>();
        towerSptrite = GetComponentInChildren<SpriteRenderer>();        
    }

    public void Init(TowerData towerData)
    {
       Vector2 tileSize = tilemap.layoutGrid.cellSize;
        float tileWorldSizeX = tileSize.x;

        data = towerData;
        baseDamage = data.AttackPower;  
        damage = baseDamage;    
        if (towerData.spriteIcon ==null)
        {
            Debug.Log($"{towerData.Name},{towerData.Icon}");
        }
        towerSptrite.sprite = towerData.spriteIcon;
        towerRange.radius = data.Range * tileWorldSizeX;
        //towerRange.radius = (data.Range * tileWorldSizeX) / 2f;
        baseAttackInterval = data.AttackSpeed;
        attackIntaval = baseAttackInterval;
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
        Vector2 dir = (target.transform.position - transform.position).normalized;
        float radius = 0.2f;


        effectObj.transform.localPosition = dir * radius;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        effectObj.transform.rotation = Quaternion.AngleAxis(angle-90f, Vector3.forward);

        effectObj.SetActive(true);
        target.GetComponent<IDamagable>().OnDamage(damage, transform.position);
        StartCoroutine(Damage());  
    }

    IEnumerator Damage()
    {      
        var ani = effectObj.GetComponent<Animator>();
        ani.Play("TowerAttack", -1, 0f);
        //ani.CrossFade("TowerAttack", 0f, -1, 0f);
        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);
        effectObj.SetActive(false);
        effectObj.transform.rotation = Quaternion.identity;
        effectObj.transform.position = transform.position;
    }

    private void Shot(GameObject target)
    {
        GameObject arrow = pool.GetPoolobject();
        arrow.transform.position = transform.position;
        arrow.SetActive(true);
     
        Projectile projectile = arrow.GetComponent<Projectile>();   
        projectile.Set(target.transform, damage); 
    }
    private void MagicShot(GameObject target)
    {
        GameObject ball = pool.GetPoolobject();
        ball.transform.position = transform.position;
        ball.SetActive(true);

        MagicProjectile magicprojectile = ball.GetComponent<MagicProjectile>();
        magicprojectile.Set(target.transform, damage);
    }

    public void ApplyBuff(float power,float speed)
    {
        damage = baseDamage + (baseDamage * power);
        attackIntaval = Mathf.Max(0.1f, baseAttackInterval - (baseAttackInterval * speed));
    }  

    public void ResetBuff()
    {
        damage = baseDamage;
        attackIntaval = baseAttackInterval;
    }

}
