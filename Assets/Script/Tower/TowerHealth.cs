using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TowerHealth : Living
{
    public TowerData data;
    public Slider healthSlider;

    private Spot mySpot;

    private TowerSpot towerSpot;

    public UIManager upui;

    private void Awake()
    {
        towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();
        upui = GameObject.FindWithTag("UiMgr").GetComponent<UIManager>();   
    }

    public void SetSpot(Spot spot)
    {
        mySpot = spot;
    }

    public Spot GetSpot()
    {
        if (mySpot == null) return null;
        
        return mySpot;  
    }

    public void AddData(int maxHp)
    {
        healthSlider = GetComponentInChildren<Slider>(true);
        MaxHealth = maxHp;
        health = MaxHealth;
        healthSlider.value = health / MaxHealth;
        healthSlider.gameObject.SetActive(false);
    }

    //protected override void OnEnable()
    //{
    //    MaxHealth = data.MaxHp;
    //    base.OnEnable();
    //    healthSlider = GetComponentInChildren<Slider>(true);
    //    healthSlider.value = health / MaxHealth;
    //    healthSlider.gameObject.SetActive(false);
    //}


    public override void OnDamage(float damage, Vector2 hitPoint)
    {        
        healthSlider.gameObject.SetActive(true);
        base.OnDamage(damage, hitPoint);
        healthSlider.value = health / MaxHealth;
        Debug.Log($"µ¥¹ÌÁö{damage}, {health}");
    }

    protected override void Die()
    {
        if (mySpot == null) return;

        base.Die();

        if (Define.OnTowerCanvas && upui != null)
        {
            upui.CloseUI();
        }
        mySpot.tower = null;
        mySpot.isSpawning = false;
        Define.OnTowerCanvas = false;
        towerSpot.TrySpawn();
        Destroy(gameObject);
    }
}
