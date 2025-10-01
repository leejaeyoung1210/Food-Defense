using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI spawnCostText;

    public WaveManager waveTimer;

    //public Pause pas;

    public GameObject pause;
    public GameObject clearUi;
    public GameObject overUi;

    public TextMeshProUGUI clearWaveText;
    //public TextMeshProUGUI clearGoldText;
    //public TextMeshProUGUI clearRpText;


    public TextMeshProUGUI overWaveText;
    //public TextMeshProUGUI overGoldText;
    //public TextMeshProUGUI overRpText;

    public Button spawnBut;
    public AudioSource audio;

    public GameObject upPanel;
    public TextMeshProUGUI towerName;
    public TextMeshProUGUI upgradeGold;
    public TextMeshProUGUI sellGold;
    private Tower currentTower;
    public Button upgradeButton;

    public GameObject towerInformation;

    public GameObject towerSynergy;

    private bool justOpenPopup = false;

    public AudioSource sfxAudio;

    public AudioClip butClip;
    public AudioClip upSellClip;

    public AudioSource bgmAudio;


    public void TowerUIOpen()
    {
        sfxAudio.PlayOneShot(butClip);
        if (towerInformation != null)
        {
        
            towerInformation.SetActive(true);
            justOpenPopup = true;
        }
    }

    public void TowerUIClose()
    {
        if (towerInformation != null)
            towerInformation.SetActive(false);
    }
    //Synergy
    public void SynergyUIOpen()
    {
        sfxAudio.PlayOneShot(butClip);
        if (towerSynergy != null)
        {

            towerSynergy.SetActive(true);
            justOpenPopup = true;
        }
    }

    public void SynergyClose()
    {
        if (towerSynergy != null)
            towerSynergy.SetActive(false);
    }


    public bool CheckTouchInUI(GameObject target)
    {
        List<RaycastResult> targets = new List<RaycastResult>();
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        }, targets);

        foreach (var tar in targets)
        {
            if (tar.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }

    public void OpenUI(GameObject towerObject)
    {
        CloseUI();
        sfxAudio.PlayOneShot(butClip);
        Tower tower = towerObject.GetComponent<Tower>();
        currentTower = tower;

        if (upPanel != null)
            upPanel.SetActive(true);

        RefreshUI();
        Define.OnTowerCanvas = true;
    }


    public void Upgrade()
    {
        if (currentTower == null) return;
        sfxAudio.PlayOneShot(upSellClip);
        if (Define.gold >= currentTower.data.UpgradeCost && currentTower.data.Upgradeable)
        {
            Define.gold -= currentTower.data.UpgradeCost;
            var newData = DataTableManager.TowerTableData.Get(currentTower.data.NextId);
            currentTower.Init(newData);
            RefreshUI();
        }
    }

    public void CloseUI()
    {
        if (upPanel != null)
            upPanel.SetActive(false);

        currentTower = null;
        Define.OnTowerCanvas = false;
    }
    public void Sell()
    {
        if (currentTower == null) return;
        sfxAudio.PlayOneShot(upSellClip);
        Define.gold += currentTower.data.ResellPrice;

        var spot = currentTower.GetComponentInParent<TowerHealth>().GetSpot();
        if (spot != null)
        {
            spot.tower = null;
            spot.isSpawning = false;            
        }

        if (!spawnBut.interactable)
        {
            spawnBut.interactable = true;
        }

        Destroy(currentTower.gameObject);
        CloseUI();        
    }

    private void RefreshUI()
    {
        if (currentTower == null) return;

        towerName.text = currentTower.data.Name;
        // 업그레이드 비용
        if (currentTower.data.Level == 6)
        {
            upgradeGold.text = "X";  // 최종레벨이면 업그레이드 불가
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeGold.text = currentTower.data.UpgradeCost.ToString();
            upgradeButton.interactable = true;
        }

        // 판매 가격
        sellGold.text = currentTower.data.ResellPrice.ToString();
    }

    private void Update()
    {
        WaveLoad();
        GoldLoad();
        TimerLoad();
        SpawnCostLoad();
        if (Define.gameClear)
        {
            audio.Stop();
            GameClear();
        }

        if (Define.gameOver)
        {
            audio.Stop();
            GameOver();
        }

        if (Input.touchCount > 0)
        {
            if (upPanel.activeSelf && !CheckTouchInUI(upPanel))
            {
                CloseUI();
            }
            if (towerInformation.activeSelf && !justOpenPopup)
            {
                if (!CheckTouchInUI(towerInformation))
                {
                    TowerUIClose();
                }
            }
            if (towerSynergy.activeSelf && !justOpenPopup)
            {
                if (!CheckTouchInUI(towerSynergy))
                {
                    SynergyClose();
                }
            }
        }

       

        if(justOpenPopup)
        {
            justOpenPopup = false;
        }
    }

    private void WaveLoad()
    {
        waveText.text = $"Wave {Define.waveCount}";
    }

    private void GoldLoad()
    {
        goldText.text = $"{Define.gold}";
    }
    private void TimerLoad()
    {
        float time = Mathf.Max(0f, waveTimer.timer);
        int minute = (int)(time / 60f);
        int second = (int)(time % 60f);
        //timerText.text = $"{minute:D2}:{second:D2}"; //
        timerText.text = $"{time:F0}";
    }

    private void SpawnCostLoad()
    {
        spawnCostText.text = $"{Define.spawnCost}";
        if (Define.spawnCost > Define.gold)
        {
            spawnCostText.color = Color.red;
        }
        else { spawnCostText.color = Color.yellow; }
    }

    public void PauseOpen()
    {
        sfxAudio.PlayOneShot(butClip);
        pause.gameObject.SetActive(true);
        Time.timeScale = 0f;
        bgmAudio.Stop();

    }
    public void PauseClose()
    {
        bgmAudio.loop = true;   
        bgmAudio.Play();   

        pause.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GameClear()
    {
        clearWaveText.text = $"최종 웨이브: {Define.waveCount}";
        clearUi.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        overWaveText.text = $"최종 웨이브: {Define.waveCount}";
        overUi.gameObject.SetActive(true);
    }

    public void SetSpawnButton(bool isActive)
    {
        spawnBut.interactable = isActive;
    }

    public void SetTimerTextColor(Color color)
    {
        timerText.color = color;
    }



}



