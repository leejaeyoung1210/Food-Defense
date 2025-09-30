using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class EnemyHealth : Living
{
    public Slider healthSlider;
    private Animator anim;

    public static event System.Action<GameObject> OnAnyEnemyRemoved;

    public GameObject damageTextPrefab;

    private int gold = 4;

    private void Awake()
    {
        anim = GetComponent<Animator>();


    }
    public void AddData(int maxHp)
    {
        healthSlider = GetComponentInChildren<Slider>(true);
        MaxHealth = maxHp;
        health = MaxHealth;
        healthSlider.value = health / MaxHealth;
        healthSlider.gameObject.SetActive(false);
    }
    public override void OnDamage(float damage, Vector2 hitPoint)
    {

        anim.SetTrigger("Hit");
        healthSlider.gameObject.SetActive(true);
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health / MaxHealth;

        ShowDamageText((int)damage);    
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
        Define.gold += gold + Define.waveCount;
        OnAnyEnemyRemoved?.Invoke(gameObject);//포탑 배열에서 지워지기 위함 
        Destroy(gameObject);
    }

    private void ShowDamageText(int damage)
    {
        GameObject dmgObj = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        dmgObj.transform.SetParent(null); 
        var text = dmgObj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = damage.ToString();
        
        StartCoroutine(HideDamageText(dmgObj, text));
    }
    IEnumerator HideDamageText(GameObject dam, TextMeshProUGUI text)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector2 startPos = dam.transform.position;
        Vector2 endPos = startPos + new Vector2(startPos.x, startPos.y + 0.3f); 

        Color startColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float y = Mathf.Lerp(0.005f, 0.3f, t);
            dam.transform.position = new Vector2(startPos.x, startPos.y + y);
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

            yield return null;
        }

        Destroy(dam);
    }
}
