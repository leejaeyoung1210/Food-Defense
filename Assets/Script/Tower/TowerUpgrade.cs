//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class TowerUpgrade : MonoBehaviour
//{
//    private Tower tower;

//    public TextMeshProUGUI upgradeGlod;

//    public TextMeshProUGUI sellGold;
   
//    private TowerSpot towerSpot;
//    private void Awake()
//    {
//        towerSpot = GameObject.FindWithTag("Spot").GetComponent<TowerSpot>();
//    }
//    private void Start()
//    {
//        tower = GetComponentInParent<Tower>();  //부모검사        
//    }

//    private void Update()
//    {
//        UpGold();
//        SellGold();
//    }


//    //public void Upgrade()
//    //{        
//    //    if (tower != null)
//    //    {
//    //        if (Define.gold >= tower.data.UpgradeCost)
//    //        {
//    //            if (tower.data.Upgradeable)
//    //            {
//    //                Define.gold -= tower.data.UpgradeCost;  
//    //                var table = DataTableManager.TowerTableData;
//    //                var data = table.Get(tower.data.NextId);
//    //                tower.Init(data);                    
//    //                Debug.Log(tower.data.Name);
//    //            }
//    //        }     
//    //    }
//    //}

//    public void Sell()
//    {
//        Debug.Log("누름");
//        Define.gold += tower.data.ResellPrice;
//        var towerHealth = tower.GetComponentInParent<TowerHealth>();
//        Spot spot = null;
//        if (towerHealth != null)
//        {
//            spot = towerHealth.GetSpot();
//        }

//        if (spot != null)
//        {
//            spot.tower = null;
//            spot.isSpawning = false;
//            towerSpot.TrySpawn();
//        }
//        Define.OnTowerCanvas = false;
//        Destroy(tower.gameObject);
//    }

//    public void Back()
//    {
//        Define.OnTowerCanvas = false;
//        var ui = transform.GetComponentInChildren<Canvas>(false);
//        ui.gameObject.SetActive(false);
//    }


//    private void UpGold()
//    {        
//        if(tower.data.Level ==6)
//        {
//            upgradeGlod.text = "X";
//            return;
//        }
//        upgradeGlod.text = $"{tower.data.UpgradeCost}";
//    }

//    private void SellGold()
//    {
//        sellGold.text = $"{tower.data.ResellPrice}";
//    }
//}
