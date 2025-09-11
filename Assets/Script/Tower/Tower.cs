using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData data;
    private CircleCollider2D collider;

    private void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        collider.radius = data.range;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.range * transform.lossyScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            Debug.Log("µé¾î¿È");
        }
    }


    




}
