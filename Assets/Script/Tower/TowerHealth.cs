using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TowerHealth : Living
{
    public TowerData data;
    public Slider healthSlider;
    //public Enemy enemy;

    //public static event System.Action<GameObject> OnAnyEnRemoved;

    protected override void OnEnable()
    {
        MaxHealth = data.MaxHp;
        base.OnEnable();
        healthSlider = GetComponentInChildren<Slider>(true);
        healthSlider.value = health / MaxHealth;
        healthSlider.gameObject.SetActive(false);
    }


    public override void OnDamage(float damage, Vector2 hitPoint)
    {
        healthSlider.gameObject.SetActive(true);
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health / MaxHealth;
    }

    protected override void Die()
    {
        base.Die();        
        Destroy(gameObject);  
        //OnAnyEnemyRemoved?.Invoke(gameObject);
    }
}
