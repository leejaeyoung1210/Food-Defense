using UnityEngine;

public class Living : MonoBehaviour, IDamagable
{
    public float MaxHealth = 10f;

    public float health { get; protected set; }
    public bool IsDead { get; protected set; }
    protected virtual void OnEnable()
    {
        IsDead = false;
        health = MaxHealth;
    }

    public virtual void OnDamage(float damage, Vector2 hitPoint)
    {
        if (IsDead) return;
        health -= damage;   
        if(health <= 0 && !IsDead)
        {
            Die();
        }
    }

    protected virtual void Die() 
    {
        IsDead = true;
    }


}
