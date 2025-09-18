using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using System.Collections;   

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
        StartCoroutine(Death());
        
    }

    IEnumerator Death()
    {
        Debug.Log("Enemy Dead");
        healthSlider.gameObject.SetActive(false);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        WaveManager.enemyTotalCount--;
        Define.gold += 20;
        OnAnyEnemyRemoved?.Invoke(gameObject);
        Destroy(gameObject);    
    }

}
