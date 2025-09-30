using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


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

    private Material material;

    public Tilemap tilemap;

    private void Awake()
    {
        //material = GetComponent<SpriteRenderer>().material;
        tilemap = GameObject.FindWithTag("Spot").GetComponent<Tilemap>();
        material = Instantiate(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = material;

        enemyRange = GetComponent<CircleCollider2D>();
        currentPath = GameObject.FindWithTag("Spawn").GetComponent<WayPoint>();
        pool = GetComponent<ObjectPooler>();
        anim = GetComponent<Animator>();
    }


    public void Init(EnemyData enemyData)
    {
        Vector2 tileSize = tilemap.layoutGrid.cellSize;
        float tileWorldSizeX = tileSize.x;

        data = enemyData;
        enemySpeed = data.MoveSpeed;
        enemyRange.radius = data.Range* tileWorldSizeX;
        attackIntaval = data.AttackSpeed;
        var hp = GetComponent<EnemyHealth>();
        hp.AddData(data.Hp);

        if (data.Type == EnemyTypes.Knight)
        {
            effectObj = transform.Find("Effect")?.gameObject;
        }
        SetShader(enemyData.Level);
        Debug.Log($"À¯´Ö ·¹º§:    {enemyData.Level}");      

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


        transform.position = Vector2.MoveTowards(transform.position, targetposition, enemySpeed * Time.deltaTime);

        float distanceToTarget = Vector2.Distance(transform.position, targetposition);

        if (distanceToTarget < 0.01f)
        {

            if (currentPoint <= 9 && (currentPoint == 1 || currentPoint % 2 == 1 || currentPoint == 6))
            {
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            currentPoint++;
            if (currentPoint == currentPath.wayindex)
            {
                currentPoint = 0;
            }


            targetposition = currentPath.GetWayPoint(currentPoint);

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

    private void SetShader(int level)
  {
        //material.EnableKeyword("OUTBASE_ON");
        //material.EnableKeyword("OUTLINE_ON");
        switch (level)
        {
            case 1:               
                break;
            case 2:
                material.EnableKeyword("OUTBASE_ON");
                material.SetColor("_OutlineColor", new Color32(173, 255, 47, 255));
                material.SetFloat("_OutlineWidth", 0.005f);
                Debug.Log($"[Shader] OutlineColor: {material.GetColor("_OutlineColor")}");
                Debug.Log($"[Shader] OutlineWidth: {material.GetFloat("_OutlineWidth")}");
                Debug.Log($"[Shader] Distortion Amount: {material.GetFloat("_Amount")}");
                break;
            case 3:
                material.EnableKeyword("OUTBASE_ON");
                material.SetColor("_OutlineColor", new Color(0, 255, 255, 255));
                material.SetFloat("_OutlineWidth", 0.005f);
                //material.EnableKeyword("OUTLINE_USES_DISTORTION");
                material.SetFloat("_Amount", 0.05f);

                material.SetColor("_ShineColor", new Color32(225, 255, 224, 255));
                material.SetFloat("_ShineWidth", 0.08f);
                material.SetFloat("_ShineGlow", 0.1f);
                break;
            case 4:
                material.EnableKeyword("OUTBASE_ON");
                material.SetColor("_OutlineColor", new Color(255, 0, 255, 255));
                material.SetFloat("_OutlineWidth", 0.005f);
                //material.EnableKeyword("OUTLINE_USES_DISTORTION");
                material.SetFloat("_Amount", 0.05f);

                material.SetColor("_ShineColor", new Color32(255, 209, 255, 255));
                material.SetFloat("_ShineWidth", 0.08f);
                material.SetFloat("_ShineGlow", 0.1f);
                break;
            case 5:
                material.EnableKeyword("OUTBASE_ON");
                material.SetColor("_OutlineColor", new Color32(255, 69, 0, 255));
                material.SetFloat("_OutlineWidth", 0.005f);
                //material.EnableKeyword("OUTLINE_USES_DISTORTION");
                material.SetFloat("_Amount", 0.05f);

                material.SetFloat("_HsvShift", 38f);
                material.SetFloat("_HsvSaturation", 1f);
                material.SetFloat("_HsvBright", 2f);

                material.SetColor("_ShineColor", new Color32(255, 244, 194, 255));
                material.SetFloat("_ShineWidth", 0.08f);
                material.SetFloat("_ShineGlow", 0.1f);
                break;
            case 6:
                material.EnableKeyword("OUTBASE_ON");
                material.SetColor("_OutlineColor", new Color32(255, 215, 0, 255));
                material.SetFloat("_OutlineWidth", 0.005f);
                //material.EnableKeyword("OUTLINE_USES_DISTORTION");
                material.SetFloat("_Amount", 0.05f);

                material.SetFloat("_HsvShift", 180f);
                material.SetFloat("_HsvSaturation", 1f);
                material.SetFloat("_HsvBright", 2f);

                material.SetColor("_ShineColor", new Color32(255, 255, 255, 255));
                material.SetFloat("_ShineWidth", 0.08f);
                material.SetFloat("_ShineGlow", 0.1f);

                break;
        }
    }

}
