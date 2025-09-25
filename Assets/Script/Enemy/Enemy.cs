using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Collections;


public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public float enemySpeed = 1f;

    private Vector2 targetposition;

    public WayPoint currentPath;
    private int currentPoint;


    private CircleCollider2D enemyRange;
    public float attackIntaval = 1f;
    public float lastAttack;

    private ObjectPooler pool;

    private Animator anim;  
    private EnemyHealth enemyHealth => GetComponent<EnemyHealth>();
    private GameObject effectObj;

    private void Awake()
    {
        enemyRange = GetComponent<CircleCollider2D>();
        currentPath = GameObject.FindWithTag("Spawn").GetComponent<WayPoint>();
        pool = GetComponent<ObjectPooler>();
        anim = GetComponent<Animator>();    

    }   

    public void Init(EnemyData enemyData)
    {
        data = enemyData;
        enemySpeed = data.MoveSpeed;
        enemyRange.radius = data.Range;
        attackIntaval = data.AttackSpeed;
        var hp = GetComponent<EnemyHealth>();
        hp.AddData(data.Hp);

        if (data.Type == EnemyTypes.Knight)
        {
            effectObj = transform.Find("Effect")?.gameObject;
        }
    }

    private void OnEnable()
    {
        currentPoint = 0;
        //enemySpeed = data.moveSpeed;    
        targetposition = currentPath.GetWayPoint(currentPoint);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
        {
            if (lastAttack + attackIntaval < Time.time)
            {
                lastAttack = Time.time;
                TypeAttack(other.gameObject);
            }
        }
    }

    private void Update()
    {
        if (enemyHealth.IsDead) return; 

        Vector2 oldpos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetposition, enemySpeed * Time.deltaTime);

        float xscale = transform.position.x - oldpos.x;

        transform.localScale = new Vector3(Mathf.Sign(xscale), 1, 1);


        float distanceToTarget = Vector3.Distance(transform.position, targetposition);

        if (distanceToTarget < 0.01f)
        {
            if (currentPoint == currentPath.wayindex)
            {
                currentPoint = 0;
            }

            targetposition = currentPath.GetWayPoint(currentPoint);
            currentPoint++;
        }
    }

    private void TypeAttack(GameObject other)
    {
        anim.SetTrigger("Attack");
        switch (data.Type)
        {            
            case EnemyTypes.Knight:
                Attack(other);
                break;
            case EnemyTypes.Archer:
                Debug.Log("Archer Attack");
                Shot(other);
                break;
            case EnemyTypes.Wizard:
                Debug.Log("wizard Attack");
                MagicShot(other);
                break;
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
        Debug.Log("Shot");
        GameObject arrow = pool.GetPoolobject(); 
        arrow.transform.position = transform.position;
        arrow.SetActive(true);

        Projectile projectile = arrow.GetComponent<Projectile>();
        projectile.Set(target.transform, data.AttackPower);
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
