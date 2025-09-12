using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Living
{
    public Slider healthSlider;
    public Enemy enemy;

    public static event System.Action<GameObject> OnAnyEnemyRemoved;
    protected override void OnEnble()
    {
        base.OnEnble();

        healthSlider.value = health/MaxHealth;
        healthSlider.enabled = false;
    }

    public override void OnDamage(float damage, Vector2 hitPoint)
    {
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health/MaxHealth;
    }

    protected override void Die()
    {
        base.Die();

        gameObject.SetActive(false);

    }
    void OnDisable()  
    {
        OnAnyEnemyRemoved?.Invoke(gameObject);
    }



}
