using System.IO;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public float enemySpeed = 1f;

    private Vector3 targetposition;

    public WayPoint currentPath;
    private int currentPoint;


    private CircleCollider2D enemyRange;
    public float attackIntaval = 1f;
    public float lastAttack;

    private ObjectPooler pool;

    private Animator anim;  
    private EnemyHealth enemyHealth => GetComponent<EnemyHealth>();


    private void Awake()
    {
        enemyRange = GetComponent<CircleCollider2D>();
        currentPath = GameObject.FindWithTag("Spawn").GetComponent<WayPoint>();
        pool = GetComponent<ObjectPooler>();
        anim = GetComponent<Animator>();    
    }

    private void Start()
    {
        enemySpeed = data.moveSpeed;    
        enemyRange.radius = data.range;
        attackIntaval = data.attackInterval;
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
        switch (data.enemyType)
        {            
            case EnemyType.Warrior:
                Debug.Log("Melee Attack");  
                other.GetComponent<TowerHealth>().OnDamage(data.damage, transform.position);        
                break;
            case EnemyType.Archer:
                Debug.Log("Archer Attack");
                Shot(other);
                break;
            case EnemyType.Wizard:
                Debug.Log("wizard Attack");
                MagicShot(other);
                break;
        }
    }
    private void Shot(GameObject target)
    {
        Debug.Log("Shot");
        GameObject arrow = pool.GetPoolobject();
        arrow.transform.position = transform.position;
        arrow.SetActive(true);

        Projectile projectile = arrow.GetComponent<Projectile>();
        projectile.Set(target.transform, data.damage);
    }

    private void MagicShot(GameObject target)
    {
        Debug.Log("Fire");
        GameObject ball = pool.GetPoolobject();
        ball.transform.position = transform.position;
        ball.SetActive(true);

        MagicProjectile magicprojectile = ball.GetComponent<MagicProjectile>();
        magicprojectile.Set(target.transform, data.damage);
    }


}
