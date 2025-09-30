using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class MagicProjectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;  
    private Animator animator;  
    Vector3 dir;
    private bool movestop = false;

    private float maxLifetime = 4f;
    private float currentLifetime = 0f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        movestop = false;
        currentLifetime = 0f;
    }
    public void Set(Transform target, float damage, float speed = 3f)
    {
        transform.SetParent(null);
        this.target = target;
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {

        currentLifetime += Time.deltaTime;
        if (currentLifetime >= maxLifetime)
        {           
            gameObject.SetActive(false);
            return;
        }
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }
        if (!movestop)
        {
            dir = (target.position - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != target) return;

        if (target == null || !target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }

        var hit = Physics2D.OverlapCircleAll(transform.position, 5f);        
        foreach (var h in hit)
        {
           if (h.CompareTag("Tower"))
           {                
                //if (collision.gameObject.activeInHierarchy == false) continue;
                //if (collision.gameObject.GetComponent<IDamagable>() == null) continue;
                //if (collision.gameObject.GetComponent<Living>().IsDead == true) continue;

                h.GetComponent<IDamagable>()?.OnDamage(damage, transform.position);
           }
        }

        animator.SetTrigger("Hit");
        transform.localScale = new Vector2(2f, 2.5f);
        movestop = true;
        StartCoroutine(AniEndig());   
    }
    IEnumerator AniEndig()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        transform.localScale = Vector2.one;
        movestop = false;
        gameObject.SetActive(false);
    }
}


