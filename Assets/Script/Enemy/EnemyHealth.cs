using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class EnemyHealth : Living
{
    public EnemyData data;
    public Slider healthSlider;
    public Enemy enemy;
 

    public static event System.Action<GameObject> OnAnyEnemyRemoved;

 

    protected override void OnEnable()
    {
        MaxHealth = data.MaxHp;        
        base.OnEnable();
        healthSlider = GetComponentInChildren<Slider>(true);
        healthSlider.value = health/MaxHealth;
        
    }
    private void Update()
    {
       
    }

    public override void OnDamage(float damage, Vector2 hitPoint)
    {
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health/MaxHealth;
        Debug.Log($"공격받음 {healthSlider.value}");
    }

    protected override void Die()
    {
        base.Die();
        GetComponent<Collider2D>().enabled = false;
        EnemySpawner.count -= 1;
        Destroy(gameObject); 

    }
    void OnDisable()  
    {
        OnAnyEnemyRemoved?.Invoke(gameObject);
    }



}
