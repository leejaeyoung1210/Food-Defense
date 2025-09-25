using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MagicProjectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;  
    private Animator animator;  
    Vector3 dir;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Set(Transform target, float damage, float speed = 3f)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {     
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }
        dir = (target.position - transform.position).normalized;
        transform.position += dir * (speed * Time.deltaTime);
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
        animator.SetTrigger("Hit");
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
       
        gameObject.SetActive(false);
    }
}


