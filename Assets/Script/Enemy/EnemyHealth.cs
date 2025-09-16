using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class EnemyHealth : Living
{
    public EnemyData data;
    public Slider healthSlider;
    //public Enemy enemy;
    private Animator anim;

    public static event System.Action<GameObject> OnAnyEnemyRemoved;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
    
    }
    protected override void OnEnable()
    {
        MaxHealth = data.MaxHp;        
        base.OnEnable();
        healthSlider = GetComponentInChildren<Slider>(true);
        healthSlider.value = health/MaxHealth;
        healthSlider.gameObject.SetActive(false);   
    }
  

    public override void OnDamage(float damage, Vector2 hitPoint)
    {
        anim.SetTrigger("Hit");
        healthSlider.gameObject.SetActive(true);
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health/MaxHealth;
    }

    protected override void Die()
    {
        anim.SetTrigger("Die");         
        base.Die();
        WaveManager.enemyTotalCount--;
        Define.Gold += 10;
        OnAnyEnemyRemoved?.Invoke(gameObject);
    }    
    
}
