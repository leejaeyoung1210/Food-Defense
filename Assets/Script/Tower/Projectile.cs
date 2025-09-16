using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;
    private Rigidbody2D rb;

    Vector3 dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != target) return;


        collision.GetComponent<IDamagable>()?.OnDamage(damage, transform.position);
        gameObject.SetActive(false);
    }
}


