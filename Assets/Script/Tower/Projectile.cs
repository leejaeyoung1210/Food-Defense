using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;

    public void Set(Transform target,  float damage, float speed = 5f)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;

    }

    private void Update()
    {
        Vector3 dir;
        if(target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        dir = (target.position - transform.position).normalized;
        transform.position += dir * (speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform != target) return;
        collision.GetComponent<IDamagable>()?.OnDamage(damage, transform.position);
        Debug.Log("Hit");
        gameObject.SetActive(false);   
    }
}

 
