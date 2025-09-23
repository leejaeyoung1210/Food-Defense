using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TowerHealth : Living
{
    public TowerData data;
    public Slider healthSlider;

    private Spot mySpot;

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
    }

    protected override void Die()
    {
        if (mySpot == null) return;

        base.Die();
        mySpot.tower = null;
        mySpot.isSpawning = false;
        Destroy(gameObject);
    }
}
