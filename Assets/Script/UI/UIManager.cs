using NUnit.Framework.Interfaces;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
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

    public AudioSource audio;



    private void Awake()
    {
        //pause = GameObject.FindWithTag("Pause");
    }
           
    private void Update()
    {
        WaveLoad();
        GoldLoad();
        TimerLoad();
        SpawnCostLoad();
        if(Define.gameClear)
        {
            audio.Stop();   
            GameClear();
        }

        if (Define.gameOver)
        {
            audio.Stop();
            GameOver();
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
        pause.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void PauseClose()
    {
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

}



