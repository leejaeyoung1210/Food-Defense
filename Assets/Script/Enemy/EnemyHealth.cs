using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : Living
{
    public Slider healthSlider;  
    private Animator anim;

    public static event System.Action<GameObject> OnAnyEnemyRemoved;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }   
    public void AddData(int maxHp)
    {
        healthSlider = GetComponentInChildren<Slider>(true);
        MaxHealth = maxHp;
        health = MaxHealth;        
        healthSlider.value = health/MaxHealth;
        healthSlider.gameObject.SetActive(false);
    }
    public override void OnDamage(float damage, Vector2 hitPoint)
    {
        anim.SetTrigger("Hit");
        healthSlider.gameObject.SetActive(true);
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health / MaxHealth;
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
        OnAnyEnemyRemoved?.Invoke(gameObject);//포탑 배열에서 지워지기 위함 
        Destroy(gameObject);
    }

}
